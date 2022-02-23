using System;
using Utilities;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CardSelectionOverlayForNetwork : MonoBehaviour
{
    //This script is for use any time the player needs to select Cards from a list
    //It should get the cards from somewhere, Likely a location object
    //It should display those cards, allow the player to select them and send the selected cards back to the sender
    public GameObject cardPrefab;
    //public Camera uiCamera;

    private GameObject display;
    //private GameObject mainCamera;
    private Transform parent;
    private Transform canvas;
    private Vector3 position = new Vector3();
    private List<GameObject> objectsCreated = new List<GameObject>();
    private int currentlySelectedCards = 0;
    private int maxCardsToSelect = 0;
    private bool hasHitMaxCards = false;

    public delegate void NotifyDoneChoosing(object sender, bool canContinueSelecting);
    public static event NotifyDoneChoosing IsDoneChoosing; //for multiplayer this cant be static

    [SerializeField]
    private MultiCardTransferSO cardTransferFromSource;

    [SerializeField]
    private QueueCardTransferSO cardTransferToDestination;


    //Private Functions
    private void Awake()
    {
        display = this.gameObject;
        parent = display.transform;
        cardTransferFromSource.dataTransferReadyEvent.AddListener(DisplayCards);

    }

    private void Start()
    {
        canvas = gameObject.transform.parent.parent;
        Canvas canvasComp = canvas.gameObject.GetComponent<Canvas>();
        canvasComp.renderMode = RenderMode.WorldSpace;

        canvas.Find("Button").GetComponent<Button>().onClick.AddListener(StopSelecting);

    }

    private void Update()
    {
       
    }

    private void DisplayCards(List<CardSO> cardDataToDisplay)
    {
        //print("Do we ever call DisplayCards?");
        maxCardsToSelect = cardDataToDisplay.Count;
        cardTransferToDestination.SetQueueDataToListener(cardDataToDisplay);
        
        try
        {
            foreach (CardSO c in cardDataToDisplay)
            {
                print("Is Display Card called?");
                CreateCard();
            }
        }
        catch (Exception e)
        {
            print(e.Message);
            print(e.StackTrace);
            print(e.InnerException);
        }

      
    }

    private void CreateCard()
    {
        GameObject cardObject = Instantiate(cardPrefab, parent) as GameObject;
        DisplayPanelCard.HasBeenSelectedEvent += CountCurrentlySelectedCards;

        objectsCreated.Add(cardObject);
        
        position = cardObject.transform.position;
        ScaleCard(cardObject);
    }

    private void ScaleCard(GameObject c)
    {
        RectTransform rectTrans = c.GetComponent<RectTransform>();
        rectTrans.localScale += new Vector3(30, 30, 0);
    }

    private void StopSelecting()
    {
        //IsDoneChoosing.Invoke();
    }

    private void CountCurrentlySelectedCards(object sender, bool hasBeenSelected)
    {
        if(hasBeenSelected)
        {
            currentlySelectedCards++;
            print("Currently Selected Cards: " + currentlySelectedCards);
            print("Max Selected Cards: " + maxCardsToSelect);

            if (currentlySelectedCards == maxCardsToSelect)
            {
                if(!hasHitMaxCards)
                {
                    IsDoneChoosing.Invoke(this, true);
                }
                
            }
        }
        else
        {
            currentlySelectedCards--;
            print("Currently Selected Cards: " + currentlySelectedCards);
            print("Max Selected Cards: " + maxCardsToSelect);
            IsDoneChoosing.Invoke(this, false);
        }
    }
}
