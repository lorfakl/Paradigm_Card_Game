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

public class UnityNetworkServer : NetworkManager
    {
        public static UnityNetworkServer Instance { get; private set; }

        public PlayerEvent OnPlayerAdded = new PlayerEvent();
        public PlayerEvent OnPlayerRemoved = new PlayerEvent();

        public int MaxConnections = 100;
        public int Port = 7777; //important: docker container internal port

        
        public List<UnityNetworkConnection> Connections
        {
            get { return _connections; }
            private set { _connections = value; }
        }
        private List<UnityNetworkConnection> _connections = new List<UnityNetworkConnection>();

        
        public class PlayerEvent : UnityEvent<string> { }

        // Use this for initialization
        public override void Awake()
        {
            base.Awake();
            Instance = this;
            //NetworkServer.RegisterHandler<AuthenticatedMessage>(OnReceiveAuthenticate);
            NetworkServer.RegisterHandler<CountMessage>(OnReciveCountMessage);
            //PlayfabHelper.OnAuthSessionTicketSuccess += OnSuccessfulSessionTicketAuth;
            //_netManager.transport.port = Port;
            //Instance.OnStartServer();
        }

        public void StartListen()
        {
            NetworkServer.Listen(MaxConnections);
        }

        public override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            NetworkServer.Shutdown();
        }

         /*
        [Server]
        private void OnReceiveAuthenticate(NetworkConnection nconn, Ser message)
        {
            HelperFunctions.Log("Server received Authenticated Message");
            var conn = _connections.Find(c => c.ConnectionId == nconn.connectionId);
                if (conn != null)
                {
                    HelperFunctions.Log("Attempting to Authenticate session ticket");
                    PlayfabHelper.Instance.AuthenticateSessionTicket(message.sessionTicket);
                }
        }*/

        [Server]
        private void OnReciveCountMessage(NetworkConnection conn, CountMessage message)
        {
            HelperFunctions.Log("Server recieved Count Message");
            StartCoroutine(Timer(3, message.number, message.timesSent));
            
        }

        [Server]
        private void OnSuccessfulSessionTicketAuth(AuthenticateSessionTicketResult success)
        {
            HelperFunctions.Log("Successfilly Authenticate session ticket");
            OnPlayerAdded.Invoke(success.UserInfo.PlayFabId);
            
        }

        [Server]
        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);

            Debug.LogWarning("Client Connected");
            HelperFunctions.Log("Client Connectede");
            var uconn = _connections.Find(c => c.ConnectionId == conn.connectionId);
            if (uconn == null)
            {
                _connections.Add(new UnityNetworkConnection()
                {
                    Connection = conn,
                    ConnectionId = conn.connectionId,
                    LobbyId = PlayFabMultiplayerAgentAPI.SessionConfig.SessionId
                });
            }

            //OnPlayerAdded.Invoke();
        }

        [Obsolete]
        [Server]
        public override void OnServerError(NetworkConnection conn, int errorCode)
        {
            base.OnServerError(conn, errorCode);

            Debug.Log(string.Format("Unity Network Connection Status: code - {0}", errorCode));
        }

        [Server]
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);

            var uconn = _connections.Find(c => c.ConnectionId == conn.connectionId);
            if (uconn != null)
            {
                if (!string.IsNullOrEmpty(uconn.PlayFabId))
                {
                    OnPlayerRemoved.Invoke(uconn.PlayFabId);
                }
                _connections.Remove(uconn);
            }
        }
    
        IEnumerator Timer(int seconds, int val, int count)
        {
            HelperFunctions.Log("Counting in " + seconds + " seconds");
            yield return new WaitForSeconds(seconds);
            foreach(int connID in NetworkServer.connections.Keys.ToList())
            {
                NetworkServer.connections[connID].Send(new CountMessage {
                    number = val+1,
                    timesSent = count + 1} 
                );
            }
            HelperFunctions.Log("Sent Count message to client");
        }

    [Serializable]
    public class UnityNetworkConnection
    {
        public bool IsAuthenticated;
        public string PlayFabId;
        public string LobbyId;
        public int ConnectionId;
        public NetworkConnection Connection;
    }
}