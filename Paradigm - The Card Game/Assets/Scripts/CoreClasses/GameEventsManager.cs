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
    private static Stack<GameEventsArgs> eventStack = new Stack<GameEventsArgs>();  //there's only ever gonna be one of these
    private static Queue<GameEventsArgs> eventQueue = new Queue<GameEventsArgs>();

    private static Dictionary<(MoveAction moveAction, NonMoveAction nonMoveAction), Func<GameEventsArgs,GameEventsArgs>> LegalityCheckCommands = new Dictionary<(MoveAction moveAction, NonMoveAction nonMoveAction), Func<GameEventsArgs, GameEventsArgs>>();
    private static bool isLegal = false;

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

    public static void CheckLegality(object s, GameEventsArgs e)
    {
        try
        {
            e = LegalityCheckCommands[(e.MoveActionEvent, e.ActionEvent)](e);
        }
        catch(Exception ex)
        {
            HelperFunctions.CatchException(ex);
            HelperFunctions.Print("Inside the catch block of check legality");
            isLegal = true;
        }

        if(isLegal)
        {
            PublishEvent(s, e);
            isLegal = false;
        }
    }



    
    #region Unity Callbacks
    //MOST CODE BELOW THIS LINE IS PURELY FOR TESTING AND WILL BE REMOVED AND REWORKED
    void Awake()
    {
        LegalityCheckCommands.Add((MoveAction.None, NonMoveAction.Attack), CheckAttackPhaseEntryLegality);
        LegalityCheckCommands.Add((MoveAction.None, NonMoveAction.DeclaredAttack), CheckDeclaredAttackLegality);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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


