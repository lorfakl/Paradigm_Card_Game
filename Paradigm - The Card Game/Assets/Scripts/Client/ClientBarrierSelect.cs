using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Mirror;
using CustomNetworkMessages;
using System;
using PlayFab;
using System.Linq;
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
        BarrierSelectManager = this.gameObject.transform.parent.gameObject.GetComponent<BarrierSelectManager>();
        //NetworkClient.Ready();
        BarrierSelectManager.Instance.RequestDeckContentsCmd(PlayFabSettings.staticPlayer.PlayFabId);
        existingCanvas = GameObject.FindGameObjectWithTag("canvas").GetComponent<Canvas>();
        receiveCardsFromDisplay.dataTransferReadyEvent.AddListener(ReceiveCurrentlySelectedCards);
    }
   

    void Start()
    {
        HelperFunctions.Log("Start method of the Client Side Barrier Select");
        //cardsInDeck = BarrierSelectManager.clientDeckContent;


        NetworkClient.RegisterHandler<ServerResetBarrierTimer>(OnClientRecievedBarrierTimeReset);
        HelperFunctions.Log("Have the Cards been sent from the Server: " + cardsInDeck?.Count);
    }

    void Update()
    {
        if (BarrierSelectManager.clientDeckContent.Count > 0)
        {
            if (!isListLoaded)
            {
                if (cardsInDeck == null)
                {
                    cardsInDeck = BarrierSelectManager.clientDeckContent;

                    if (cardsInDeck == null)
                    {
                        print("Weird its still null");
                    }
                    else
                    {
                        print("Going through next run");
                    }
                    
                }
                else
                {
                    if (cardsInDeck.Count > 0)
                    {
                        LoadCardList();
                        isListLoaded = true;
                        NetworkClient.Send<ClientRecievedCardsFromServer>(new ClientRecievedCardsFromServer
                        {
                            cardsRecieved = cardsInDeck.Count,
                            pfID = PlayfabHelper.PlayFabID
                        });

                        print("Client has received cards, letting server know");
                    }
                }

            }
        }
        
    }

    private void LoadCardList()
    {
        print("Inside LoadCardList");
        foreach (ClientCardInfo c in cardsInDeck)
        {
            CardSO cSO = (CardSO)ScriptableObject.CreateInstance(typeof(CardSO));
            cSO.Init(c);
            cardsToDisplay.Add(cSO);
        }

        LoadInCSOPrefab();
    }

    private void LoadInCSOPrefab()
    {
        print("Inside LoadInCSOPrefab");
        GameObject csoObject = Instantiate(csoPrefab, this.gameObject.transform) as GameObject;
        //Destroy(csoObject.GetComponent<Canvas>());
        CardSelectionOverlayForNetwork csoScript = csoObject.GetComponent<CardSelectionOverlayForNetwork>();
        csoScript.MaxCardsToSelect = BarrierSelectManager.barriersToSelect;

        transferCardsToDisplay.dataTransferReadyEvent.Invoke(cardsToDisplay);
        GameObject sceneTextObj = GameObject.FindGameObjectWithTag("Finish");
        sceneTextObj.GetComponent<Text>().text = "The CSO has been created you just cant see it";

    }

    private void ReceiveCurrentlySelectedCards(List<CardSO> cardSOs)
    {
        print("Updating selected cards, locally: " + cardSOs.Count);
        barriersCurrentlySelected.Clear();
        

        foreach(CardSO s in cardSOs)
        {
            if(!barriersCurrentlySelected.Contains(s.InstanceId))
            {
                barriersCurrentlySelected.Add(s.InstanceId);
                print(PlayfabHelper.PlayFabID + " Adding " + s.InstanceId + " on the server");
                BarrierSelectManager.Instance.UpdateSelectedBarriersCmd(PlayfabHelper.PlayFabID, s.InstanceId);
            }

        }
        

        foreach(string id in barriersCurrentlySelected)
        {
            var result = cardSOs.FindAll(c => c.InstanceId == id);
            if(result.Count == 0)
            {
                barriersCurrentlySelected.Add(id);
                print(PlayfabHelper.PlayFabID + " Removing " + id + " on the server");
                BarrierSelectManager.Instance.UpdateSelectedBarriersCmd(PlayfabHelper.PlayFabID, id);
            }
        }

        if(barriersCurrentlySelected.Count == BarrierSelectManager.Instance.barriersToSelect)
        {
            print("Selected all required barriers " + barriersCurrentlySelected.Count + " out of " + BarrierSelectManager.Instance.barriersToSelect);
            NetworkClient.Send(new ClientCompletedBarrierSelection
            {
                barriersSelected = barriersCurrentlySelected.Count,
                playFadId = PlayfabHelper.PlayFabID
            });
        }

        
    }

    private void OnClientRecievedBarrierTimeReset(ServerResetBarrierTimer arg2)
    {
        print("Recieved Timer Reset. We've run out of time. Sending what we got");
        BarrierSelectManager.Instance.ReceiveClientBarrierSelections(PlayfabHelper.PlayFabID, barriersCurrentlySelected.ToArray());
        NetworkClient.Send<ClientBarrierSelectionTimeout>(new ClientBarrierSelectionTimeout
        {
            playFadId = PlayfabHelper.PlayFabID,
            instanceIds = barriersCurrentlySelected.ToArray()
        });
    }

}
