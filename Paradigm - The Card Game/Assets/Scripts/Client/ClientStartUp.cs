using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using System;
using PlayFab.ClientModels;
using Mirror;
using TMPro;
using Utilities;
using CustomNetworkMessages;
using System.Linq;


public class ClientStartUp : MonoBehaviour
{

    UnityNetworkClient _nm;
    [SerializeField]
    TMP_Text connStat;

    public TMP_Text authStat;

    [SerializeField]
    TMP_Text serverStat;

    public bool useOtherAccount;

    public static ClientStartUp Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        // _authService = PlayFabAuthService.Instance;
        // PlayFabAuthService.OnDisplayAuthentication += OnDisplayAuth;
        //PlayFabAuthService.OnLoginSuccess += OnLoginSuccess;

        _nm = UnityNetworkClient.Instance;
        _nm.OnDisconnected.AddListener(OnDisconnected);
        _nm.OnConnected.AddListener(OnConnected);
        
        NetworkClient.RegisterHandler<ServerShuttingDownMessage>(OnServerShutdown);
        NetworkClient.RegisterHandler<ServerMaintenanceMessage>(OnMaintenanceMessage);
        NetworkClient.RegisterHandler<CountMessage>(OnReciveCountMessage);
        


        //_messageWindow = MessageWindow.Instance;
    }

    

    [Client]
    private void OnMaintenanceMessage(ServerMaintenanceMessage msg)
    {
        var message = msg;
        //_messageWindow.Title.text = "Maintenance Shutdown scheduled";
        //_messageWindow.Message.text = string.Format("Maintenance is scheduled for: {0}", message.ScheduledMaintenanceUTC.ToString("MM-DD-YYYY hh:mm:ss"));
        // _messageWindow.gameObject.SetActive(true);
    }

    [Client]
    private void OnReciveCountMessage(NetworkConnection conn, CountMessage message)
    {
        //HelperFunctions.Log("Client Reicved Count Message");
        //HelperFunctions.Log("Count Contents: " + message.number + " sent: " + message.timesSent +" times");
        StartCoroutine(Timer(3, message.number, message.timesSent));

    }

    [Client]
    private void OnServerShutdown(ServerShuttingDownMessage msg)
    {
        //   _messageWindow.Title.text = "Shutdown In Progress";
        // _messageWindow.Message.text = "Server has issued a shutdown.";
        // _messageWindow.gameObject.SetActive(true);
        NetworkClient.Disconnect();
    }

    [Client]
    private void OnConnected()
    {
        connStat.text = "Connected to Server";
        serverStat.text = NetworkClient.serverIp.ToString();
        Utilities.HelperFunctions.Log("Connected");
        
        //HelperFunctions.Log("Sending Count");
        //NetworkClient.connection.Send(new CountMessage());

    }

    [Client]
    IEnumerator Timer(int seconds, int val, int count)
    {
        //HelperFunctions.Log("Counting in " + seconds + " seconds");
        yield return new WaitForSeconds(seconds);
        NetworkClient.connection.Send(new CountMessage
        {
            number = val + 1,
            timesSent = count + 1
        }
             );
        //HelperFunctions.Log("Client Sent Count messafg");
    }



    private void OnDisconnected(int? code)
    {
        if(connStat != null)
        {
            connStat.text = "Disconnected!";
        }
        
        //_messageWindow.Message.text = "You were disconnected from the server";
        //_messageWindow.gameObject.SetActive(true);
    }
}
