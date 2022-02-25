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
    private List<string> clientPlayFabIDs = new List<string>();

    private Dictionary<string, List<string>> clientBarrierSelections = new Dictionary<string, List<string>>();

    private void Awake()
    {
        NetworkServer.RegisterHandler<ClientCompletedBarrierSelection>(OnServerRecievedBarrierSelectionCompleted);
        NetworkServer.RegisterHandler<ClientBarrierSelectionTimeout>(OnServerReceivedClientBarrierSelectionTimeout);
    }

    private void Start()
    {
        StartCoroutine(BarrierSelectTimer());
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

        SelectBarriersForClient(msg.playFadId);
        FinalizeBarrierSelection(msg.playFadId);
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
    }

    private void SelectBarriersForClient(string pfID)
    {
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
                    HelperFunctions.Log("Successfully removed InstanceID: " + instanceID + 
                        "\n" + "Number of Barriers selected: " + clientBarrierSelections[pfID].Count);
                }
                else
                {
                    HelperFunctions.Log("Did not remove InstanceID: " + instanceID + " not sure why");
                }
            }
            else
            {
                clientBarrierSelections[pfID].Add(instanceID);
                HelperFunctions.Log("Successfully Add InstanceID: " + instanceID + "\n" +
                    "Number of Barriers selected: " + clientBarrierSelections[pfID].Count);
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
    }
}




