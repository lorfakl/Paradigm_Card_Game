using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Collections;
using Utilities;
using CustomNetworkMessages;
using PlayFab.ServerModels;
using System;
using System.Linq;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class ServerSideBarrierSelect : NetworkBehaviour
{
    private int numberOfClientsCompletedBarrierSelection = 0;
    private int clientReceivedCardsCount = 0;
    private List<string> clientPlayFabIDs = new List<string>();

    private Dictionary<string, List<string>> clientBarrierSelections = new Dictionary<string, List<string>>();

    private void Awake()
    {
        NetworkServer.RegisterHandler<ClientCompletedBarrierSelection>(OnServerRecievedBarrierSelectionCompleted);
        NetworkServer.RegisterHandler<ClientBarrierSelectionTimeout>(OnServerReceivedClientBarrierSelectionTimeout);
        NetworkServer.RegisterHandler<ClientRecievedCardsFromServer>(OnServerReceivedClientReceivedCards);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnServerRecievedBarrierSelectionCompleted(NetworkConnection conn, ClientCompletedBarrierSelection msg)
    {
        numberOfClientsCompletedBarrierSelection++;
        string pfId = ParadigmServer.Instance.GetParadigmServerConnection(conn).PlayFabId;
        clientPlayFabIDs.Add(pfId);
        HelperFunctions.Log(pfId + " completed barrier selection");
        WriteServerPlayerEventRequest writeEvent = new WriteServerPlayerEventRequest
        {
            EventName = PlayfabHelper.CustomEventNames.pd_player_completed_bs.ToString(),
            PlayFabId = pfId,
            Timestamp = DateTime.UtcNow,
            Body = new Dictionary<string, object>
            {
                {"Barriers Selected: ",  msg.barriersSelected}
            }
        };
        PlayfabHelper.WritePlayerSpecificEvent(writeEvent);

        if (numberOfClientsCompletedBarrierSelection == 2)
        {
            HelperFunctions.Log("All Clients have completed barrier selection");
            WriteTitleEventRequest writeTitleEvent = new WriteTitleEventRequest
            {
                EventName = PlayfabHelper.CustomEventNames.pd_all_players_completed_bs.ToString(),
                Timestamp = DateTime.UtcNow,
                Body = new Dictionary<string, object>
                {
                    {"PlayFabIDs",  clientPlayFabIDs}
                }
            };
            PlayfabHelper.WriteTitleEvent(writeTitleEvent);

            foreach(string key in clientBarrierSelections.Keys)
            {
                FinalizeBarrierSelection(key);
            }

            HelperFunctions.Log("Changing Scenes");
            ParadigmServer.Instance.ServerChangeScene("Territory Challenge Scene");



        }
    }

    private void OnServerReceivedClientBarrierSelectionTimeout(NetworkConnection conn, ClientBarrierSelectionTimeout msg)
    {

        SelectBarriersForClient(msg.playFadId, msg.instanceIds);
        FinalizeBarrierSelection(msg.playFadId);
    }

    private void OnServerReceivedClientReceivedCards(NetworkConnection conn, ClientRecievedCardsFromServer msg)
    {
        HelperFunctions.Log("Received confirmation that ClientID: " + msg.pfID + " has receieved their cards");

        clientReceivedCardsCount++;

        HelperFunctions.Log("Should the timer be start? \n" +
            "Does this: " + ParadigmServer.Instance.Connections.Count + " equal this: " +
            clientReceivedCardsCount);

        
        if(clientReceivedCardsCount == ParadigmServer.Instance.Connections.Count)
        {
            StartCoroutine(BarrierSelectTimer());
        }
    }

    IEnumerator BarrierSelectTimer()
    {
        HelperFunctions.Log("Time left to select barriers " + BarrierSelectManager.Instance.timeOnTimer + " seconds");
        while (BarrierSelectManager.Instance.timeOnTimer > 0)
        {
            yield return new WaitForSeconds(1);
            //Debug.Log("WHATS GOING ON");
            //Debug.Log("Time left on timer: " + this.TimeLeftOnTimer);
            BarrierSelectManager.Instance.timeOnTimer--;

        }
        BarrierSelectManager.Instance.timeOnTimer = BarrierSelectManager.Instance.DefaultTimerTime;
        HelperFunctions.Log("Time is up, resetting Timer");

        foreach (int connID in NetworkServer.connections.Keys.ToList())
        {
            NetworkServer.connections[connID].Send(new ServerResetBarrierTimer
            {
                dateTimeUTC = DateTime.UtcNow.ToString(),
                timeOnTimer = BarrierSelectManager.Instance.timeOnTimer
            });
        }
        
    }

    private void SelectBarriersForClient(string pfID, string[] currentlySelected)
    {
        int maxBarriers = BarrierSelectManager.Instance.barriersToSelect;
        if (currentlySelected.Length != maxBarriers)
        {
            HelperFunctions.Log(pfID + "has selected " + currentlySelected.Length + " out of " + maxBarriers + " barriers");

            ParadigmServerConnection psc = ParadigmServer.Instance.Connections.Find(pCon => pCon.PlayFabId == pfID);
            HelperFunctions.LogListContent("Selected Cards: ", clientBarrierSelections[pfID]);
            for (int i = clientBarrierSelections[pfID].Count; i < BarrierSelectManager.Instance.barriersToSelect; i++)
            {
                var notSelectedCards = psc.PlayerInfo.Locations[ValidLocations.Deck].Where(c => !clientBarrierSelections[pfID].Contains(c.InstanceID)).ToList();
                HelperFunctions.LogListContent("Not Selected Cards: ", notSelectedCards);

                int randomIndex = UnityEngine.Random.Range(0, notSelectedCards.Count);
                clientBarrierSelections[pfID].Add(notSelectedCards[randomIndex].InstanceID);
                HelperFunctions.LogListContent("Newly Selected Cards: ", clientBarrierSelections[pfID]);
            }


            WriteTitleEventRequest writeTitleEvent = new WriteTitleEventRequest
            {
                EventName = PlayfabHelper.CustomEventNames.pd_server_made_barrier_selects.ToString(),
                Timestamp = DateTime.UtcNow,
                Body = new Dictionary<string, object>
                {
                    {"PlayFabID",  pfID},
                    {"NumberSelectedAtTimeout", currentlySelected.Length }
                }
            };
            PlayfabHelper.WriteTitleEvent(writeTitleEvent);
        }

        
        //psc.ConnectedPlayer.pla
        //clientBarrierSelections[pfID]
    }

    public void UpdateClientBarriers(string pfID, string instanceID)
    {
        if(clientBarrierSelections.ContainsKey(pfID))
        {
            if(clientBarrierSelections[pfID].Contains(instanceID))
            {
                if(clientBarrierSelections[pfID].Remove(instanceID))
                {
                    HelperFunctions.Log(pfID + " Successfully removed InstanceID: " + instanceID + 
                        "\n" + pfID + " Number of Barriers selected: " + clientBarrierSelections[pfID].Count);
                }
                else
                {
                    HelperFunctions.Log("Did not remove InstanceID: " + instanceID + " not sure why");
                }
            }
            else
            {
                clientBarrierSelections[pfID].Add(instanceID);
                HelperFunctions.Log(pfID + " Successfully Add InstanceID: " + instanceID + "\n" +
                    pfID + " Number of Barriers selected: " + clientBarrierSelections[pfID].Count);
            }
        }
        else
        {
            clientBarrierSelections.Add(pfID, new List<string>());
            clientBarrierSelections[pfID].Add(instanceID);
            HelperFunctions.Log(pfID + " added to dictionary" + "\n" + 
                "List object initialized" + "\n" + 
                "Successfully Add InstanceID: " + instanceID + "\n" +
                "Number of Barriers selected: " + clientBarrierSelections[pfID].Count);
        }
    }

    private void FinalizeBarrierSelection(string pfID)
    {
        var result = ParadigmServer.Instance.Connections.Find(conn => conn.PlayFabId == pfID);
        List<Card> foundCards = result.ConnectedPlayer.GetLocation(ValidLocations.Deck).GetContents().Where(cards => clientBarrierSelections[pfID].Contains(cards.InstanceID.ToString())).ToList();
        result.ConnectedPlayer.GetLocation(ValidLocations.Deck).MoveContent(foundCards, result.ConnectedPlayer.GetLocation(ValidLocations.BZ));

        HelperFunctions.Log(pfID + " has chosen the following barriers: ");
        HelperFunctions.LogListContent("Barriers: ", foundCards);
    }
}




