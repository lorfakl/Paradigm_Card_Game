﻿
using System;
//using System.IO;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
//using System.Reflection;
//using System.Collections;
//using System.Globalization;
using System.Collections.Generic;
using Builder;
using Utilities;

namespace DataBase
{
    class CardDataBase
    {
        private static List<int> allCardIDs = new List<int>();
        private static List<Card> allCards = new List<Card>();
        private static Dictionary<string, CardConstrInfo> typeDict = new Dictionary<string, CardConstrInfo>();
        private static Dictionary<int, (Type type, System.Object[] arguments)> cardDict = new Dictionary<int, (Type type, System.Object[] arguments)>();
#if UNITY_SERVER
        private static string dbConnString = "URI=file:" + System.IO.Directory.GetCurrentDirectory() + "\\CardDataBase.db"; //get database file path
#else
        private static string dbConnString = "URI=file:" + Application.dataPath + "/CardDataBase.db";
#endif

        private enum SearchMod { nameToggle, typeToggle, famToggle }
        private static bool isDataLoaded = false;
        private static List<int> playerDeckContents = new List<int>();
        private static bool isPlayerDeckLoaded = false;
        private static bool isCardDictLoaded = false;

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
            isDataLoaded = true;
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
            if (!isDataLoaded)
            {
                PrepareDictionary();
            }
#if UNITY_SERVER
            string conn = "URI=file:" + System.IO.Directory.GetCurrentDirectory() + "\\CardDataBase.db"; //get database file path
#else
            string conn = "URI=file:" + Application.persistentDataPath + "/CardDataBase.db";
#endif
            HelperFunctions.Log("Database local scope file path" + conn);
            HelperFunctions.Log("Database global scope file path" + dbConnString);
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

                                int iD = Int32.Parse(reader[0].ToString());
                        
                                cardDict.Add(iD, (cardType, ob));
                                allCardIDs.Add(iD);
                                
                                
                            }
                            else
                            {
                                //Debug.Log(reader[1] + " does NOT map to a valid card type");
                            }
                        }
                    }
                }

                dbconn.Close();
                isDataLoaded = true;
                isCardDictLoaded = true;

                
            }
        }

        private static Card CreateCardInstance(int id, Player p)
        {
            if (!isCardDictLoaded)
            {
                GetDataBaseData();
            }

            var (type, arguments) = cardDict[id];
            Card c = (Card)Activator.CreateInstance(type, arguments);

            c.ID = id;
            c.ClassType = c.GetType().ToString();
            c.Owner = p;
            c.GenerateInstanceID();
            AbilityBuilder.CreateAbilityInstance(c);

            return c;
        }

        public static Card CreateCardInstance(int id)
        {
            if (!isCardDictLoaded)
            {
                GetDataBaseData();
            }

            var (type, arguments) = cardDict[id];
            Card c = (Card)Activator.CreateInstance(type, arguments);

            c.ID = id;
            c.ClassType = c.GetType().ToString();
            c.GenerateInstanceID();
            AbilityBuilder.CreateAbilityInstance(c);

            return c;
        }

        public static Dictionary<string, string> GetClientSafeCardInfo(int id)
        {
            Dictionary<string, string> cardInfo = new Dictionary<string, string>();

            Card c = CreateCardInstance(id);
            cardInfo.Add("Name", c.Name);
            cardInfo.Add("Fam", c.Family);
            cardInfo.Add("Type", c.ClassType);
            
            for(int i = 0; i< c.Abilities.Count; i++)
            {
                string ablKey = "Abl" + i;
                cardInfo.Add(ablKey, c.Abilities[i].AbilityText);
            }

            return cardInfo;

        }

        public static void LoadAllCardsList()
        {
            if(!isCardDictLoaded)
            {
                GetDataBaseData();
            }

            foreach(int id in allCardIDs)
            {
                var (type, arguments) = cardDict[id];
                Card c = (Card)Activator.CreateInstance(type, arguments);

                c.ID = id;
                c.ClassType = c.GetType().ToString();


                foreach (Ability a in c.getAbilities())
                {

                    //a.LinkToCard(c);
                    //throw new Exception("You have to fix rhe card ability text also do the art too");
                }

                foreach (Trait t in c.getTraits())
                {
                    t.LinkToCard(c.getName());
                }

                allCards.Add(c);
            }
        }

        public static void SavePlayerDeck(Deck d)
        {
            using (SqliteConnection dbconn = new SqliteConnection(dbConnString))
            {
                dbconn.Open(); //connect to database
                using (SqliteCommand cmd = dbconn.CreateCommand()) //create sql command object
                {
                    //string countQuery = "SELECT count(*) FROM PlayerDeck";
                    //cmd.CommandText = countQuery;
                    //int tableCount = Int32.Parse(cmd.ExecuteScalar().ToString());

                    if (IsPlayerDeckSaved())
                    {
                        string deleteQuery = "DELETE FROM PlayerDeck";
                        //Debug.Log("Not Empty deleteing old deck");
                        cmd.CommandText = deleteQuery;
                        cmd.ExecuteNonQuery();
                    }

                    if(!IsPlayerDeckSaved())
                    {
                        //Debug.Log("Just Got Wiped");
                    }

                    string queryPortion = "INSERT INTO PlayerDeck (ID, Name) VALUES ("; //sql command text
                    playerDeckContents.Clear(); //removes old saved deck data for when the game doesn't restart so this static class is still in memory
                    foreach (Card c in d)
                    {
                        cmd.CommandText = queryPortion + c.ID + ", " + "'" + c.getName() + "')";
                        //Debug.Log(cmd.CommandText);
                        cmd.ExecuteNonQuery();
                        playerDeckContents.Add(c.ID);
                    }
                    //cmd.CommandText = query; //put sql command text in the command object

                }
            }
        }

        public static List<int> LoadPlayerDeck()
        {
            if (!isPlayerDeckLoaded)
            {
                using (SqliteConnection dbconn = new SqliteConnection(dbConnString))
                {
                    dbconn.Open(); //connect to database
                    using (SqliteCommand cmd = dbconn.CreateCommand()) //create sql command object
                    {
                        string query = "SELECT * FROM PlayerDeck"; //sql command text
                        cmd.CommandText = query; //put sql command text in the command object

                        using (IDataReader reader = cmd.ExecuteReader()) //run query
                        {
                            while (reader.Read())
                            {
                                playerDeckContents.Add(Int32.Parse(reader[0].ToString()));
                            }

                            isPlayerDeckLoaded = true;
                            return playerDeckContents;
                        }
                    }
                }
            }
            else
            {
                return playerDeckContents;
            }
        }

        public static bool IsPlayerDeckSaved()
        {
            using (SqliteConnection dbconn = new SqliteConnection(dbConnString))
            {
                dbconn.Open(); //connect to database
                using (SqliteCommand cmd = dbconn.CreateCommand()) //create sql command object
                {
                    string countQuery = "SELECT count(*) FROM PlayerDeck";
                    cmd.CommandText = countQuery;
                    int tableCount = Int32.Parse(cmd.ExecuteScalar().ToString());

                    if(tableCount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool IsDataLoaded
        {
            get { return isDataLoaded; }
        }

        public static List<int> SavedDeckContents
        {
            get { return playerDeckContents; }
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
                        if (allCards[i].getFam().Name.Contains(searchVal))
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

            if (playerDeck == null)
            {
                throw new Exception("You passed in a null deck to MakePlayerDeck");
            }

            if (!isDataLoaded && !AbilityBuilder.isJSONDataLoaded)
            {
                GetDataBaseData();
                AbilityBuilder.ParseAbilityJSON();
            }

            if (IsPlayerDeckSaved())
            {
                foreach (int id in LoadPlayerDeck())
                {
                    playerDeck.AddCard(CreateCardInstance(id, p));
                }
                
                return;
            }

            

            foreach (int id in allCardIDs)
            {
                Debug.Log("Card ID: " + id);
                Card c = CreateCardInstance(id, p);
                Debug.Log("Card Instance NAme: " + c.Name);
                if (playerDeck.AddCard(c)) 
                {
                    c.setOwner(p);
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