using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ServerModels;
using CustomNetworkMessages;
using Utilities;
using System.Collections;
using System.Linq;



public class ParadigmServer : NetworkManager
{
    public int MaxConnections = 180;
    public int Port = 7777;

    public UnityEvent<string> OnPlayerAdded = new UnityEvent<string>();
    public UnityEvent<string> OnPlayerRemoved = new UnityEvent<string>();


    public static ParadigmServer Instance
    {
        get;
        private set;
    }

    public List<ParadigmServerConnection> Connections
    {
        get;
        private set;
    }

    public string ServerID
    {
        get;
        set;
    }

    public override void Awake()
    {
        base.Awake();
        Instance = this;
        Connections = new List<ParadigmServerConnection>();
        NetworkServer.RegisterHandler<ClientSessionTicketMessage>(OnServerReceiveSessionTicketMessage);
        NetworkServer.RegisterHandler<ClientLogMessage>(OnServerReceiveClientLogMessage);
        PlayfabHelper.OnAuthSessionTicketSuccess += OnSuccessfulSessionTicketAuth;
        PlayfabHelper.OnWriteTelemetrySuccess += OnSuccessfulWriteTelemetryEvent;

        //Contact Azure and load newest database
    }

    public void StartListen()
    {
        NetworkServer.Listen(MaxConnections);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        
        HelperFunctions.Log("Client Connected");
        var uconn = Connections.Find(c => c.ConnectionId == conn.connectionId);

        if (uconn == null)
        {
            uconn = new ParadigmServerConnection()
            {
                Connection = conn,
                ConnectionId = conn.connectionId,
                ServerId = ServerID
            };
            Connections.Add(uconn);
        }
        WriteConnectionEvent(uconn, PlayfabHelper.CustomEventNames.pd_player_connected_server);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        var uconn = Connections.Find(c => c.ConnectionId == conn.connectionId);

        WriteConnectionEvent(uconn, PlayfabHelper.CustomEventNames.pd_player_disconnected_server);
        HelperFunctions.Log(uconn.PlayFabId + " has disconnected from " + uconn.ServerId);

        if (uconn != null)
        {
            if (!string.IsNullOrEmpty(uconn.PlayFabId))
            {
                OnPlayerRemoved.Invoke(uconn.PlayFabId);
            }
            Connections.Remove(uconn);
        }
        
    }

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        NetworkServer.Shutdown();
    }

    [Server]
    private void OnServerReceiveSessionTicketMessage(NetworkConnection nconn, ClientSessionTicketMessage message)
    {
        HelperFunctions.Log("Server received Authenticated Message");
        var conn = Connections.Find(c => c.ConnectionId == nconn.connectionId);
        if (conn != null)
        {
            HelperFunctions.Log("Attempting to Authenticate session ticket");
            PlayfabHelper.AuthenticateSessionTicket(message.sessionTicket);
        }
    }

    [Server]
    private void OnServerReceiveClientLogMessage(NetworkConnection conn, ClientLogMessage message)
    {
        HelperFunctions.Log("Server recieved Count Message");
        

    }

    [Server]
    private void OnSuccessfulSessionTicketAuth(AuthenticateSessionTicketResult success)
    {
        HelperFunctions.Log("Successfilly Authenticate session ticket");
        OnPlayerAdded.Invoke(success.UserInfo.PlayFabId);

    }

    private void OnSuccessfulWriteTelemetryEvent(WriteEventResponse success)
    {
        HelperFunctions.Log("Successfully wrote " + success.EventId + " to PlayFab");
    }

    private void WriteConnectionEvent(ParadigmServerConnection uconn, PlayfabHelper.CustomEventNames eventName)
    {
        if(uconn is null)
        {
            HelperFunctions.Log("Uconn in WriteConnectionEvent is null. Skipping this function");
            return;
        }
        
        WriteServerPlayerEventRequest connectEvent = new WriteServerPlayerEventRequest
        {
            PlayFabId = uconn.PlayFabId,
            EventName = eventName.ToString(),
            Timestamp = DateTime.UtcNow,
            Body = new Dictionary<string, object>
            {
                {"ServerID", ServerID }
            }
        };
        PlayfabHelper.WritePlayerSpecificEvent(connectEvent);
    }
}

[Serializable]
public class ParadigmServerConnection
{
    public bool IsAuthenticated;
    public string PlayFabId;
    public string ServerId;
    public int ConnectionId;
    public NetworkConnection Connection;
}