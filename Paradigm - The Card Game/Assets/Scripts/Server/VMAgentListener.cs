using System.Collections;
using UnityEngine;
using PlayFab;
using System;
//using PlayFab.Networking;
using System.Collections.Generic;
using PlayFab.MultiplayerAgent.Model;
using CustomNetworkMessages;


public class VMAgentListener : MonoBehaviour
{
    private List<ConnectedPlayer> _connectedPlayers;
    public bool Debugging = true;
    // Use this for initialization

    void Start()
    {
        _connectedPlayers = new List<ConnectedPlayer>();
        PlayFabMultiplayerAgentAPI.Start();
        Debug.Log("Called GSDK Start");

        //GSDK Server Callbacks 
        PlayFabMultiplayerAgentAPI.IsDebugging = Debugging;
        PlayFabMultiplayerAgentAPI.OnMaintenanceCallback += OnMaintenance;
        PlayFabMultiplayerAgentAPI.OnShutDownCallback += OnShutdown;
        PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;
        PlayFabMultiplayerAgentAPI.OnAgentErrorCallback += OnAgentError;

        //Mirror callbacks
        ParadigmServer.Instance.OnPlayerAdded.AddListener(OnPlayerAdded);
        ParadigmServer.Instance.OnPlayerRemoved.AddListener(OnPlayerRemoved);
  
        
        StartCoroutine(ReadyForPlayers());
        Debug.Log(DateTime.Now + ": Leaving Unity Start");
    }

    IEnumerator ReadyForPlayers()
    {
        yield return new WaitForSeconds(.5f);
        PlayFabMultiplayerAgentAPI.ReadyForPlayers();
        Debug.Log(DateTime.Now + ": Called Ready for players");
    }

    private void OnServerActive()
    {
        Debug.Log(DateTime.Now + ":Server Started From Agent Activation ServerID: " +
            PlayFabMultiplayerAgentAPI.ServerIdKey + " was assigned");
        ParadigmServer.Instance.ServerID = PlayFabMultiplayerAgentAPI.ServerIdKey;
    }
    private void OnPlayerAdded(string playfabId)
    {
        Debug.Log(DateTime.Now + ": At the start of VMAgentL OnPlayerAdded");
        _connectedPlayers.Add(new ConnectedPlayer(playfabId));
        PlayFabMultiplayerAgentAPI.UpdateConnectedPlayers(_connectedPlayers);
        Debug.Log(DateTime.Now + ": called update connectedPlayers from PF. Leaving OnPlayerAdded");
    }

    private void OnPlayerRemoved(string playfabId)
    {
        ConnectedPlayer player = _connectedPlayers.Find(x => x.PlayerId.Equals(playfabId, StringComparison.OrdinalIgnoreCase));
        _connectedPlayers.Remove(player);
        PlayFabMultiplayerAgentAPI.UpdateConnectedPlayers(_connectedPlayers);
    }

    

    private void OnAgentError(string error)
    {
        Debug.Log(DateTime.Now + ": There was an agent error");
        Debug.Log(error);
    }

    private void OnShutdown()
    {
        Debug.Log("Server is shutting down");
        foreach (var conn in UnityNetworkServer.Instance.Connections)
        {
            conn.Connection.Send<ServerShuttingDownMessage>(new ServerShuttingDownMessage { msg = DateTime.Now + " Server is shutting down YA YEET!" });
        }
        StartCoroutine(Shutdown());
    }

    IEnumerator Shutdown()
    {
        Debug.Log(DateTime.Now + ": Inside the shutdown coroutine");
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }

    private void OnMaintenance(DateTime? NextScheduledMaintenanceUtc)
    {
        Debug.LogFormat("Maintenance scheduled for: {0}", NextScheduledMaintenanceUtc.Value.ToLongDateString());
        foreach (var conn in UnityNetworkServer.Instance.Connections)
        {
            conn.Connection.Send<ServerMaintenanceMessage>(new ServerMaintenanceMessage()
            {
                scheduledMaintenanceUTC = NextScheduledMaintenanceUtc.ToString()
            });
        }
    }
}
