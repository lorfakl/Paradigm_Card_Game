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
    public Text deckCount;
    public Text graveCount;
    public Text barrierCount;
    public Text nonUIDeckCount;
    public Text nonUIGraveCount;
    public Text nonUIHandCount;
    public Text nonUIBarrierCount;


    private static Stack<GameEventsArgs> eventStack = new Stack<GameEventsArgs>();  //there's only ever gonna be one of these
    private static Queue<GameEventsArgs> eventQueue = new Queue<GameEventsArgs>();
    
    

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

    /// <summary>
    /// GameEventManager will end up attached to an empty gameobject when the game starts to well...manage game events
    /// Thats why it extends Monobehaviour and has Awake, Update, and Start functions
    /// </summary>
    
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

    }

   /// <summary>
   /// THIS IS GARBAGE USE THE OBSERVER PATTERN
   /// </summary>
    private void CheckPlayerInfo()
    {
        /*int[] data = { UIPlayer.PlayerDeck.Count, UIPlayer.GetLocation("Grave").Count, UIPlayer.GetLocation("BZ").Count, UIPlayer.PlayerDeck.Count, UIPlayer.GetLocation("Grave").Count, UIPlayer.GetLocation("Hand").Count, UIPlayer.GetLocation("BZ").Count };
        if(!initalData.SequenceEqual(data))
        {
            OnPlayerInfoChange(data, UIPlayer.GetLocation("Hand").GetContents(), UIPlayer.GetLocation("Field").GetContents(), NonUIPlayer.GetLocation("Hand").GetContents(), NonUIPlayer.GetLocation("Field").GetContents());
            initalData = data;
        }*/
    }
 
}
