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

public class UIManager : MonoBehaviour
{
    public GameObject handspace;
    public GameObject[] cardSlots;
    public GameObject cardPrefab;
    public UIScriptableObject drawUIEffects;
    public float CardDrawMoveSpeed = 1;
    public Vector3 CardScale;


    public Vector2 postionOffset;

    //private Dictionary<MoveAction, Action<string>> MoveActionFunctionDict = new Dictionary<MoveAction, Action<string>>();
    private int nextCardSlot;
    private void Awake()
    {
        /*MoveActionFunctionDict.Add(MoveAction.Break, "Doing Break UI Action");
        MoveActionFunctionDict.Add(MoveAction.Build, "Doing Build UI Action");
        MoveActionFunctionDict.Add(MoveAction.Collect, "Doing Collect UI Action");
        MoveActionFunctionDict.Add(MoveAction.Crystallize, "Doing Crystallize UI Action");
        MoveActionFunctionDict.Add(MoveAction.Delete, "Doing Delete UI Action");
        MoveActionFunctionDict.Add(MoveAction.Despawn, "Doing Despawn UI Action");
        MoveActionFunctionDict.Add(MoveAction.Draw, "Doing Draw UI Action");
        MoveActionFunctionDict.Add(MoveAction.Lock, "Doing Lock UI Action");
        MoveActionFunctionDict.Add(MoveAction.Rest, "Doing Rest UI Action");
        MoveActionFunctionDict.Add(MoveAction.Return, "Doing Return UI Action");
        MoveActionFunctionDict.Add(MoveAction.Search, "Doing Search UI Action");
        MoveActionFunctionDict.Add(MoveAction.Spawn, "Doing Spawn UI Action");
        MoveActionFunctionDict.Add(MoveAction.Unlock, "Doing Unlock UI Action");*/
        

    }
    void Start()
    {
        GameEventsManager.NotifySubsOfEvent += CheckForUIEvent;
        nextCardSlot = cardSlots.Length - 1;
        postionOffset = new Vector2(0, 0);

        if(drawUIEffects == null)
        {
            HelperFunctions.Error("DrawUIEffects is null");
        }

        if (handspace == null)
        {
            HelperFunctions.Error("Handscapce is null");
        }

        if (cardSlots == null)
        {
            HelperFunctions.Error("Card Slots is null");
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

    private void CheckForUIEvent(object sender, GameEventsArgs e)
    {
        if(e.IsUIEvent)
        {
            print("UI manager Caught a UI event");
            print("UI Event Data: ");
            e.Print();
            UiEvents ue = (UiEvents)e;

            ShowCardMovement(ue);
           
        }
        
    }

    private void ShowCardMovement(UiEvents e)
    {
        //if(nextCardSlot  0)
        HelperFunctions.Print("Is this getting called?");
        Player owner = e.EventOwner;
        Transform startPoint;
        if (owner.Type == "AI")
        {
            Debug.LogWarning("Please fix the Card bug, this if statement is just while that bug exists");
            Debug.LogWarning("Grabbing UI Player start point instead of NonUI");
            startPoint = drawUIEffects.UiEntryPoint;   
        }
        //cardSlots[nextCardSlot];
        HelperFunctions.Print("Current CardSlot index: " + nextCardSlot);
        GameObject cardObject = HelperFunctions.CreateCard(e.EventOriginCard, false, cardSlots[nextCardSlot].transform, cardPrefab);
        

        if (CardScale == null)
        {
            HelperFunctions.ScaleCard(cardObject, new Vector3(30, 30, 0));
        }
        else
        {
            HelperFunctions.ScaleCard(cardObject, CardScale);
        }
        nextCardSlot--;
        //cardSlots[nextCardSlot].transform.position.x 
        Vector3 newpostion = cardSlots[nextCardSlot].transform.position;
        newpostion.x -= 1.746666f;
        cardObject.transform.DOMove(newpostion, 1.5f);
    }

    private void PositionHand(GameObject c)
    {
        
        

        
        
        
    }
}
