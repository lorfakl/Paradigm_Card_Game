using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// This function is called by the Control Board State class to send a updated
        /// board state object from the client across the internet to the BoardStateModel 
        /// object on the server  
        /// </summary>
        /// <param name="boardModelJson"></param>
        public static void SendNewBoardModelUpdate(BoardState boardStateUpdatedFromController)
        {
            string boardModelJson = JsonConvert.SerializeObject(boardStateUpdatedFromController, Formatting.Indented);
            if (isOnline)
            {
                //network call to other TransportLayr
            }
            else
            {
                BoardStateModel.UpdateBoardModel(JObject.Parse(boardModelJson));
            }
        }

        /// <summary>
        /// This function is called by the BoardStateModel class to send a updated
        /// board state object across the internet to the BoardStateView object on the
        /// client machine
        /// </summary>
        /// <param name="boardStateModelDIct"></param>
        public static void SendBoardModelToView(BoardState boardState)
        {
            string jsonBoardString = JsonConvert.SerializeObject(boardState, Formatting.Indented);

            if (isOnline)
            {
                //sent it to the client machine
                /*The client on the recieving end would then figure out which player it is and
                 update the view as described in the JSON recieved*/
            }
            else
            {
                BoardStateView.ReceivedUpdatedModel(JObject.Parse(jsonBoardString));
            }
        }

    }
}
