using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DataBase;
using Utilities;

public class EventManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject rendererManager;
    //public GameObject eventProcessor;
    public Text deckCount;
    public Text graveCount;
    public Text barrierCount;
    public Text nonUIDeckCount;
    public Text nonUIGraveCount;
    public Text nonUIHandCount;
    public Text nonUIBarrierCount;

    private static Queue<GameEventsArgs> eventQueue = new Queue<GameEventsArgs>();
    private static List<Card> tcBuffer = new List<Card>();
    private static Card activeLand;
    private Location uiPlayerReturnedLocation = null;
    private Location noUiPlayerReturnedLocation = null;
    private bool setUp = false;
    private List<Player> playerPool = new List<Player>();
    private int playerIndex = 0;
    private Player p1;
    private Player p2;
    private Majesty p1Majesty;
    private Majesty p2Majesty;
    Player firtTurnPlayer;
    Player secTurnPlayer;



    public delegate void EventAddedHandler(object sender, GameEventsArgs data); //the delegate
    public static event EventAddedHandler NotifySubsOfEvent; // an instance of the delegate only ever gonna be one
    //Only functions that return void and have parameters of type object and GameEventsArgs can be called when 


    private static void OnEventAdd(object sender, GameEventsArgs data)
    {
        if (NotifySubsOfEvent != null) //if there are subcribers
        {
            NotifySubsOfEvent(sender, data);
        }
        
    }

    /// <summary>
    /// This function is used to add GameEvents to the queue, more logic is needed to differeniate GameEvents from one another
    /// Which will lead into adding things to the stack and doing timer things 
    /// </summary>
    /// <param name="e"></param>
    public static void PublishEvent(object s, GameEventsArgs e)
    {
        eventQueue.Enqueue(e);
        OnEventAdd(s, e);

        if(e.ActionEvent == NonMoveAction.GameEnd)
        {
            Destroy(GameObject.FindWithTag("Player"));
            Destroy(GameObject.FindWithTag("AiPlayer"));

            SceneManager.LoadScene("mainmenu");
            //throw new Exception("Game over somebody ran outta cards");
            

        }
    }

  


    public Location UiPlayerReturnedLocation
    {
        get { return uiPlayerReturnedLocation; }
        set { uiPlayerReturnedLocation = value; }
    }

    public Player UIPlayer
    {
        get { return p2; }
    }

    public Location NoUiPlayerReturnedLocation
    {
        get { return noUiPlayerReturnedLocation; }
        set { noUiPlayerReturnedLocation = value; }
    }

    public Player NonUIPlayer
    {
        get { return p1; }
    }

    public Queue<GameEventsArgs> GetQueue
    {
        get { return eventQueue; }
    }


    public static void AddTCLand(Card l)
    {
        tcBuffer.Add(l);
        Debug.Log("Player:" + l.getOwner().PlayerID + " selected " + l.getName() + " for Territory Challenge");
    }

    public Player GrabPlayer()
    {
        
        if(playerIndex == 2)
        {
            playerIndex = 0;
        }
        //print("prolly no players " + playerPool.Count);
        Player playReturned = playerPool[playerIndex];
        playerIndex++;
        return playReturned;
    }

    public PlayerInteraction GetPlayerInteraction(bool ai)
    {
        GameObject g = null;
        if (ai)
        {
            g = GameObject.FindWithTag("AiPlayer");
        }
        else
        {
           g = GameObject.FindWithTag("Player");
        }

        return g.GetComponent<PlayerInteraction>();

    }
    /// <summary>
    /// GameEventManager will end up attached to an empty gameobject when the game starts to well...manage game events
    /// Thats why it extends Monobehaviour and has Awake, Update, and Start functions
    /// </summary>
    
    //MOST CODE BELOW THIS LINE IS PURELY FOR TESTING AND WILL BE REMOVED AND REWORKED
    void Awake()
    {

        p1 = new HumanPlayer(52);
        playerPool.Add(p1);

        p2 = new AIPlayer(12);
        playerPool.Add(p2);
        Debug.Log("The desks are the same: " + p1.PlayerDeck.Equals(p2.PlayerDeck));
        Debug.Log("P1 Deck ID: " + p1.PlayerDeck.Owner.PlayerID);
        Debug.Log("P2 Deck ID: " + p2.PlayerDeck.Owner.PlayerID);
        p1.Majesty = p1.PlayerDeck.GetMajesty();
        //p1.Majesty.PrintData();
        p2.Majesty = p2.PlayerDeck.GetMajesty();
        //p2.Majesty.PrintData();
        
        p1.ListLocationSizes();
        print("Now other one");
        p2.ListLocationSizes();

        if ( p1 == null || p2 == null)
        {
            Debug.Log("Player in EventManager is Null as fuck!");
        }

        GameObject player1Obj = Instantiate(player1);
        GameObject player2Obj = Instantiate(player2);
        p1.GamePlayHook = player1Obj.GetComponent<PlayerInteraction>();
        p2.GamePlayHook = player2Obj.GetComponent<PlayerInteraction>();
    }

    void Start()
    {
        
        //print("Should still be a full deck" + gameTime.NoUIPlayer.PlayerDeck.Count);
        
    }

    // Update is called once per frame
    void Update()
    {

        //CheckPlayerInfo();
        if (!setUp)
        {
            Location p1Temp = UiPlayerReturnedLocation;

            Location p2Temp = NoUiPlayerReturnedLocation;
            if (p1Temp == null || p2Temp == null)
            {
                print("Someone hasnt selected a TC card yet");
                if (p1Temp == null)
                {
                    print("Human hasnt chosen");
                }
                else
                {
                    print("Ai hasnt chosen");
                }

            }
            else
            {
                try
                {
                    if (p2Temp.Owner.PlayerID == p1Temp.Owner.PlayerID)
                    {
                        throw new Exception("Somehow these are the same");
                    }
                    else
                    {
                        //Debug.Log("Not the same we are not the same");
                        //Debug.Log("UI PLayer location owner ID: " + UiPlayerReturnedLocation.Owner.PlayerID + " here's the card ID: " + UiPlayerReturnedLocation.GetContents()[0].Owner.PlayerID);
                        //Debug.Log("NOUI PLayer location owner ID: " + NoUiPlayerReturnedLocation.Owner.PlayerID + " here's the card ID: " + NoUiPlayerReturnedLocation.GetContents()[0].Owner.PlayerID);
                        //Debug.Log("p1 Card ID: " + UiPlayerReturnedLocation.GetContents()[0].Owner.PlayerID + "p2 Card ID: " + NoUiPlayerReturnedLocation.GetContents()[0].Owner.PlayerID);
                    }
                    playerPool = HelperFunctions.StartTerritoryChallenge(p1Temp.Content[0], p2Temp.Content[0]);
                    //print("Should be a slightly less full deck" + gameTime.NoUIPlayer.PlayerDeck.Count);
                    //print("Human  count Should be a slightly less full deck" + gameTime.UIPlayer.PlayerDeck.Count);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    print("One of the locations was null");
                    print(e.Message);
                    List<Card> p1Lands = p1.PlayerDeck.GetLandscapesInDeck();
                    List<Card> p2Lands = p2.PlayerDeck.GetLandscapesInDeck();
                    int i = UnityEngine.Random.Range(0, p1Lands.Count);
                    int j = UnityEngine.Random.Range(0, p2Lands.Count);
                    HelperFunctions.StartTerritoryChallenge(p1Lands[i], p2Lands[j]);

                }
                setUp = true;
                firtTurnPlayer = playerPool[0];
                secTurnPlayer = playerPool[1];

                if(firtTurnPlayer.Equals(secTurnPlayer))
                {
                    throw new Exception("Thery the same, TC eval function is fucked");
                }

                activeLand = p2.TCCard;

            }

            //firtTurnPlayer.PlayerTurn.Owner = 
            //secTurnPlayer.PlayerTurn

        }


        //print("Events enqueued: " + eventQueue.Count);

        print("Is anybody out there!?!: ");

        if (p1.IsPreparedToStart && p2.IsPreparedToStart)
        {
            //print("P1 HP: " + p1.GetPlayerUIStatus());
            //print("P2 HP: " + p2.GetPlayerUIStatus());
            if (p1.Majesty.HP > 0 && p2.Majesty.HP > 0)
            {
                for(int i = 0; i < playerPool.Count; i++)
                {
                    if (firtTurnPlayer.Equals(secTurnPlayer))
                    {
                        throw new Exception("Somehow these are the same");
                    }
                    print("Playing the game");
                    playerPool[i].PlayerTurn.StartTurn();
                    print("Status: " + playerPool[i].PlayerTurn.Owner.GetPlayerUIStatus());
                    //print("Are the turn object the same " + )
                    print("I Value: " + i);
                    //playerPool[i].PlayerTurn.StartTurn();
                   // print("Status: " + secTurnPlayer.PlayerTurn.Owner.GetPlayerUIStatus());
                }
                /* if (firtTurnPlayer.Equals(secTurnPlayer))
                {
                    throw new Exception("Somehow these are the same");
                }
                print("Playing the game");
                firtTurnPlayer.PlayerTurn.StartTurn();
                print("Status: " + firtTurnPlayer.PlayerTurn.Owner.GetPlayerUIStatus());
                secTurnPlayer.PlayerTurn.StartTurn();
                print("Status: " + secTurnPlayer.PlayerTurn.Owner.GetPlayerUIStatus());
                //Debug.Log("Is this the AI?: " + p1.PlayerID + " " + p2.PlayerID);*/
            }

        }
    }
 
}
