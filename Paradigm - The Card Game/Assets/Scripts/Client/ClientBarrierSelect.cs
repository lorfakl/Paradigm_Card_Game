using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Mirror;
using CustomNetworkMessages;
using System;
using PlayFab;

public class ClientBarrierSelect : NetworkBehaviour
{
    [SerializeField]
    private MultiCardTransferSO transferCardsToDisplay;
    [SerializeField]
    private GameObject csoPrefab;

    private bool isListLoaded = false;
    private List<CardSO> cardsToDisplay = new List<CardSO>();

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
        GameObject CSOobject = Instantiate(csoPrefab, this.gameObject.transform) as GameObject;
        transferCardsToDisplay.dataTransferReadyEvent.Invoke(cardsToDisplay);
    }

    private void OnClientRecievedBarrierTimeReset(ServerResetBarrierTimer arg2)
    {
        //Send the Server the selections we currently have 
        //NetworkClient.Send<>
    }

}
