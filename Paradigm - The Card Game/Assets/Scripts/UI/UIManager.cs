using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using DG.Tweening;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

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
    [Header("Card Space Game Object References")]
    public GameObject handspace;
    public GameObject fieldspace;
    public GameObject otherHandspace;
    public GameObject otherFieldspace;
    public GameObject attackPrefab;
    public GameObject blockPrefab;

    public GameObject cardPrefab;
    [Header("Turnphase Notice Game Object References")]
    public GameObject turnphaseTextPrefab;
    public GameObject onScreenTurnphase;
    [Header("Info Groups Object References")]
    public GameObject playerInfoGroup;
    public GameObject otherInfoGroup;

    [Header("Scriptable Object References")]
    public UIScriptableObject drawUIEffects;
    public UIScriptableObject spawnUIEffects;
    public UIScriptableObject turnphaseUIEffects;
    public UIScriptableObject attackUIEffects;

    #endregion

    #region Private Fields
    [SerializeField]
    private static ObservableCollection<GameObject> fieldCards = new ObservableCollection<GameObject>();

    [SerializeField]
    private static ObservableCollection<GameObject> handCards = new ObservableCollection<GameObject>();

    [SerializeField]
    private static ObservableCollection<GameObject> otherFieldCards = new ObservableCollection<GameObject>();

    [SerializeField]
    private static ObservableCollection<GameObject> otherHandCards = new ObservableCollection<GameObject>();

    [SerializeField]
    private static float tweenSpeed = .1f;

    private Dictionary<(MoveAction moveAction, NonMoveAction nonMoveAction), ICommand> UICommandsDict = new Dictionary<(MoveAction moveAction, NonMoveAction nonMoveAction), ICommand>();
    
    
    #endregion

    #region Properties
    public static ObservableCollection<GameObject> HandCards
    {
        get { return handCards; }
    }

    public static ObservableCollection<GameObject> FieldCards
    {
        get { return fieldCards; }
    }

    public static ObservableCollection<GameObject> OtherHandCards
    {
        get { return otherHandCards; }
    }

    public static ObservableCollection<GameObject> OtherFieldCards
    {
        get { return otherFieldCards; }
    }

    public static float CardWidth
    {
        get { return 2.88f; }
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
        UICommandsDict.Add((MoveAction.Spawn, NonMoveAction.None), new SpawnCommand(spawnUIEffects));
        //UICommandsDict.Add(MoveAction.Unlock, "Doing Unlock UI Action");
        UICommandsDict.Add((MoveAction.Draw, NonMoveAction.None), new DrawCommand(drawUIEffects));
        UICommandsDict.Add((MoveAction.None, NonMoveAction.Turn), new TurnPhaseCommand(turnphaseUIEffects, turnphaseTextPrefab, onScreenTurnphase));
        UICommandsDict.Add((MoveAction.None, NonMoveAction.Attack), new AttackCommand(attackUIEffects, attackPrefab));
        fieldCards.CollectionChanged += HandleFieldItemChange;
        handCards.CollectionChanged += HandleHandItemChange;
        otherFieldCards.CollectionChanged += HandleFieldItemChange;
        otherHandCards.CollectionChanged += HandleHandItemChange;
        

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
        //print("Cards on field: " + fieldCards.Count);
    }
    #endregion


    private void HandleFieldItemChange(object sender, NotifyCollectionChangedEventArgs e)
    {
        ObservableCollection<GameObject> originCollection;
        originCollection = (ObservableCollection<GameObject>)sender;
        //print("was this called?");
        float xMove = -3.07f * (originCollection.Count - 1);

        
        switch(e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if(originCollection.Count == 1)
                {
                    originCollection[originCollection.Count - 1].transform.DOMoveX(-CardWidth / 2, .1f);
                }
                originCollection[originCollection.Count - 1].transform.DOMoveX(xMove-CardWidth/2, .1f);
                break;

            case NotifyCollectionChangedAction.Remove:
                break;
        }
    }

    private void HandleHandItemChange(object sender, NotifyCollectionChangedEventArgs e)
    {
        ObservableCollection<GameObject> originCollection;
        originCollection = (ObservableCollection<GameObject>)sender;

        /*try
        {
            print(originCollection[0].GetComponent<CardScript>().Card.Owner.Type);

        }
        catch(Exception ex)
        {
            HelperFunctions.CatchException(ex);
        }*/

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Remove:
                int removedIndex = e.OldStartingIndex;
                for(int i = 0; i < originCollection.Count; i++)
                {
                    Vector3 position;
                    if(i < removedIndex)
                    {
                        position = originCollection[i].transform.localPosition;
                        originCollection[i].transform.DOMoveX(position.x - CardWidth / 2, tweenSpeed);
                    }
                    else
                    {
                        position = originCollection[i].transform.localPosition;
                        originCollection[i].transform.DOMoveX(position.x + CardWidth / 2, tweenSpeed);
                    }
                }
                break;

            case NotifyCollectionChangedAction.Add:
                break;
        }
    }

    private void CheckForUIEvent(object sender, GameEventsArgs e)
    {
        if(e.IsUIEvent)
        {
            //print("UI manager Caught a UI event");
            //print("UI Event Data: " + "\n" + "Player: " + e.EventOwner.Type.ToString() + " Target" + e.PlayerTarget.Type.ToString() + "\n" + 
            //    "MoveAction: " + e.MoveActionEvent + " NonMoveAction: " + e.ActionEvent);
     
            if (e.ActionEvent == NonMoveAction.Turn)
            {
                //print(e.EventOwner.Type + " is starting their " + e.TurnPhase + " phase");
            }
            
            try
            {

                StartCoroutine(UICommandsDict[(e.MoveActionEvent, e.ActionEvent)].Execute(e));
            }
            catch(Exception ex)
            {
                HelperFunctions.CatchException(ex);
            }
        }

        UpdateInfoGroup(e);
        
    }

    private void UpdateInfoGroup(GameEventsArgs e)
    {
        Text[] textInInfoGroup;
        if(e.EventOwner.Type == PlayerType.AI)
        {
            textInInfoGroup = otherInfoGroup.GetComponentsInChildren<Text>();
        }
        else
        {
            textInInfoGroup = playerInfoGroup.GetComponentsInChildren<Text>();
        }

        for (int i = 0; i < textInInfoGroup.Length; i++)
        {
            string[] splitString = textInInfoGroup[i].text.Split(':');
            textInInfoGroup[i].text = splitString[0] + ": ";

            switch (i)
            {
                case 0:
                    textInInfoGroup[i].text += e.EventOwner.PlayerDeck.Count;
                    break;
                case 1:
                    textInInfoGroup[i].text += e.EventOwner.GetLocation(ValidLocations.DZ).Count;
                    break;
                case 2:
                    textInInfoGroup[i].text += e.EventOwner.GetLocation(ValidLocations.SC).Count;
                    break;
                case 3:
                    textInInfoGroup[i].text += e.EventOwner.GetLocation(ValidLocations.Grave).Count;
                    break;
                case 4:
                    textInInfoGroup[i].text += e.EventOwner.BZ.Count;
                    break;
            }
        }
    }

}




