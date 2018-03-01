using System;
using System.Data;
using UnityEngine;
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
        public static GameEventsArgs RaiseNewEvent(object sender, Player owner, Player target, NonMoveAction nonMoveAction = NonMoveAction.None)
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
