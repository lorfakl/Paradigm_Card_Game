using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace CustomNetworkMessages
{
    #region Messages sent from Clients
    public struct ClientAuthRequestMessage : NetworkMessage
    {
        public string sessionTicket;
    }

    public struct ClientLogMessage : NetworkMessage
    {
        public string playFabId;
        public string logMessage;
        public string stackTrace;
        public string time;
    }

    public struct ClientCompletedBarrierSelection : NetworkMessage
    {
        public string playFadId;
        public int barriersSelected;
    }

    public struct ClientBarrierSelectionTimeout : NetworkMessage
    {
        public string playFadId;
        public string[] instanceIds;
    }

    public struct ClientCurrentCardSelection : NetworkMessage
    {
        public string[] instanceIds;
    }

    public struct ClientRecievedCardsFromServer : NetworkMessage
    {
        public string pfID;
        public int cardsRecieved;
    }

    #endregion


    #region Messages sent from Server

    public struct ServerAuthResponseMessage : NetworkMessage
    {
        public string resultCode;
        public string message;
    }

    public struct ServerShuttingDownMessage : NetworkMessage
    {
        public string msg;
    }

    public struct ServerMaintenanceMessage : NetworkMessage
    {
        public string scheduledMaintenanceUTC;
    }

    public struct ServerResetBarrierTimer : NetworkMessage
    {
        public string dateTimeUTC;
        public int timeOnTimer;
    }

    #endregion




    public struct CountMessage : NetworkMessage
    {
        public int number;
        public int timesSent;
    }
    
}
