using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DataBase;

public class GameEventsManager : MonoBehaviour
{
    private static Stack<GameEventsArgs> eventStack = new Stack<GameEventsArgs>();  //there's only ever gonna be one of these
    private static Queue<GameEventsArgs> eventQueue = new Queue<GameEventsArgs>();
    private static List<Card> tcBuffer = new List<Card>();
    private static Card activeLand;
    private static bool isTCDone = false;
    private static int numOfTurns = 0;
    private Player p1;
    private Player p2;
    

    public delegate void EventAddedHandler(object sender, GameEventsArgs data); //the delegate
    public static event EventAddedHandler NotifySubsOfEvent; // an instance of the delegate only ever gonna be one

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
        p1 = new Player(5);
        p2 = new Player();

        if( p1 == null || p2 == null)
        {
            Debug.Log("Player in EventManager is Null as fuck!");
        }

        CardDataBase.MakePlayerDeck(p1);
        CardDataBase.MakePlayerDeck(p2);

        p1.Majesty = p1.PlayerDeck.GetMajesty();
        p2.Majesty = p2.PlayerDeck.GetMajesty();

        p1.PlayerTurn = new Turn(p1);
        p2.PlayerTurn = new Turn(p2);
        Debug.Log("Number of Individual Abilities: " + Ability.numOfAbilities);

    }

    void Start()
    {
        ValidatePlayers(new Player[] { p1, p2 });
        Debug.Log("Player:" + p1.PlayerID + " cards in hand " + p1.GetLocation("Hand").Count);
        Debug.Log("Player:" + p2.PlayerID + " cards in hand " + p2.GetLocation("Hand").Count);
    }

    // Update is called once per frame
    void Update()
    {
        if(p1 == p2)
        {
            Debug.Log("These players are the same object");
        }

        if (Input.GetKeyDown("space"))
        {
            Debug.Log("We're starting the test");

            if(tcBuffer.Count == 0 || isTCDone)
            {
                Debug.Log("Current Majesty HP: " + p1.Majesty.HP);
                PlayGame(p1, p2);

                StartCoroutine(WaitSomeTime(UnityEngine.Random.Range(3, 8)));
                p1.Majesty.HP = p1.Majesty.HP - UnityEngine.Random.Range(3, 600);
                if(!isTCDone)
                {
                    BeginTerritoryChallenge();
                }
            } 
            
        }
    }

    private void BeginTerritoryChallenge()
    {
        if(tcBuffer.Count == 0)
        {
            throw new Exception("Something went wrong with Territory Challenge Landscape selection");
        }
        Debug.Log("Getting shape from " + tcBuffer[0].getName());
        ShapeTrait p1Shape = tcBuffer[0].GetShape();
        Debug.Log("Getting shape from " + tcBuffer[1].getName());
        ShapeTrait p2Shape = tcBuffer[1].GetShape();
        Player tcWinner;

        if((p1Shape == ShapeTrait.Circle && p2Shape == ShapeTrait.Square) || (p1Shape == ShapeTrait.Square && p2Shape == ShapeTrait.Triangle) || (p1Shape == ShapeTrait.Triangle && p2Shape == ShapeTrait.Circle))
        {
            tcWinner = tcBuffer[0].getOwner();
        }
        else if ((p2Shape == ShapeTrait.Circle && p1Shape == ShapeTrait.Square) || (p2Shape == ShapeTrait.Square && p1Shape == ShapeTrait.Triangle) || (p2Shape == ShapeTrait.Triangle && p1Shape == ShapeTrait.Circle))
        {
            tcWinner = tcBuffer[1].getOwner();
        }
        else if(p1Shape == ShapeTrait.None || p2Shape == ShapeTrait.None)
        {
            throw new Exception("Either " + tcBuffer[0].getName() + " is not a Landscape or " + tcBuffer[1].getName() + " is not Landscape. Or Neither are Landscapes");
        }
        else
        {
            Debug.Log("Its a draw...redo!");
            throw new Exception("Fix the display!!!");
            //relaunch landscape selection
        }

        isTCDone = true;
        if(tcWinner == p2)//default p1 did not win TC swap them
        {
            Player temp = p1;
            p1 = tcWinner;
            p2 = temp;
        }

        for(int i=0; i<2; i++)
        {
            if(tcBuffer[i].getOwner() == tcWinner)
            {
                activeLand = tcBuffer[i];
            }
        }
    }

    private void PlayGame(Player player1, Player player2)
    {
        player1.PlayerTurn.StartTurn();
        player2.PlayerTurn.StartTurn();
        numOfTurns++;
        if(numOfTurns % 6 == 0)
        {
            Debug.Log("Twist Dimensions");
            TwistDims();
        }
    }

    private void TwistDims()
    {

    }

    private void ValidatePlayers(Player[] players)
    {
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].PlayerTurn != null && players[i].PlayerDeck != null)
            {
                Debug.Log("Player:"+ players[i].PlayerID +" Turn Validated, Not Null");
                Debug.Log("Player: " + players[i].PlayerID + " Deck Validated, Not Null");
            }
            else
            {
                Debug.Log("NULL OBJECT!! ERROR GONNA HAVE AN ERROR!!!!");
                return;
            }           
        }
    }

    private IEnumerator WaitSomeTime(int t)
    {
        yield return new WaitForSeconds(t);
    }
}
