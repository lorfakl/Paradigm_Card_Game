using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DataBase;
using Utilities;

public class GameEventsManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private static Stack<GameEventsArgs> eventStack = new Stack<GameEventsArgs>();  //there's only ever gonna be one of these
    private static Queue<GameEventsArgs> eventQueue = new Queue<GameEventsArgs>();
    private static List<Card> tcBuffer = new List<Card>();
    private static Card activeLand;
    private static bool isTCDone = false;
    private static int numOfTurns = 0;
    private GameTimeManager gameTime = null;
    private Location uiPlayerReturnedLocation = null;
    private Location noUiPlayerReturnedLocation = null;
    private bool setUp = false;
    private List<Player> playerPool = new List<Player>();
    private int playerIndex = 0;
    
    private Player p1;
    private Player p2;
    

    public delegate void EventAddedHandler(object sender, GameEventsArgs data); //the delegate
    public static event EventAddedHandler NotifySubsOfEvent; // an instance of the delegate only ever gonna be one

    public Location UiPlayerReturnedLocation
    {
        get { return uiPlayerReturnedLocation; }
        set { uiPlayerReturnedLocation = value; }
    }

    public Location NoUiPlayerReturnedLocation
    {
        get { return noUiPlayerReturnedLocation; }
        set { noUiPlayerReturnedLocation = value; }
    }

    public Player UIPlayer
    {
        get { return gameTime.UIPlayer; }
    }

    public Player NonUIPlayer
    {
        get { return gameTime.NoUIPlayer; }
    }

    private static void OnEventAdd (object sender, GameEventsArgs data)
    {
        if(NotifySubsOfEvent != null) //if there are subcribers
        {
            NotifySubsOfEvent(sender,data);
        }
    }

    public static void AddTCLand(Card l)
    {
        tcBuffer.Add(l);
        Debug.Log("Player:" + l.getOwner().PlayerID + " selected " + l.getName() + " for Territory Challenge");
    }

    public Player GrabPlayer()
    {
        Player playReturned = playerPool[playerIndex];
        playerIndex++;
        return playReturned;
    }

    /// <summary>
    /// This function is used to add GameEvents to the queue, more logic is needed to differeniate GameEvents from one another
    /// Which will lead into adding things to the stack and doing timer things 
    /// </summary>
    /// <param name="e"></param>
    public static void PublishEvent(object s, GameEventsArgs e) 
    {                                                   
        eventQueue.Enqueue(e);
        OnEventAdd(s,e);
    }

    /// <summary>
    /// GameEventManager will end up attached to an empty gameobject when the game starts to well...manage game events
    /// Thats why it extends Monobehaviour and has Awake, Update, and Start functions
    /// </summary>
    
    //MOST CODE BELOW THIS LINE IS PURELY FOR TESTING AND WILL BE REMOVED AND REWORKED
    void Awake()
    {
        gameTime = new GameTimeManager();
        p1 = gameTime.UIPlayer;
        playerPool.Add(p1);

        p2 = gameTime.NoUIPlayer;
        playerPool.Add(p2);

        CardDataBase.MakePlayerDeck(p1);
        CardDataBase.MakePlayerDeck(p2);
        
        
        //this whole business should be handled in gametime object
        //p1 = new Player(5);
        //p2 = new Player();

        if( p1 == null || p2 == null)
        {
            Debug.Log("Player in EventManager is Null as fuck!");
        }

        

        p1.Majesty = p1.PlayerDeck.GetMajesty();
        p2.Majesty = p2.PlayerDeck.GetMajesty();


        Instantiate(player1);
        Instantiate(player2);

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!setUp)
        {
            Location p1Temp = UiPlayerReturnedLocation;

            Location p2Temp = NoUiPlayerReturnedLocation;
            if (p1Temp == null || p2Temp == null)
            {
                print("Someone hasnt selected a TC card yet");
                if(p1Temp == null)
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
                    gameTime.StartTerritoryChallenge(p1Temp.GetContents()[0], p2Temp.GetContents()[0]);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    print("One of the locations was null");
                    print(e.Message);
                    List<Card> p1Lands = p1.PlayerDeck.GetLandscapesInDeck();
                    List<Card> p2Lands = p2.PlayerDeck.GetLandscapesInDeck();
                    int i = UnityEngine.Random.Range(0, p1Lands.Count);
                    int j = UnityEngine.Random.Range(0, p2Lands.Count);
                    gameTime.StartTerritoryChallenge(p1Lands[i], p2Lands[j]);

                }
                setUp = true;
            }
        }
        


    }

    /// <summary>
    /// Will actually be a timer one of these days
    /// </summary>
    /// <param name="p"></param>
    /// <param name="temp"></param>
    /// <returns></returns>
 
}
