using System;
using Utilities;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public class CardSelectionOverlayForNetwork : MonoBehaviour
{
    
 
    private Transform parent;
    private Transform canvas;
    private Camera mainCamera;
    private Vector3 position = new Vector3();
    private readonly List<GameObject> objectsCreated = new List<GameObject>();
    private readonly Dictionary<string, CardSO> cardsSelected = new Dictionary<string, CardSO>();
    private int currentlySelectedCards = 0;
    private int maxCardsToSelect = 0;
    private bool hasHitMaxCards = false;

    public delegate void NotifyDoneChoosing(object sender, bool canContinueSelecting);
    public static event NotifyDoneChoosing IsDoneChoosing; //for multiplayer this cant be static

    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private GameObject display;

    [SerializeField]
    private MultiCardTransferSO cardTransferFromSource;

    [SerializeField]
    private MultiCardTransferSO cardTransferToSource;

    [SerializeField]
    private QueueCardTransferSO cardTransferToDestination;

    public int MaxCardsToSelect
    {
        get { return maxCardsToSelect; }
        set { maxCardsToSelect = value; }
    }

    //Private Functions
    private void Awake()
    {
        //display = this.gameObject;
        parent = display.transform;
        cardTransferFromSource.dataTransferReadyEvent.AddListener(DisplayCards);
        mainCamera = Camera.main;
    }

    private void Start()
    {
        canvas = gameObject.transform;
        Canvas canvasComp = canvas.gameObject.GetComponent<Canvas>();
        /*canvasComp.renderMode = RenderMode.WorldSpace;
        canvasComp.worldCamera = mainCamera;
        gameObject.transform.position = mainCamera.transform.position;*/

        canvas.Find("Button").GetComponent<Button>().onClick.AddListener(StopSelecting);

    }

    private void Update()
    {
       
    }

    private void DisplayCards(List<CardSO> cardDataToDisplay)
    {
        //print("Do we ever call DisplayCards?");
        
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
        cardObject.GetComponent<DisplayPanelCard>().HasBeenSelectedEvent += CountCurrentlySelectedCards;

        objectsCreated.Add(cardObject);
        
        position = cardObject.transform.position;
        ScaleCard(cardObject);
    }

    private void ScaleCard(GameObject c)
    {
        RectTransform rectTrans = c.GetComponent<RectTransform>();
        //rectTrans.localScale += new Vector3(30, 30, 0);
    }

    private void StopSelecting()
    {
        //IsDoneChoosing.Invoke();
        print("Button Pressed");
    }

    private void CountCurrentlySelectedCards(object sender, CardSO c)
    {
        if(cardsSelected.ContainsKey(c.InstanceId))
        {
            currentlySelectedCards--;
            print("Currently Selected Cards: " + currentlySelectedCards);
            print("Max Selected Cards: " + maxCardsToSelect);
            cardsSelected.Remove(c.InstanceId);
            IsDoneChoosing.Invoke(this, false);
        }
        else
        {
            currentlySelectedCards++;
            print("Currently Selected Cards: " + currentlySelectedCards);
            print("Max Selected Cards: " + maxCardsToSelect);
            cardsSelected.Add(c.InstanceId, c);
            if (currentlySelectedCards == maxCardsToSelect)
            {
                if (!hasHitMaxCards)
                {
                    IsDoneChoosing.Invoke(this, true);
                }
            }
        }

        cardTransferToSource.SendListDataToListener(cardsSelected.Values.ToList());
    }
}
