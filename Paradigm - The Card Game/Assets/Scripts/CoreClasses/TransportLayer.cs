using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TransportLayer
{
    public static class EventIngestion
    {
        private static bool isOnline;
        private static string ipAddress;
        private static string port;


        public static void EventIntake(object s, GameEventsArgs e)
        {
            if(isOnline)
            {
                //serialize event data and send to server
            }
            else
            {
                //send to local event manager
                GameEventsManager.CheckLegality(s, e);
            }
        }
    }
}
