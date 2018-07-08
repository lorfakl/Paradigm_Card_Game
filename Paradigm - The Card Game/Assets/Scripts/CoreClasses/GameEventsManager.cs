using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using DataBase;
using Utilities;

public class GameEventsManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public Text deckCount;
    public Text graveCount;
    public Text barrierCount;
    public Text nonUIDeckCount;
    public Text nonUIGraveCount;
    public Text nonUIHandCount;
    public Text nonUIBarrierCount;

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
    private Majesty p1Majesty;
    private Majesty p2Majesty;
    

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
        if(playerIndex == 2)
        {
            playerIndex = 0;
        }
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
        print("Made player deck?");
        CardDataBase.MakePlayerDeck(p2);

        p1.Majesty = p1.PlayerDeck.GetMajesty();
        p2.Majesty = p2.PlayerDeck.GetMajesty();
        print("Should be a full deck" + gameTime.NoUIPlayer.PlayerDeck.Count);
        foreach(Card c in gameTime.NoUIPlayer.PlayerDeck.GetContents())
        {
            print(c.Name);
        }

        p1.PlayerDeck.GameStartSetup();
        p2.PlayerDeck.GameStartSetup();

        if ( p1 == null || p2 == null)
        {
            Debug.Log("Player in EventManager is Null as fuck!");
        }

        Instantiate(player1);
        Instantiate(player2);
    }

    void Start()
    {
        deckCount = GameObject.FindWithTag("yourDeckCount").GetComponent<Text>();
        graveCount = GameObject.FindWithTag("yourGraveCount").GetComponent<Text>();
        barrierCount = GameObject.FindWithTag("yourBarrierCount").GetComponent<Text>();
        nonUIDeckCount = GameObject.FindWithTag("enemyDeckCount").GetComponent<Text>();
        nonUIGraveCount = GameObject.FindWithTag("enemyGraveCount").GetComponent<Text>();
        nonUIHandCount = GameObject.FindWithTag("enemyHandCount").GetComponent<Text>();
        nonUIBarrierCount = GameObject.FindWithTag("enemyBarrierCount").GetComponent<Text>();
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
                    playerPool = gameTime.StartTerritoryChallenge(p1Temp.GetContents()[0], p2Temp.GetContents()[0]);
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
                p1 = GrabPlayer();
                p2 = GrabPlayer();

            }
        }

        
        deckCount.text = "Deck: " + gameTime.UIPlayer.PlayerDeck.Count;
        graveCount.text = "Grave: " + gameTime.UIPlayer.GetLocation("Grave").Count;
        barrierCount.text = "Barriers: " + gameTime.UIPlayer.GetLocation("BZ").Count;
        nonUIDeckCount.text = "EDeck: " + gameTime.NoUIPlayer.PlayerDeck.Count;
        nonUIGraveCount.text = "EGrave: " + gameTime.NoUIPlayer.GetLocation("Grave").Count;
        nonUIHandCount.text = "EHand: " + gameTime.NoUIPlayer.GetLocation("Hand").Count;
        nonUIBarrierCount.text = "EBarriers: " + gameTime.NoUIPlayer.GetLocation("BZ").Count;
        
        print(gameTime.NoUIPlayer.PlayerDeck.Count);

        if (p1.Majesty.HP > 0 && p2.Majesty.HP > 0)
        {
            print("Playing the game");
        }


    }

  
 
}
