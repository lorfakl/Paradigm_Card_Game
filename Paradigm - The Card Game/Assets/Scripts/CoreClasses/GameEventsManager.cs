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

public class GameEventsManager : MonoBehaviour
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

    private static Stack<GameEventsArgs> eventStack = new Stack<GameEventsArgs>();  //there's only ever gonna be one of these
    private static Queue<GameEventsArgs> eventQueue = new Queue<GameEventsArgs>();
    private static List<Card> tcBuffer = new List<Card>();
    private static Card activeLand;
    private static int numOfTurns = 0;
    private GameTimeManager gameTime = null;
    private Location uiPlayerReturnedLocation = null;
    private Location noUiPlayerReturnedLocation = null;
    private bool setUp = false;
    private List<Player> playerPool = new List<Player>();
    private int playerIndex = 0;
    private int[] initalData;
    private Player p1;
    private Player p2;
    private Majesty p1Majesty;
    private Majesty p2Majesty;
    

    public delegate void EventAddedHandler(object sender, GameEventsArgs data); //the delegate
    public static event EventAddedHandler NotifySubsOfEvent; // an instance of the delegate only ever gonna be one
    //Only functions that return void and have parameters of type object and GameEventsArgs can be called when 

    /// <summary>
    /// the delegate for updating the UI, sends out a message to the RenderManager's subscriber
    /// </summary>
    /// <param name="data">Int array to update the values of the player and enemy card locations</param>
    /// <param name="uiHand">A list of cards that represent the cards that the player has in their hand</param>
    /// <param name="uiField">A list of cards that represent the cards that the player has on their field</param>
    /// <param name="noUiField">A list of cards that represent the cards that the enemy has on their field</param>
    /// <param name="noUiHand">A list of cards that represent the cards that the enemy has in their hand</param>
    public delegate void UIUpdateHandler(int[] data, List<Card> uiHand, List<Card> uiField, List<Card> noUiHand, List<Card> noUiField); 
    public static event UIUpdateHandler UpdateUI; 

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

    private static void OnPlayerInfoChange(int[] data, List<Card> handCards, List<Card> fieldCards, List<Card> enemyHandCards, List<Card> enemyFieldCards)
    {
        if(UpdateUI != null)
        {
            UpdateUI(data, handCards, fieldCards, enemyHandCards, enemyFieldCards);
        }
    }


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
        gameTime = new GameTimeManager();
        p1 = gameTime.UIPlayer;
        playerPool.Add(p1);

        p2 = gameTime.NoUIPlayer;
        playerPool.Add(p2);
        print("Litterally just added the shit" + playerPool.Count);
        CardDataBase.MakePlayerDeck(p1);
        print("Made player deck?");
        CardDataBase.MakePlayerDeck(p2);

        p1.Majesty = p1.PlayerDeck.GetMajesty();
        p2.Majesty = p2.PlayerDeck.GetMajesty();
        print("Should be a full deck" + gameTime.NoUIPlayer.PlayerDeck.Count);
        print("Human player ID:" + gameTime.UIPlayer.PlayerID);
        print("AI player ID:" + gameTime.NoUIPlayer.PlayerID);
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
        GameObject rm = Instantiate(rendererManager);
        rm.SendMessage("SetPlayers", playerPool);
        print("Should still be a full deck" + gameTime.NoUIPlayer.PlayerDeck.Count);
        int[] initalInfo = { UIPlayer.PlayerDeck.Count, UIPlayer.GetLocation(ValidLocations.Grave).Count, UIPlayer.GetLocation(ValidLocations.BZ).Count, UIPlayer.PlayerDeck.Count, UIPlayer.GetLocation("Grave").Count, UIPlayer.GetLocation("Hand").Count, UIPlayer.GetLocation("BZ").Count };
        initalData = initalInfo;
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
                    playerPool = gameTime.StartTerritoryChallenge(p1Temp.GetContents()[0], p2Temp.GetContents()[0]);
                    print("Should be a slightly less full deck" + gameTime.NoUIPlayer.PlayerDeck.Count);
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
                activeLand = p2.TCCard;

            }
        }


        //print("Events enqueued: " + eventQueue.Count);

        //print(gameTime.NoUIPlayer.PlayerDeck.Count);

        if (p1.IsPreparedToStart && p2.IsPreparedToStart)
        {
            if (p1.Majesty.HP > 0 && p2.Majesty.HP > 0)
            {
                print("Playing the game");
                p1.PlayerTurn.StartTurn();
                p2.PlayerTurn.StartTurn();
                //Debug.Log("Is this the AI?: " + p1.IsAI);
            }

        }
    }

   /// <summary>
   /// THIS IS GARBAGE USE THE OBSERVER PATTERN
   /// </summary>
    private void CheckPlayerInfo()
    {
        
        int[] data = { UIPlayer.PlayerDeck.Count, UIPlayer.GetLocation("Grave").Count, UIPlayer.GetLocation("BZ").Count, UIPlayer.PlayerDeck.Count, UIPlayer.GetLocation("Grave").Count, UIPlayer.GetLocation("Hand").Count, UIPlayer.GetLocation("BZ").Count };
        if(!initalData.SequenceEqual(data))
        {
            OnPlayerInfoChange(data, UIPlayer.GetLocation("Hand").GetContents(), UIPlayer.GetLocation("Field").GetContents(), NonUIPlayer.GetLocation("Hand").GetContents(), NonUIPlayer.GetLocation("Field").GetContents());
            initalData = data;
        }
    }
 
}
