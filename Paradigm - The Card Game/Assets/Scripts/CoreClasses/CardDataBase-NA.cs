using System;
using System.IO;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

namespace DataBase
{
    class CardDataBase
    {
        private static List<Card> allCards = new List<Card>();
        private static Dictionary<string, CardConstrInfo> typeDict = new Dictionary<string, CardConstrInfo>();
        private static string dbConnString = "URI=file:" + Application.dataPath + "/CardDataBase.db";
        private enum SearchMod { nameToggle, typeToggle, famToggle }
        private static bool isDataLoaded = false;

        private struct CardConstrInfo
        {
            public Type t;
            public int maxArgs;
        }

        private static void PrepareDictionary()
        {         
            typeDict.Add("Accessor", MakeEntry(typeof(Accessor), 8));
            typeDict.Add("Element", MakeEntry(typeof(Element), 7));
            typeDict.Add("Majesty", MakeEntry(typeof(Majesty), 8));
            typeDict.Add("Source", MakeEntry(typeof(Source), 6));
            typeDict.Add("Mechanism", MakeEntry(typeof(Mechanism), 7));
            typeDict.Add("Phantom", MakeEntry(typeof(Phantom), 9));
            typeDict.Add("Philosopher", MakeEntry(typeof(Philosopher), 6));
            typeDict.Add("Landscape", MakeEntry(typeof(Landscape), 6));
        }

        private static CardConstrInfo MakeEntry(Type t, int i)
        {
            CardConstrInfo info = new CardConstrInfo();
            info.t = t;
            info.maxArgs = i;

            return info;
        }

        public static void GetDataBaseData()
        {
            PrepareDictionary();
            string conn = "URI=file:" + Application.dataPath + "/CardDataBase.db"; //get database file path

            using (SqliteConnection dbconn = new SqliteConnection(conn))
            {
                dbconn.Open(); //connect to database
                using (SqliteCommand cmd = dbconn.CreateCommand()) //create sql command object
                {
                    string query = "SELECT * FROM Cards"; //sql command text
                    cmd.CommandText = query; //put sql command text in the command object

                    using (IDataReader reader = cmd.ExecuteReader()) //run query
                    {
                        while (reader.Read())
                        {
                            if (typeDict.ContainsKey(reader[1].ToString()))
                            {
                                CardConstrInfo constrInfo = typeDict[reader[1].ToString()];
                                Type cardType = constrInfo.t;
                                List<System.Object> recordData = new List<System.Object>();
                                for (int i = 2; i < reader.FieldCount; i++) //go through the columns of the DB record 
                                                                            //and removes any NULL values before processing
                                {
                                    if (!reader.IsDBNull(i))
                                    {
                                        recordData.Add(reader[i]);
                                    }
                                }

                                while (recordData.Count < constrInfo.maxArgs)
                                {
                                    recordData.Add("");
                                }

                                if (recordData.Count > constrInfo.maxArgs)
                                {
                                    Debug.Log("Definitely gonna break");
                                }

                                System.Object[] ob = recordData.ToArray();
                                Card c = (Card)Activator.CreateInstance(cardType, ob);
                                Debug.Log("Dynamically Created Card: " + c.getName());
                                allCards.Add(c);
                            }
                            else
                            {
                                Debug.Log(reader[1] + " does NOT map to a valid card type");
                            }
                        }
                    }
                }

                dbconn.Close();
                isDataLoaded = true;
            }
        }

        public bool IsDataLoaded
        {
            get { return isDataLoaded; }
        }
        
        public static List<Card> GetAllCards(){ return allCards; }
        public static Card GetCard(string n)
        {
            if(!isDataLoaded)
            {
                GetDataBaseData();
            }

            foreach(Card c in allCards)
            {
                if(c.getName() == n)
                {
                    return c;
                }
            }

            return null;
        }
        /// <summary>
        /// //This function searches allCards which is a reference to
        //A list containing all the cards in the carddatabase
        //The parameters here are the string being searched for known as searchVal
        //and the search modifier which is the name of the toggle that is on in the UI
        //This function returns a list of cards found in the search
        /// </summary>
        /// <param name="searchVal"></param>
        /// <param name="searchMod"></param>
        /// <returns></returns>

        public static List<Card> Search(string searchVal, string searchMod)
        {
            
            List<Card> searchResults = new List<Card>(); //list for the results
            Debug.Log("Search Mod: " + searchMod + " SearchVal: " + searchVal);
            string[] searchMods = new string[3] { "nameToggle", "typeToggle", "famToggle" }; //this array contains all possible values of searchMod      
            SearchMod currentSearchMod = (SearchMod)Enum.Parse(typeof(SearchMod), searchMod); //mapping the search mod name to an enum value
            switch (currentSearchMod) //since switch can only use ints and enums and the searchMod string HAS to be in the array the index is just returned
            {
                case SearchMod.nameToggle:  //search with the name modifier
                    for (int i = 0; i < allCards.Count; i++)
                    {
                        if (allCards[i].getName().Contains(searchVal))
                        {
                            searchResults.Add(allCards[i]);
                            Debug.Log("Found: " + allCards[i].getName());
                        }
                    }
                    break;

                case SearchMod.typeToggle: //search with the type modifier, now that we're dealing with actual type this is slightly more involved

                    CardConstrInfo value; //to get the type being searched for from the typeDict
                    if(typeDict.TryGetValue(searchVal, out value)) //out keyword require for pass by reference and somehow this function returns 2 values a bool and TValue of the Dictionary
                    {
                        for (int i = 0; i < allCards.Count; i++)
                        {
                            if(allCards[i].GetType() == value.t)//get the type of i and compare to type from dict entry
                            {
                                searchResults.Add(allCards[i]);
                                Debug.Log("Found: " + allCards[i].getName());
                            }
                        }
                    }
                    else
                    {
                        Debug.Log(searchVal + " is not a valid card type, this is case-sensitive");
                    }
                    break;

                case SearchMod.famToggle: //search with the Family modifier
                    for (int i = 0; i < allCards.Count; i++)
                    {
                        if (allCards[i].getFam().FamString.Contains(searchVal))
                        {
                            searchResults.Add(allCards[i]);
                            Debug.Log("Found: " + allCards[i].getName());
                        }
                    }
                    break;

                default:
                    Debug.Log("Invalid"); //This line should NEVER run
                    break;
            }

            return searchResults; //return the list of results
        }

        public static void MakePlayerDeck(Player p)
        {
            Deck playerDeck = p.PlayerDeck;

            if(!isDataLoaded)
            {
                GetDataBaseData();
            }

            if (playerDeck == null)
            {
                Debug.Log("Null as fuck!");
                return;
            }

            foreach (Card c in allCards)
            {
                if(playerDeck.AddCard(c))
                {
                    Debug.Log("Card Add Success");
                }
                else
                {
                    Debug.Log("Card Failed to Add");
                }
            }

            Debug.Log("Deck created with " + playerDeck.Count + " cards");
            
        }
    }
}
