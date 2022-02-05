using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransportLayer;
using Utilities;

/// <summary>
/// BoardStateModel class contains all the information in order to represent the JSON string 
/// known as boardstate in C# as an object. This information is kept on the Server and the sends
/// the BoardStateView to the client to allow the UI to be updated. 
/// </summary>
#region SERVER ONLY 
public class BoardStateModel : MonoBehaviour
{
    private static string initialThisTurnDictData = "{\"CardsPlayed\":0,\"Attacks\":0,\"CentralActionsAvailable\":3,\"CentralActionsTaken\":0,\"DamageDealt\":0,\"DamageRecieved\":0,\"BarriersBroken\":0,\"BarriersBrokenByOpponent\":0,\"BarriersBuilt\":0,\"DamageRedirected\":0,\"CardsDeleted\":0,\"Deleted\":0,\"Despawned\":0,\"Destroyed\":0,\"Drawn\":0,\"Forged\":0,\"TimesHealed\":0,\"DamageHealed\":0,\"Initiated\":0,\"Locked\":0,\"Nulled\":0,\"Reacted\":0,\"Resolved\":0,\"Restored\":0,\"Rested\":0,\"Returned\":0,\"Searched\":0,\"Spawned\":0}";
    private static string initialOverGameDictData = "{\"CardsPlayed\":0,\"Attacks\":0,\"CentralActionsTaken\":0,\"DamageDealt\":0,\"DamageRecieved\":0,\"BarriersBroken\":0,\"BarriersBrokenByOpponent\":0,\"BarriersBuilt\":0,\"DamageRedirected\":0,\"CardsDeleted\":0,\"Deleted\":0,\"Despawned\":0,\"Destroyed\":0,\"Drawn\":0,\"Forged\":0,\"TimesHealed\":0,\"DamageHealed\":0,\"Initiated\":0,\"Locked\":0,\"Nulled\":0,\"Reacted\":0,\"Resolved\":0,\"Restored\":0,\"Rested\":0,\"Returned\":0,\"Searched\":0,\"Spawned\":0}";
    public static PlayerInfo PlayerInfoOne {get; set;}

    public static PlayerInfo PlayerInfoTwo { get; set; }
    public static BoardState BoardState { get; set; }

    void Awake()
    {
        PlayerInfoOne = new PlayerInfo();
        PlayerInfoTwo = new PlayerInfo();
        BoardState = new BoardState();
        BoardState.PlayerInfo = new List<PlayerInfo>();
        BoardState.PlayerInfo.Add(PlayerInfoOne);
        BoardState.PlayerInfo.Add(PlayerInfoTwo);

    }

    public static void SetInitialBoardState(Player p1, Player p2)
    {
        //called from the GameMaster after set up
        PlayerInfoOne.ThisTurn = JsonConvert.DeserializeObject<Dictionary<string, int>>(initialThisTurnDictData);
        PlayerInfoTwo.ThisTurn = JsonConvert.DeserializeObject<Dictionary<string, int>>(initialThisTurnDictData);
        PlayerInfoOne.OverGame = JsonConvert.DeserializeObject<Dictionary<string, int>>(initialOverGameDictData);
        PlayerInfoTwo.OverGame = JsonConvert.DeserializeObject<Dictionary<string, int>>(initialOverGameDictData);

        PlayerInfoOne.PlayerID = p1.ID;
        PlayerInfoTwo.PlayerID = p2.ID;

        foreach(ValidLocations l in Enum.GetValues(typeof(ValidLocations)))
        {
            PlayerInfoOne.LocationBoardStates.Add(l.ToString(), LocationBoardState.CreateLocationBoardState(p1.GetLocation(l)));
            PlayerInfoTwo.LocationBoardStates.Add(l.ToString(), LocationBoardState.CreateLocationBoardState(p2.GetLocation(l)));
        }
    }

    public static void UpdateBoardModel(JObject modelUpdate)
    {
        print(modelUpdate.ToString());

    }

    /// <summary>
    /// Conditions call this function, this is how they perform board checks
    /// </summary>
    /// <param name="id">GUID of the player to check</param>
    /// <param name="validLocation">Location enum of which Location to check</param>
    /// <returns>Specified Location object belonging to specified player</returns>
    public static Location CheckLocation(string id, ValidLocations validLocation)
    {
        return GameMaster.PlayerIdDict[id].GetLocation(validLocation);   
    }

    public static void SendNewBoardModel()
    {
        EventIngestion.SendBoardModelToView(BoardState);
    }
}
#endregion

#region CLIENT AND SERVER CLASSES. THIS WILL BE IN CLIENT CODE MINUS THE CONSTRUCTORS
public class MovedBy
{
    [JsonProperty]
    public string Action { get; set; }
    [JsonProperty]
    public string ActionOriginator { get; set; }
    [JsonProperty]
    public string Player { get; set; }
}

public class RecentMoves
{
    [JsonProperty]
    public string Destination { get; set; }
    [JsonProperty]
    public List<string> ObjectsMoved { get; set; }
    [JsonProperty]
    public int Count { get; set; }
    [JsonProperty]
    public MovedBy MovedBy { get; set; }
    [JsonProperty]
    public string PreviousMoveId { get; set; }

    public RecentMoves()
    {
        ObjectsMoved = new List<string>();
    }

    public static RecentMoves CreateRecentMoveRecord(LocationChanges locationChanges)
    {
        RecentMoves recentMove = new RecentMoves();
        foreach(Card card in locationChanges.c)
        {
            recentMove.ObjectsMoved.Add(card.Name);
            recentMove.Count += 1;
        }
        recentMove.PreviousMoveId = locationChanges.changeId;
        recentMove.Destination = locationChanges.destination.ValidName.ToString();

        throw new NotImplementedException(); //location changes needs to be exposed somehow
        //return recentMove;
    }
}

public class LocationBoardState
{
    [JsonProperty]
    public string LocationName { get; set; }
    [JsonProperty]
    public List<CardShell> Contents { get; set; }
    [JsonProperty]
    public int Count { get; set; }
    [JsonProperty]
    public List<RecentMoves> RecentMoves { get; set; }

    public LocationBoardState()
    {
        RecentMoves = new List<RecentMoves>();
        Contents = new List<CardShell>();
    }

    public static LocationBoardState CreateLocationBoardState(Location l)
    {
        LocationBoardState locationBoardState = new LocationBoardState();
        locationBoardState.LocationName = l.Name;
        locationBoardState.Count = l.Count;
        foreach(Card c in l)
        {
            locationBoardState.Contents.Add(new CardShell(c.Name, c.Abilities.Count));
        }

        return locationBoardState;
    }
    
}

public class PlayerInfo
{
    public string PlayerID { get; set; }
    public Dictionary<string, int> ThisTurn { get; set; }
    public Dictionary<string, int> OverGame { get; set; }
    public Dictionary<string, LocationBoardState> LocationBoardStates { get; set; }
    public List<Bond> Bonds { get; set; }

    public PlayerInfo()
    {
        ThisTurn = new Dictionary<string, int>();
        OverGame = new Dictionary<string, int>();
        LocationBoardStates = new Dictionary<string, LocationBoardState>();
        Bonds = new List<Bond>();

    }
}

public class CardShell
{
    public string Name { get; set; }
    public string InstanceID { get; set; }

    public List<AbilityShell> Abilities { get; private set; }

    public CardShell()
    {
        Abilities = new List<AbilityShell>();
    }

    public CardShell(string name, int ablCount)
    {
        Name = name;
        for(int i = 0; i < ablCount; i++)
        {
            Abilities.Add(new AbilityShell(i + 1)); //add 1 because ability count doesnt start at 0
                                                    //this allows the view to know which ability on a
                                                    //card is able to activate
        }
    }
}
public class BoardState
{
    public string Turnphase { get; set; }
    public int TurnCount { get; set; }
    public int Phase { get; set; }
    public string CurrentTurnPlayer { get; set; }
    public string RespondingPlayer { get; set; }
    public bool DimTwistReady { get; set; }
    public int DimTwistCount { get; set; }
    public List<PlayerInfo> PlayerInfo { get; set; }

}

public class Bond
{
    public string BondTarget { get; set; }
    public string BondObject { get; set; }

    public static Bond CreateBondRecord(GameEventsArgs e)
    {
        if(e.ActionEvent == NonMoveAction.Forge)
        {
            Bond bondRecord = new Bond();
            bondRecord.BondTarget = e.CardTargets[0].Name;
            bondRecord.BondObject = e.EventOriginCard.Name;
            return bondRecord;
        }
        else
        {
            throw new Exception("Error! Did not provide a Forge event");
        }
    }
}

public class AbilityShell
{
    public int AbilityIndex { get; set; }
    public bool CanActivate { get; set; }

    public AbilityShell(int index)
    {
        AbilityIndex = index;
        CanActivate = false;
    }
}

public class BoardStateRoot
{
    public BoardState BoardState { get; set; }
}


#endregion

