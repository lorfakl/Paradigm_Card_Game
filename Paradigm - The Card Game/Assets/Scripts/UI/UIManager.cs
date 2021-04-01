using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using DG.Tweening;

public enum UITarget
{
    Deck,
    Hand,
    Field,
    Grave
}

public enum ListToUpdate
{
    Field,
    Hand
}

[RequireComponent(typeof(UIScriptableObject))]

public class UIManager : MonoBehaviour
{
    #region Public Inspector Fields
    public GameObject handspace;
    public GameObject fieldspace;
    public GameObject otherHandspace;
    public GameObject otherFieldspace;
    public GameObject cardPrefab;

    public UIScriptableObject drawUIEffects;
    public UIScriptableObject spawnUIEffects;

    public GameObject playerInfoGroup;
    public GameObject otherInfoGroup;
    #endregion

    #region Private Fields
    [SerializeField]
    private static List<GameObject> fieldCards = new List<GameObject>();

    [SerializeField]
    private static List<GameObject> handCards = new List<GameObject>();

    [SerializeField]
    private static List<GameObject> otherFieldCards = new List<GameObject>();

    [SerializeField]
    private static List<GameObject> otherHandCards = new List<GameObject>();

    private Dictionary<MoveAction, ICommand> UICommandsDict = new Dictionary<MoveAction, ICommand>();
    #endregion

    #region Properties
    public static List<GameObject> HandCards
    {
        get { return handCards; }
    }

    public static List<GameObject> FieldCards
    {
        get { return fieldCards; }
    }

    public static List<GameObject> OtherHandCards
    {
        get { return otherHandCards; }
    }

    public static List<GameObject> OtherFieldCards
    {
        get { return otherFieldCards; }
    }

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        /*
         NON MOVE ACTIONS THAT HAVE UI INVOLVED(all of them)
            Attack - A turn player designates 1 of their accessors and one of their opponent's accessors or barriers for battle.
            Battle - An accessor inflicts damage to another accessor after attacking, or breaks the barrier.. This includes calculation.
            Block - After a turn player designates attack targets, the defending player changes the accessor on their field that will battle.
            Damage - Reducing an accessor's HP.    
            Forge - The creation of a bond between an Accessor and Bond Mechanism.
            Heal - Increasing an accessor's HP.
            Initiate - When the ability of a card is used.
            React - Spawning or initiating immediately after an opponent spawns or initiates.
            Resolve - When cards spawn or initiate while on the stack.Block - After a turn player designates attack targets, the defending player changes the accessor on their field that will battle.    
            Twist Dimensions - When a landscape switches to a different one by phase or ability
            
         */
        //UICommandsDict.Add(MoveAction.Break, "Doing Break UI Action");
        //UICommandsDict.Add(MoveAction.Build, "Doing Build UI Action");
        //UICommandsDict.Add(MoveAction.Collect, "Doing Collect UI Action");
        //UICommandsDict.Add(MoveAction.Crystallize, "Doing Crystallize UI Action");
        //UICommandsDict.Add(MoveAction.Delete, "Doing Delete UI Action");
        //UICommandsDict.Add(MoveAction.Despawn, "Doing Despawn UI Action");
        //UICommandsDict.Add(MoveAction.Lock, "Doing Lock UI Action");
        //UICommandsDict.Add(MoveAction.Rest, "Doing Rest UI Action");
        //UICommandsDict.Add(MoveAction.Return, "Doing Return UI Action");
        //UICommandsDict.Add(MoveAction.Search, "Doing Search UI Action");
        UICommandsDict.Add(MoveAction.Spawn, new SpawnCommand(spawnUIEffects));
        //UICommandsDict.Add(MoveAction.Unlock, "Doing Unlock UI Action");
        UICommandsDict.Add(MoveAction.Draw, new DrawCommand(drawUIEffects));


    }
    void Start()
    {
        GameEventsManager.NotifySubsOfEvent += CheckForUIEvent;

        if(drawUIEffects == null)
        {
            HelperFunctions.Error("DrawUIEffects is null");
        }

        if (handspace == null)
        {
            HelperFunctions.Error("Handscapce is null");
        }


        if (cardPrefab == null)
        {
            HelperFunctions.Error("Card Prefab is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    private void CheckForUIEvent(object sender, GameEventsArgs e)
    {
        if(e.IsUIEvent)
        {
            print("UI manager Caught a UI event");
            print("UI Event Data: ");
            e.Print();
            UiEvents ue = (UiEvents)e;
            /*if(ue.MoveActionEvent == MoveAction.Draw)
            {
                ShowCardMovement(ue);
            }/**/
            try
            {
                StartCoroutine(UICommandsDict[ue.MoveActionEvent].Execute(ue));
            }
            catch(Exception ex)
            {
                print(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException + "\n" + ex.Source);
            }
        }
        
    }

}




