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
    private static Landscape activeLand;
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
        p1 = new Player();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("We're starting the test");
            
            Debug.Log("Current Majesty HP: " + p1.Majesty.HP);
            PlayGame(p1, p2);
            StartCoroutine(WaitSomeTime(UnityEngine.Random.Range(3, 8)));
            p1.Majesty.HP = p1.Majesty.HP - UnityEngine.Random.Range(3, 600);
            
        }
    }

    private void PlayGame(Player player1, Player player2)
    {
        player1.PlayerTurn.StartTurn();
        player2.PlayerTurn.StartTurn();
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
