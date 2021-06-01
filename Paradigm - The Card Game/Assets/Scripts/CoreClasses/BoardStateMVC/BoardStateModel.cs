using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransportLayer;
using Utilities;

#region SERVER ONLY 
public class BoardStateModel : MonoBehaviour
{
   

    private static Dictionary<(string playerID, ValidLocations location), Location> boardStateModelDict = new Dictionary<(string playerID, ValidLocations location), Location>();

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
        //
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
        EventIngestion.SendBoardModelToView(boardStateModelDict);
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

    public RecentMoves()
    {
        ObjectsMoved = new List<string>();
    }

    public static RecentMoves CreateRecentMoveRecord(LocationChanges locationChanges)
    {
        RecentMoves recentMove = new RecentMoves();
        throw new NotImplementedException(); //location changes needs to be exposed somehow
        //return recentMove;
    }
}

public class LocationBoardState
{
    [JsonProperty]
    public string LocationName { get; set; }
    [JsonProperty]
    public List<string> Contents { get; set; }
    [JsonProperty]
    public int Count { get; set; }
    [JsonProperty]
    public List<RecentMoves> RecentMoves { get; set; }

    public LocationBoardState()
    {
        RecentMoves = new List<RecentMoves>();
        Contents = new List<string>();
    }
    
}

public class PlayerInfo
{
    public string PlayerID { get; set; }
    public Dictionary<string, int> ThisTurn { get; set; }
    public Dictionary<string, int> OverGame { get; set; }
    public List<LocationBoardState> LocationBoardStates { get; set; }
    public List<Bond> Bonds { get; set; }

    public PlayerInfo()
    {
        ThisTurn = new Dictionary<string, int>();
        OverGame = new Dictionary<string, int>();
        LocationBoardStates = new List<LocationBoardState>();
        Bonds = new List<Bond>();

    }
}

public class BoardState
{
    public string Turnphase { get; set; }
    public int TurnCount { get; set; }
    public int Phase { get; set; }
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
            throw new NotImplementedException();
        }
        else
        {
            throw new Exception("Error! Did not provide a Forge event");
        }
    }
}


public class BoardStateRoot
{
    public BoardState BoardState { get; set; }
}
#endregion

