using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace CustomNetworkMessages
{
    public struct AuthenticatedMessage : NetworkMessage
    {
        public string sessionTicket;
    }

    public struct ClientSessionTicketMessage : NetworkMessage
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

    public struct CountMessage : NetworkMessage
    {
        public int number;
        public int timesSent;
    }
    public struct ServerShuttingDownMessage : NetworkMessage
    {
        public string msg;
    }

    public struct ServerMaintenanceMessage : NetworkMessage
    {
        public string scheduledMaintenanceUTC;
    }
}
