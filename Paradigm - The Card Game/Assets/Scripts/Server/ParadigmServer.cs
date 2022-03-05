
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
using DataBase;


public class ParadigmServer : NetworkManager
{
    public int MaxConnections = 2;
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

    public List<Player> ConnectedPlayers
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
        ConnectedPlayers = new List<Player>();

        NetworkServer.RegisterHandler<ClientLogMessage>(OnServerReceiveClientLogMessage);
        NetworkServer.RegisterHandler<CountMessage>(OnReciveCountMessage);
        //PlayfabHelper.OnAuthSessionTicketSuccess += OnSuccessfulSessionTicketAuth;
        PlayfabHelper.OnWriteTelemetrySuccess += OnSuccessfulWriteTelemetryEvent;
        string serverLey = PlayFabMultiplayerAgentAPI.ServerIdKey;
        //Contact Azure and load newest database but for now load the local DB file
        //using CardDataBase-NA make required calls. 

    }

    public ParadigmServerConnection GetParadigmServerConnection(NetworkConnection conn)
    {
        ParadigmServerConnection psc = Connections.Find(target => target.ConnectionId == conn.connectionId);
        return psc;
    }

    /// <summary>
    /// Called to update the Connections variable. Used to update PlayFab related information
    /// about the underlying Network connection
    /// </summary>
    /// <param name="conn"></param>
    public void UpdateServerConnections(ParadigmServerConnection conn)
    {
        foreach(ParadigmServerConnection connection in Connections)
        {
            if(connection.ConnectionId == conn.ConnectionId)
            {
                connection.Connection = conn.Connection;
                connection.IsAuthenticated = conn.IsAuthenticated;
                connection.PlayFabId = conn.PlayFabId;
                connection.ServerId = conn.ServerId;

                return;
            }
        }

        //Connection does not currently exist in the Connections
        Connections.Add(conn);
    }

    public void StartListen()
    {
        NetworkServer.Listen(MaxConnections);
    }

    #region Client Connection Callbacks
    public override void OnServerConnect(NetworkConnection conn)
    {
        if(ConnectedPlayers.Count == 2)
        {
            return;
        }

        base.OnServerConnect(conn);

        HelperFunctions.Log("Client Connected");
        var uconn = Connections.Find(c => c.ConnectionId == conn.connectionId);

        /*if (uconn == null)
        {
            uconn = new ParadigmServerConnection()
            {
                Connection = conn,
                ConnectionId = conn.connectionId,
                ServerId = PlayFabMultiplayerAgentAPI.SessionConfig.SessionId
            };
            Connections.Add(uconn);
        }*/
        WriteConnectionEvent(uconn, PlayfabHelper.CustomEventNames.pd_player_connected_server);

        Player p = new HumanPlayer(uconn.PlayFabId);
        uconn.ConnectedPlayer = p;
        CardDataBase.MakePlayerDeck(p);
        ConnectedPlayers.Add(p);
        HelperFunctions.Log("Player Object created");
        HelperFunctions.Log("Deck has " + p.PlayerDeck.Count + "cards in it");
        HelperFunctions.Log("Player DZ Count: " + p.GetLocationCount(ValidLocations.DZ.ToString()));
        HelperFunctions.Log("Player BZ Count: " + p.GetLocationCount(ValidLocations.BZ.ToString()));
        HelperFunctions.Log("Player Hand Count: " + p.GetLocationCount(ValidLocations.Hand.ToString()));
        HelperFunctions.Log("Player Grave Count: " + p.GetLocationCount(ValidLocations.Grave.ToString()));
        HelperFunctions.Log("Player Field Count: " + p.GetLocationCount(ValidLocations.Field.ToString()));
        HelperFunctions.Log("Player LandZ Count: " + p.GetLocationCount(ValidLocations.LandZ.ToString()));
        uconn.PlayerInfo = new NetworkPlayerInfo(uconn.PlayFabId);

        if(Connections.Count == 2)
        {
            WriteTitleEventRequest writeTitleEvent = new WriteTitleEventRequest
            {
                EventName = PlayfabHelper.CustomEventNames.pd_server_scene_change.ToString(),
                Timestamp = DateTime.UtcNow,
                Body = new Dictionary<string, object>
                {
                    {"PlayFabIDs",  new string[2]{ Connections[0].PlayFabId, Connections[1].PlayFabId } },
                    {"SceneChangeTo", "BarrierSelectScene" }
                }
            };
            PlayfabHelper.WriteTitleEvent(writeTitleEvent);


            ServerChangeScene("BarrierSelectScene");
        }

    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        var uconn = Connections.Find(c => c.ConnectionId == conn.connectionId);

        
        HelperFunctions.Log(uconn.PlayFabId + " has disconnected from " + uconn.ServerId);

        if (uconn != null)
        {
            if (!string.IsNullOrEmpty(uconn.PlayFabId))
            {
                OnPlayerRemoved.Invoke(uconn.PlayFabId);
            }

            ConnectedPlayers.Remove(uconn.ConnectedPlayer);
            Connections.Remove(uconn);

        }

        WriteConnectionEvent(uconn, PlayfabHelper.CustomEventNames.pd_player_disconnected_server);
    }
    #endregion

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        NetworkServer.Shutdown();
    }

    [Server]
    private void OnServerReceiveClientLogMessage(NetworkConnection conn, ClientLogMessage message)
    {
        HelperFunctions.Log("Server recieved Count Message");
        

    }

    [Server]
    private void OnReciveCountMessage(NetworkConnection conn, CountMessage message)
    {
        //HelperFunctions.Log("Server recieved Count Message");
        StartCoroutine(Timer(3, message.number, message.timesSent));

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
                {"ServerId", ServerID },
                {"ConnectionId", uconn.ConnectionId },
                {"Total Connections", Connections.Count }
            }
        };
        PlayfabHelper.WritePlayerSpecificEvent(connectEvent);
    }

    private void WritePlayerEvent(string playFabID, Dictionary<string, object> eventData, PlayfabHelper.CustomEventNames eventName)
    {
       
        WriteServerPlayerEventRequest connectEvent = new WriteServerPlayerEventRequest
        {
            PlayFabId = playFabID,
            EventName = eventName.ToString(),
            Timestamp = DateTime.UtcNow,
            Body = eventData
        };
        PlayfabHelper.WritePlayerSpecificEvent(connectEvent);
    }

    IEnumerator Timer(int seconds, int val, int count)
    {
        //HelperFunctions.Log("Counting in " + seconds + " seconds");
        yield return new WaitForSeconds(seconds);
        foreach (int connID in NetworkServer.connections.Keys.ToList())
        {
            NetworkServer.connections[connID].Send(new CountMessage
            {
                number = val + 1,
                timesSent = count + 1
            }
            );
        }
        //HelperFunctions.Log("Sent Count message to client");
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
    public Player ConnectedPlayer;
    public NetworkPlayerInfo PlayerInfo;
}

public struct ClientCardInfo
{
    public int ID;
    public string InstanceID;
    public string CurrentLocation;

    public override string ToString() =>
        $"ID: {ID}; InstanceID: {InstanceID}; Location: {CurrentLocation}";
}

[Serializable]
public class NetworkPlayerInfo
{
    private readonly string playFabId;

    public bool IsCurrentTurn
    {
        get;
        set;
    }

    public string PlayFabID
    {
        get { return playFabId; }
    }

    public Dictionary<ValidLocations, List<ClientCardInfo>> Locations
    {
        get;
    }

    public NetworkPlayerInfo(string pfID)
    {
        playFabId = pfID;
        Locations = new Dictionary<ValidLocations, List<ClientCardInfo>>();
        foreach(ValidLocations v in Enum.GetValues(typeof(ValidLocations)))
        {
            Locations.Add(v, new List<ClientCardInfo>());
        }
    }

    public override string ToString()
    {
        
        string content = $"PlayFabId: {playFabId}" + "\n";
        content += $"IsCurrentTurn: {IsCurrentTurn}" + "\n";
        foreach(ValidLocations key in Locations.Keys)
        {
            content += $"Location: {key.ToString()}; Count: {Locations[key].Count}" + "\n";
        }
        return content;
    }

}
