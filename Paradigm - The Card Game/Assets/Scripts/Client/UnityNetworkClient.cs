using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using Utilities;

public class UnityNetworkClient : NetworkManager 
{
    public static UnityNetworkClient Instance { get; private set; }

    public ConnectedEvent OnConnected = new ConnectedEvent();
    public DisconnectedEvent OnDisconnected = new DisconnectedEvent();

    public class ConnectedEvent : UnityEvent { }
    public class DisconnectedEvent : UnityEvent<int?> { }

    public override void Awake()
    {
        base.Awake();
        Instance = this;
        this.authenticator.OnClientAuthenticated.AddListener(OnClientAuthenticated);
    }

    public override void Start()
    {
        base.Start();
        this.StartClient();
    }


    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("connected");
        OnConnected.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        Debug.Log("disconnected");
        OnDisconnected.Invoke(null);
    }

    private void OnClientAuthenticated(NetworkConnection client)
    {
        Debug.Log("This proves, private methods can subscribe to events");
        Debug.Log("Client is authenticated by the way");

        ClientStartUp.Instance.authStat.text = string.Format("Logged in as ID:{0}", PlayfabHelper.PlayFabID);
    }

    /// <summary>
    /// Called on clients when a servers tells the client it is no longer ready.
    /// <para>This is commonly used when switching scenes.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientNotReady(NetworkConnection conn) { }

    /// <summary>
    /// Called from ClientChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows client to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
    /// <param name="sceneOperation">Scene operation that's about to happen</param>
    /// <param name="customHandling">true to indicate that scene loading will be handled through overrides</param>
    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling) { }

    /// <summary>
    /// Called on clients when a scene has completed loaded, when the scene load was initiated by the server.
    /// <para>Scene changes can cause player objects to be destroyed. The default implementation of OnClientSceneChanged in the NetworkManager is to add a player object for the connection if no player object exists.</para>
    /// </summary>
    /// <param name="conn">The network connection that the scene change message arrived on.</param>
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }



    /// <summary>
    /// This is invoked when the client is started.
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();
    }


    /// <summary>
    /// This is called when a client is stopped.
    /// </summary>
    public override void OnStopClient() { }
}
