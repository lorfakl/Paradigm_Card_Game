using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TransportLayer
{
    public static class EventIngestion
    {
        private static bool isOnline = false;
        private static string ipAddress;
        private static string port;

        public static bool IsOnline
        {
            get;
        }

        public static void EventIntake(object s, GameEventsArgs e)
        {
            if(isOnline)
            {
                //serialize event data and send to server
            }
            else
            {
                //send to local event manager
                GameEventsManager.AddToStack(s, e);
            }
        }

        public static void SendReturnEvent(GameEventsArgs e)
        {

        }



    }
}
