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
    private static Queue<Action> actionQ = new Queue<Action>();
    private static Stack<Action> actionStack = new Stack<Action>();
    public static bool isTimeToResolveStack = false;

    private static Queue<GameEventsArgs> eventQueue = new Queue<GameEventsArgs>();

    private static Dictionary<(MoveAction moveAction, NonMoveAction nonMoveAction), Func<GameEventsArgs,GameEventsArgs>> LegalityCheckCommands = new Dictionary<(MoveAction moveAction, NonMoveAction nonMoveAction), Func<GameEventsArgs, GameEventsArgs>>();
    private static bool isLegal = false;
    private static int requestsToEndStack;

    public delegate void EventAddedHandler(object sender, GameEventsArgs data); //the delegate
    public static event EventAddedHandler NotifySubsOfEvent; // an instance of the delegate only ever gonna be one

    private static void OnEventAdd(object sender, GameEventsArgs data)
    {
        NotifySubsOfEvent?.Invoke(sender, data);
    }

    /// <summary>
    /// This function is used to add GameEvents to the queue, more logic is needed to differeniate GameEvents from one another
    /// Which will lead into adding things to the stack and doing timer things 
    /// </summary>
    /// <param name="e"></param>
    private static void PublishEvent(object s, GameEventsArgs e)
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

    private static IEnumerator ResolveStack()
    {//make this into a Coroutine
        
        while (actionStack.Count > 0)
        {
            actionStack.Pop().Resolve();
        }
        
        yield return null;
    }

    public static void AddToStack(object s, GameEventsArgs e)
    {
        PublishEvent(s, e);
    }

    public static void AddToStack(List<Action> actions, GameEventsArgs e)
    {
        foreach (Action a in actions)
        {
            actionQ.Enqueue(a);
        }

        actionStack.Push(actionQ.Dequeue());
        actionStack.Peek().GenerateEvent(e);
        //GenerateEvent tells all other abilities that something happened specifically 
        //this will be am Initiate Event so conditions that pop off from initiate events
        //are able to trigger This is the stack notification 

        //Conditions that trigger this way should notify the UIManager with a stack notification
        //This is why UI Manager needs a Queue
        HelperFunctions.Print("Other abilities should trigger and UI should do stuff");
        HelperFunctions.RaiseNewUIEvent(actionStack.Peek(), actionStack.Peek(), EventType.StackNotification);
        //TransportLayer.ServerMessages.PromptForResponse(); //something like this for networked play

    }

    public static void EndStackSignal(object caller)
    {
        if(caller.GetType() != typeof(UIManager))
        {
            return;
        }
        else
        {
            requestsToEndStack++;
            if(requestsToEndStack >= GlobalGameConfiguration.NumberOfPlayers)
            {
                UIManager uIManager = (UIManager)caller;
                uIManager.StartCoroutine(ResolveStack());
            }
        }
    }

    #region Unity Callbacks
    //MOST CODE BELOW THIS LINE IS PURELY FOR TESTING AND WILL BE REMOVED AND REWORKED
    void Awake()
    {

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isTimeToResolveStack)
        {
            isTimeToResolveStack = false;
            StartCoroutine(ResolveStack());
        }
        
    }
    #endregion

    #region Private Functions for Legality
    private GameEventsArgs CheckAttackPhaseEntryLegality(GameEventsArgs g)
    {
        print("CHECKING LEGALITY");
        GameEventsArgs gevent = new GameEventsArgs();
        List<Card> cardsAbleToAttack = new List<Card>();
        if(g.EventOwner.Type == g.PlayerTarget.Type) //prevents making attacks with a different player's cards
        {
            print("Checking that the same player is declaring this attack");
            print("Cards on the field: " + g.EventOwner.Field.Count);
            foreach (Card c in g.EventOwner.Field) //for each card on the field
            {
                print("Are the cards on the field accessors?");
                print("Is the card an Accessor?: " + c.GetType().ToString());
                print("Is the card a subclass of Accessor?: " + c.GetType().IsSubclassOf(typeof(Accessor)));
                if (c.GetType() == typeof(Accessor) || c.GetType().IsSubclassOf(typeof(Accessor)) ) //check to see if they inherit from Accessor
                {
                    Accessor a = (Accessor)c; //do a cast
                    print("lastly does it have attacks?: " + a.NumOfAttacks);
                    if(a.NumOfAttacks > 0) //if the accessor can do an attack
                    {
                        print("HAS ATTACKS AND IS ACCESSOR");
                        cardsAbleToAttack.Add(c); //add to cardtargets
                    }
                    
                }
            }

            gevent = HelperFunctions.GenerateReturnEvent(g.EventOwner, g.PlayerTarget, cardsAbleToAttack, new GameAction(MoveAction.None, NonMoveAction.Attack), EventType.UIUpdate);
            if (TransportLayer.EventIngestion.IsOnline)
            {
                TransportLayer.EventIngestion.SendReturnEvent(gevent);
            }
            else
            {
                isLegal = true;
                return gevent;
            }
            
        }

        isLegal = false;
        return gevent;
    }
    
    private GameEventsArgs CheckDeclaredAttackLegality(GameEventsArgs g)
    {
        if(g.TargetCard.CurrentLocation == ValidLocations.Field)
        {
            isLegal = true;
            if(g.TargetCard.Owner.Field.Count > 1)
            {
                //Generate a return event telling the UI Manager to give a block prompt to the other player
                GameEventsArgs gameEvent = HelperFunctions.GenerateReturnEvent(g.EventOriginCard, g.ActionEvent, g.TargetCard);
                return gameEvent;
            }
            return g;
        }
        else
        {
            isLegal = false;
            return g;
        }
        
    }
    #endregion
}



