using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using CustomNetworkMessages;
using Utilities;
using PlayFab.ServerModels;
using PlayFab.ClientModels;
using PlayFab;

/*
    Authenticators: https://mirror-networking.com/docs/Components/Authenticators/
    Documentation: https://mirror-networking.com/docs/Guides/Authentication.html
    API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkAuthenticator.html
*/

public class PlayFabAuthenticator : NetworkAuthenticator
{
    #region Server

    /// <summary>
    /// Called on server from StartServer to initialize the Authenticator
    /// <para>Server message handlers should be registered in this method.</para>
    /// </summary>
    //[Server]
    public override void OnStartServer()
    {
        // register a handler for the authentication request we expect from client
        NetworkServer.RegisterHandler<ClientAuthRequestMessage>(OnAuthRequestMessage, false);
        string serverLey = PlayFabMultiplayerAgentAPI.ServerIdKey;
    }

    /// <summary>
    /// Called on server from OnServerAuthenticateInternal when a client needs to authenticate
    /// </summary>
    /// <param name="conn">Connection to client.</param>
    //[Server]
    public override void OnServerAuthenticate(NetworkConnection conn) { }

    /// <summary>
    /// Called on server when the client's AuthRequestMessage arrives
    /// </summary>
    /// <param name="conn">Connection to client.</param>
    /// <param name="msg">The message payload</param>
    //[Server]
    public void OnAuthRequestMessage(NetworkConnection conn, ClientAuthRequestMessage msg)
    {
        HelperFunctions.Log("Server received Authentication Request message from Client");

        HelperFunctions.Log("Attempting to Authenticate session ticket");
        PlayfabHelper.OnAuthSessionTicketSuccess += OnSuccessfulClientAuthentication;
        PlayfabHelper.AuthenticateSessionTicket(msg.sessionTicket, conn, OnFailedClientAuthentication);  
    }

    [Server]
    public void OnFailedClientAuthentication(PlayFabError playFabError, NetworkConnection conn)
    {
        ServerAuthResponseMessage authResponseMessage = new ServerAuthResponseMessage
        {
            message = PlayfabHelper.DisplayPlayFabError(playFabError),
            resultCode = playFabError.HttpCode.ToString()
        };
        HelperFunctions.Log("Client Authentication Failed, rejected Client connection");
        conn.Send(authResponseMessage);
        StartCoroutine(HelperFunctions.Timer(3));
        ServerReject(conn);
    }
    [Server]
    public void OnSuccessfulClientAuthentication(AuthenticateSessionTicketResult response, NetworkConnection conn)
    {
        if(response.IsSessionTicketExpired == false && (ParadigmServer.Instance.Connections.Count <= 2))
        { //if ticket is not expired and there are less than 2 players connected
            HelperFunctions.Log("Server Successfully authed the client");
            ServerAuthResponseMessage authResponseMessage = new ServerAuthResponseMessage
            {
                message = "Success, you are authenticated",
                resultCode = 200.ToString()
            };

            conn.Send(authResponseMessage);
            ParadigmServer.Instance.UpdateServerConnections(new ParadigmServerConnection
            {
                PlayFabId = response.UserInfo.PlayFabId,
                Connection = conn,
                ConnectionId = conn.connectionId,
                IsAuthenticated = true,
                ServerId = ParadigmServer.Instance.ServerID
            });
            // Accept the successful authentication
            ServerAccept(conn);
        }
        else if(response.IsSessionTicketExpired == true)
        {
            HelperFunctions.Log("Server unable to authed the client. Invalid SessionTicket");
            ServerAuthResponseMessage authResponseMessage = new ServerAuthResponseMessage
            {
                message = "Invalid Session Ticket",
                resultCode = "401"
            };
            HelperFunctions.Log("Client Authentication Failed, rejected Client connection");
            conn.Send(authResponseMessage);
            StartCoroutine(HelperFunctions.Timer(3));
            ServerReject(conn);
        }
        else
        {
            HelperFunctions.Log("Server unable to auth the client. Max Clients Reached");
            ServerAuthResponseMessage authResponseMessage = new ServerAuthResponseMessage
            {
                message = "Max Number of Players Connected another Server needs to be spun up",
                resultCode = "MaxPlayers"
            };
            HelperFunctions.Log("Client Authentication Failed, already 2 players connected");
            conn.Send(authResponseMessage);
            StartCoroutine(HelperFunctions.Timer(3));
            ServerReject(conn);
        }
        
    }

    #endregion

    #region Client

    /// <summary>
    /// Called on client from StartClient to initialize the Authenticator
    /// <para>Client message handlers should be registered in this method.</para>
    /// </summary>
    //[Client]
    public override void OnStartClient()
    {
        // register a handler for the authentication response we expect from server
        NetworkClient.RegisterHandler<ServerAuthResponseMessage>(OnAuthResponseMessage, false);
    }

    /// <summary>
    /// Called on client from OnClientAuthenticateInternal when a client needs to authenticate
    /// </summary>
    /// <param name="conn">Connection of the client.</param>
    //[Client]
    public override void OnClientAuthenticate(NetworkConnection conn)
    {
        Debug.Log("Client is required to auth");
        PlayfabHelper.OnLoginSuccess += OnLoginSuccess;
        PlayfabHelper.Login();
    }

    [Client]
    private void OnLoginSuccess(LoginResult success)
    {
        //_messageWindow.Title.text = "Login Successful";
        HelperFunctions.Log("Successful PlayFab Login");
        //_messageWindow.gameObject.SetActive(true);
        if (NetworkClient.connection == null)
        {
            Debug.Log("This is null for some reason");
        }

        HelperFunctions.Log("Client Sent Authenticated Message");
        NetworkClient.connection.Send(new ClientAuthRequestMessage()
        {
            sessionTicket = success.SessionTicket
        });
    }

    /// <summary>
    /// Called on client when the server's AuthResponseMessage arrives
    /// </summary>
    /// <param name="conn">Connection to client.</param>
    /// <param name="msg">The message payload</param>
    //[Client]
    public void OnAuthResponseMessage(ServerAuthResponseMessage msg)
    {
        if(msg.resultCode == "200")
        {
            Debug.Log("Client Authentication Successful");
            ClientAccept(NetworkClient.connection);
        }
        else
        {
            Debug.Log("Client Authentication Failed please try again");
            //ClientReject(NetworkClient.connection);
        }
    }

    #endregion
}
