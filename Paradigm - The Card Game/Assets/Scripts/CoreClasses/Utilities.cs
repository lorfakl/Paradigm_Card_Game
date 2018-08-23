using System;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Collections.Generic;


/// <summary>
/// The utilities class is for storing the general purpose commonly used functions and data structures 
/// </summary>
namespace Utilities
{
    
    
    public static class HelperFunctions
    {
        private static string conn = "URI=file:" + Application.dataPath + "/CardDataBase.db";

        /// <summary>
        /// TODO ADD A FILTER TO CHOOSE WHICH CARDS ARE DISPLAYED(most work to be 
        /// done in DisplaySelectionCards.cs)
        /// This is the simplife function call to display a list of cards to the 
        /// player so that they can select x amount of cards and those cards are 
        /// sent to the destination to be process or whatever
        /// </summary>
        /// <param name="source">The location object the cards are being taken from</param>
        /// <param name="destination">The location object the cards are being place in</param>
        /// <param name="numToSelect">The number of cards the user is allowed to select</param>
        public static GameObject SelectCards(Location source, Location destination, int numToSelect)
        {
            GameObject display = GameObject.Instantiate(Resources.Load("DisplayOverlay")) as GameObject;
            if(source.Owner.PlayerID == destination.Owner.PlayerID)
            {
                Debug.Log("We started good");
            }
            display.GetComponentInChildren<DisplaySelectionCards>().SetCardPath(source, destination, numToSelect);
            return display;
        }

        /// <summary>
        /// This function Instantiates a Card prefab for you
        /// </summary>
        /// <param name="c">Card object that contains the data the Gameobject needs to have</param>
        /// <param name="inDisplayMode">Boolean to choose whether this GameObject is in DisplaySelection mode</param>
        /// <param name="parent">Transform of the parent</param>
        /// <param name="cardPrefab">Optional: If you have a prefab reference or nah</param>
        /// <returns>GameOject so that the number of objects created can be kept by the caller</returns>
        public static GameObject CreateCard(Card c, bool inDisplayMode, Transform parent, GameObject cardPrefab = null)
        {
            GameObject cardObject = null; 
            if (cardPrefab == null)
            {
                cardObject = GameObject.Instantiate(Resources.Load("Card", typeof(GameObject)), parent) as GameObject;
                Debug.Log("Instantiated prefab");
            }
            else
            {
                cardObject = GameObject.Instantiate(cardPrefab, parent) as GameObject;
            }
            cardObject.SendMessage("SetMode", inDisplayMode);
            cardObject.GetComponent<CardScript>().SetCard(c);
            Transform cardName = cardObject.transform.FindDeepChild("cardName");
            cardName.GetComponent<Text>().text = c.Name;
            Transform content = cardObject.transform.FindDeepChild("Content");
            content.GetComponent<Text>().text = c.GetAbilityText();
            if (c == null)
            {
                throw new Exception("The Card's null dumbass!(CreateCard)");
            }
            //position = cardObject.transform.position;

            return cardObject;
        }


        public static MonoBehaviour AccessMonoBehaviour()
        {
            GameObject monoAccess = GameObject.FindWithTag("MonobehaviourAccessor");
            return monoAccess.GetComponent<MonoBehaviourAccessor>();
        }

        /// <summary>
        /// Raise a new event that is the direct result of a card effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="boardChanges"></param>
        /// <param name="source"></param>
        /// <param name="moveAction"></param>
        /// <param name="notMoveAction"></param>
        /// <param name="cardTargets"></param>
        public static void RaiseNewEvent(object sender, List<LocationChanges> boardChanges, Card source, MoveAction moveAction,
                                NonMoveAction notMoveAction, List<Card> cardTargets)
        {
            GameEventsArgs newEvent = new GameEventsArgs(boardChanges, source, moveAction, notMoveAction, cardTargets);
            GameEventsManager.PublishEvent(sender, newEvent);
        }

        /// <summary>
        /// Raise a new event that is not related to a specific card but instead its a core game event like a turn phase transition or dimension twist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="owner"></param>
        /// <param name="target"></param>
        public static GameEventsArgs RaiseNewEvent(object sender, Player owner, Player target, NonMoveAction nonMoveAction)
        {
            GameEventsArgs newEvent = new GameEventsArgs(owner, target, nonMoveAction);
            GameEventsManager.PublishEvent(sender, newEvent);
            return newEvent;
        }

        /// <summary>
        /// Raise a new event that is directly related to Card movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="boardChanges"></param>
        /// <param name="moveAction"></param>
        public static void RaiseNewEvent(object sender, List<LocationChanges> boardChanges, MoveAction moveAction)
        {
            GameEventsArgs newEvent = new GameEventsArgs(boardChanges, moveAction);
            GameEventsManager.PublishEvent(sender, newEvent);
        }

        public static void RaiseNewEvent(object sender, Card cardSource, NonMoveAction notMoveAction, Card cardTarget)
        {
            GameEventsArgs newEvent = new GameEventsArgs(cardSource, notMoveAction, cardTarget);
            GameEventsManager.PublishEvent(sender, newEvent);
        }

        /// <summary>
        /// This functions is to both read and write from the Database. If you read from a Table, the function returns a Dictionary with keys 0 - Records returned
        /// If you write to a Table well, thats not implemented yet.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="command"></param>
        /// <param name="read"></param>
        /// <returns></returns>
        public static Dictionary<int, List<System.Object>> AccessDataBaseTable(string table, bool read = true, string command = "")
        {
            Dictionary<int, List<System.Object>> tableData = new Dictionary<int, List<System.Object>>();
            using (SqliteConnection dbConnection = new SqliteConnection(conn))
            {
                dbConnection.Open();
                using (SqliteCommand cmd = dbConnection.CreateCommand())
                {
                    if(read)
                    {
                        string query = "SELECT * FROM " + table; //get the info from the table 
                        cmd.CommandText = query;
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            int recordsReturned = 0;
                            while (reader.Read())
                            {
                                List<System.Object> recordData = new List<System.Object>();
                                for(int i = 0; i < reader.FieldCount; i++)
                                {
                                    recordData.Add(reader[i]);
                                }
                                tableData.Add(recordsReturned, recordData);
                                recordsReturned++;
                            }
                            return tableData;
                        }
                    }
                    else //write
                    {
                        Debug.Log("Currently You Can only read. Write hasn't been implemented");
                        return null;
                    }
                }
            }
        }
    }


    public static class Dictionaries
    {
        //this static class will be responsible for mmapping card names to their abilities and traits
        //which map to the functions that actually do the things for abilities and traits
        private static bool arePrepared = false;
        
        
        public static bool Prepared
        {
            get { return arePrepared; }
        }

        public static void PrepareAllDictionaries()
        {


            arePrepared = true;

        }

        
    }
}
