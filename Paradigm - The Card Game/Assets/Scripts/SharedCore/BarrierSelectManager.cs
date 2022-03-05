using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Utilities;
using System;
using System.Linq;

public class BarrierSelectManager : NetworkBehaviour
{
    [SyncVar]
    public int barriersToSelect = 12;

    [SyncVar]
    public int timeOnTimer = 60;

    public Text timerText;
    public Text canvasText;
    public GameObject clientSideBarrierObject;
    public Canvas sceneCanvas;

    public static List<ClientCardInfo> clientDeckContent = new List<ClientCardInfo>();

    private ServerSideBarrierSelect ServerSideBarrierSelect
    {
        get;
        set;
    }

    private ClientBarrierSelect ClientBarrierSelect
    {
        get;
        set;
    }


    public static BarrierSelectManager Instance
    {
        get;
        private set;
    }

    public int DefaultTimerTime
    {
        get { return 60; }
    }

    void Awake()
    {
        Instance = this;


    }

    void Start()
    {
        
        HelperFunctions.Log("Is start not called");
        if (isServer)
        {
            HelperFunctions.Log("This is the Server");
            ConvertCardsToCardShells();

            //StartCoroutine(BreakTimer());

            ServerSideBarrierSelect = this.gameObject.AddComponent<ServerSideBarrierSelect>();
        }
        else
        {

            HelperFunctions.Log("This is the client");
            GameObject clientSideBarrierManager = Instantiate(clientSideBarrierObject, this.gameObject.transform) as GameObject;
            ClientBarrierSelect = clientSideBarrierManager.GetComponent<ClientBarrierSelect>();
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = "Time Left: " + timeOnTimer;
    }

    #region Run on Server Functions
    /// <summary>
    /// This functions is run on the server to convert the Card class to CardShell
    /// To Avoid having to write custom serialization AND to minimize the amount of information
    /// the client has about the game state
    /// </summary>
    [Server]
    void ConvertCardsToCardShells()
    {
        try
        {
            foreach (ParadigmServerConnection pConn in ParadigmServer.Instance.Connections)
            {
                foreach (Card c in pConn.ConnectedPlayer.PlayerDeck.Content.ToArray())
                {
                    ClientCardInfo convertedCard = ConvertCard(c);
                    ValidLocations currentLocation = HelperFunctions.ParseEnum<ValidLocations>(convertedCard.CurrentLocation);
                    //HelperFunctions.Log("Inside ConvertCardsToCardShells Loop" + "\n"
                    //    + "Key added to NetworkPlayerInfo.Locations Dict: " + currentLocation);
                    if(!pConn.PlayerInfo.Locations.ContainsKey(currentLocation))
                    {
                        pConn.PlayerInfo.Locations.Add(currentLocation, new List<ClientCardInfo>());
                    }

                    //HelperFunctions.Log("Placing: " + convertedCard.InstanceID + " in the NetworkPlayer's " + currentLocation);
                    pConn.PlayerInfo.Locations[currentLocation].Add(convertedCard);
                }
            }
        }
        catch (Exception e)
        {
            HelperFunctions.CatchException(e);
        }  
    }

    [Server]
    ClientCardInfo ConvertCard(Card c)
    {
        c.MoveToGameStartLocation();
        ClientCardInfo cardInfo = new ClientCardInfo
        {
            ID = c.ID,
            InstanceID = c.InstanceID.ToString(),
            CurrentLocation = c.CurrentLocation.ToString()
        };

        return cardInfo;
    }

    [Server]
    IEnumerator BreakTimer()
    {
        int phewTime = 5;
        HelperFunctions.Log("Just holding for a sec");
        while (phewTime > 0)
        {
            yield return new WaitForSeconds(1);
            HelperFunctions.Log("Gonna hold for " + phewTime + " more seconds");
            phewTime--;
        }
    }

    [Command(requiresAuthority = false)]
    public void UpdateSelectedBarriersCmd(string playFabID, string instanceID, NetworkConnectionToClient sender = null)
    {
        if(isServerOnly)
        {
            ServerSideBarrierSelect.UpdateClientBarriers(playFabID, instanceID);
        }
        else
        {
            return;
        }
        
    }

    [Command(requiresAuthority = false)]
    public void RequestDeckContentsCmd(string pfId, NetworkConnectionToClient sender = null)
    {
        if(isServerOnly)
        {
            HelperFunctions.Log("Client: " + pfId + " requested Deck contents");
            ParadigmServerConnection psc = ParadigmServer.Instance.Connections.Find(c => c.PlayFabId == pfId);
            HelperFunctions.Log("Any Deck items on the Server? " + psc.PlayerInfo.Locations[ValidLocations.Deck].Count);
            SendDeckContentsRpc(psc.Connection, psc.PlayerInfo.Locations[ValidLocations.Deck].ToArray());
        }
        else
        {
            return;
        }
    }

    [Command(requiresAuthority = false)]
    public void ReceiveClientBarrierSelections(string pfId, string[] instanceIds, NetworkConnectionToClient sender = null)
    {

    }

    #endregion

    #region Run on Client Functions

    [TargetRpc]
    public void SendDeckContentsRpc(NetworkConnection clientTarget, ClientCardInfo[] cardShells)
    {
        try
        {
            ClientBarrierSelect clientBarrierComp = this.gameObject.GetComponent<ClientBarrierSelect>();
            clientBarrierComp.cardsInDeck = clientDeckContent = cardShells.ToList();
            HelperFunctions.Log("Executing on ConnectionId: " + clientTarget.connectionId);
            HelperFunctions.Log("Any thing in the args?: " + cardShells.Length);
            
            HelperFunctions.Log
                (
                    "Client received deck content from server" + "\n" +
                    "Deck Count: " + clientDeckContent.Count
                );
        }
        catch(Exception e)
        {
            HelperFunctions.CatchException(e);
        }
    }
    #endregion
}


