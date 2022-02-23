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
    private bool isLocalClientDebug = false;
    private bool isQueueLoaded = false;

    public List<ClientCardInfo> cardsInDeck;

    public GameObject displayPrefab;

    public static Queue<CardSO> cardsToDisplay = new Queue<CardSO>();

    private BarrierSelectManager BarrierSelectManager
    {
        get;
        set;
    }
    private void Awake()
    {
        BarrierSelectManager = this.gameObject.GetComponent<BarrierSelectManager>();
        BarrierSelectManager.RequestDeckContentsCmd(PlayFabSettings.staticPlayer.PlayFabId);

        displayPrefab = BarrierSelectManager.displayPanel;
        
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
        if(!isQueueLoaded)
        {
            if(cardsInDeck.Count > 0)
            {
                LoadQueue();
                isQueueLoaded = true;
            }
        }
        
    }

    private void LoadQueue()
    {
        foreach(ClientCardInfo c in cardsInDeck)
        {
            CardSO cSO = (CardSO)ScriptableObject.CreateInstance(typeof(CardSO));
            cSO.Init(c);
            cardsToDisplay.Enqueue(cSO);
        }
    }

    private void OnClientRecievedBarrierTimeReset(ServerResetBarrierTimer arg2)
    {
        throw new NotImplementedException();
    }

}
