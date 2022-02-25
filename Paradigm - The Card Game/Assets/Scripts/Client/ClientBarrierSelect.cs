using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Mirror;
using CustomNetworkMessages;
using System;
using PlayFab;
using UnityEngine.Events;

public class ClientBarrierSelect : NetworkBehaviour
{
    [SerializeField]
    private MultiCardTransferSO transferCardsToDisplay;

    [SerializeField]
    private MultiCardTransferSO receiveCardsFromDisplay;

    [SerializeField]
    private GameObject csoPrefab;

    private bool isListLoaded = false;
    private List<CardSO> cardsToDisplay = new List<CardSO>();
    private List<string> barriersCurrentlySelected = new List<string>();

    private Canvas existingCanvas; 
    public List<ClientCardInfo> cardsInDeck;
    

    private BarrierSelectManager BarrierSelectManager
    {
        get;
        set;
    }

    private void Awake()
    {
        BarrierSelectManager = this.gameObject.GetComponent<BarrierSelectManager>();
        BarrierSelectManager.RequestDeckContentsCmd(PlayFabSettings.staticPlayer.PlayFabId);
        existingCanvas = GameObject.FindGameObjectWithTag("canvas").GetComponent<Canvas>();
        receiveCardsFromDisplay.dataTransferReadyEvent.AddListener(ReceiveCurrentlySelectedCards);
    }
   

    void Start()
    {
        HelperFunctions.Log("Start method of the Client Side Barrier Select");
        cardsInDeck = BarrierSelectManager.clientDeckContent;


        NetworkClient.RegisterHandler<ServerResetBarrierTimer>(OnClientRecievedBarrierTimeReset);
        HelperFunctions.Log("Have the Cards been sent from the Server: " + cardsInDeck?.Count);
    }

    void Update()
    {
        if(!isListLoaded)
        {
            if (cardsInDeck.Count > 0)
            {
                LoadCardList();
                isListLoaded = true;
            }
        } 
    }

    private void LoadCardList()
    {
        foreach(ClientCardInfo c in cardsInDeck)
        {
            CardSO cSO = (CardSO)ScriptableObject.CreateInstance(typeof(CardSO));
            cSO.Init(c);
            cardsToDisplay.Add(cSO);
        }

        LoadInCSOPrefab();
    }

    private void LoadInCSOPrefab()
    {
        Destroy(csoPrefab.GetComponent<Canvas>());
        GameObject csoObject = Instantiate(csoPrefab, existingCanvas.transform) as GameObject;
        CardSelectionOverlayForNetwork csoScript = csoObject.GetComponent<CardSelectionOverlayForNetwork>();
        csoScript.MaxCardsToSelect = BarrierSelectManager.barriersToSelect;

        transferCardsToDisplay.dataTransferReadyEvent.Invoke(cardsToDisplay);
    }

    private void ReceiveCurrentlySelectedCards(List<CardSO> cardSOs)
    {
        foreach(CardSO c in cardSOs)
        {
            if(barriersCurrentlySelected.Contains(c.InstanceId))
            {
                barriersCurrentlySelected.Remove(c.InstanceId);
            }
            else
            {
                barriersCurrentlySelected.Add(c.InstanceId);
            }
        }
    }

    private void OnClientRecievedBarrierTimeReset(ServerResetBarrierTimer arg2)
    {

        NetworkClient.Send<ClientBarrierSelectionTimeout>(new ClientBarrierSelectionTimeout
        {
            playFadId = PlayfabHelper.PlayFabID,
            instanceIds = barriersCurrentlySelected.ToArray()
        });
    }

}
