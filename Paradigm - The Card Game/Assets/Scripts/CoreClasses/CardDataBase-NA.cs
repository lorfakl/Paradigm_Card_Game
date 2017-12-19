using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DataBase
{
    static class CardDataBase
    {

        private static List<Card> everySingleCard = new List<Card>();

        /// <summary>
        /// This function takes in an array of lines. this array is the entire database text file each string at the indecies
        /// represents an entire line of the database file.
        /// (On Line 31) a list of cards is declared 
        /// (On Line 33) a for loop goes through the entire lines array and grabs 9 indecies at a time
        /// this is because the text file is formatted in a way that every 9 lines holds the data of a single card
        /// (On Line 35) a temporary card is created with data from the file. it only goes from 0 to 7 because the 9th line 
        /// in the file is blank space
        /// (On Line 36) the temporary card is then added to the all cards list, once the loop finishes the all cards list will
        /// contain a card object for every card in the database
        /// (On Line 38) sets the card Collection list variable to contain all cards created from the file
        /// (On Line 39) returns allCards list even though I could just return getCardCollection
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<Card> LoadData(string[] lines)
        {
            List<Card> AllCards = new List<Card>();
            int cardInfoLine = 7;

            for (int i = 0; i < lines.Length; i += 9)
            {
                if(cardInfoLine +9 < lines.Length)
                {
                    cardInfoLine = cardInfoLine + 9;
                }
                Card temp = null;
                string[] e = { lines[i + 2] };
                string[] t = { lines[i + 1], lines[i + 7] };
                if (lines[cardInfoLine].Contains("Accessor") || lines[cardInfoLine].Contains("Elemental") || lines[cardInfoLine].Contains("Majesty"))
                {
                    
                    temp = new Accessor(lines[i], e, t, Int32.Parse(lines[i + 3]), Int32.Parse(lines[i + 4]), Convert.ToBoolean(Int32.Parse(lines[i + 5])), lines[i + 7]);

                    if (lines[cardInfoLine].Contains("Majesty"))
                    {
                        temp = (Majesty)temp;
                    }
                }
                else if(lines[cardInfoLine].Contains("Element"))
                {
                    temp = new Element(lines[i], e, t, Int32.Parse(lines[i + 3]), Convert.ToBoolean(Int32.Parse(lines[i + 5])), lines[i + 7]);

                }
                else if(lines[cardInfoLine].Contains("Mechanism"))
                {
                    temp = new Mechanism(lines[i], e, t, Int32.Parse(lines[i + 3]), Convert.ToBoolean(Int32.Parse(lines[i + 5])), lines[i + 7]);
                }
                else //Respirators and Philosophers
                {
                    temp = new AuxiliaryCard(lines[i], e, t, Convert.ToBoolean(Int32.Parse(lines[i + 5])), lines[i + 7]);
                }

                AllCards.Add(temp);
            }
            setCardCollection(AllCards);
            return AllCards;
        }
        /// <summary>
        /// Loads the individual lines from the file
        /// (On Line 53) A TextAsset is declared because text files in Unity are implicity converted to type TextAsset Objects
        /// when they are imported into the project. Resources.Load is used because it allows easy access to raw files inside 
        /// the project
        /// (On Line 54) the lines array is created by separating the dataBase file line by line, making each line of the 
        /// file a string. when it reaches a new line in the file is when the Split function knows to make a new string
        /// (On Line 55) calls the LoadData in order to create the Card objects
        /// (On Line 56) redundant
        /// </summary>
        public static void getFileData()
        {
            TextAsset cardCollection = Resources.Load("carddatabase") as TextAsset;
            string[] lines = cardCollection.text.Split("\n"[0]);
            List<Card> allCards = LoadData(lines);
            setCardCollection(allCards);
        }

        public static void setCardCollection(List<Card> cards)
        {
            everySingleCard = cards;
        }

        public static List<Card> getCardCollection()
        {
            return everySingleCard;
        } 


        /// <summary>
        /// //This function searches AllCards which is a reference to
        //A list containing all the cards in the carddatabase text file
        //The parameters here are the string being searched for known as searchVal
        //and the search modifier which is the name of the toggle that is on in the UI
        //Note: Think about adding a Const field to the AllCards parameter
        /// </summary>
        /// <param name="searchVal"></param>
        /// <param name="searchMod"></param>
        /// <param name="AllCards"></param>
        /// <returns></returns>

        public static List<Card> search(string searchVal, string searchMod, List<Card> AllCards)
        {
            List<Card> searchResults = new List<Card>(); //list for the results                 
            string[] searchMods = new string[3] { "nameToggle", "typeToggle", "famToggle" }; //this array contains all possible values of searchMod      
            //Debug.Log("Search: " + searchVal);

            //Debug.Log("Index? " + Array.IndexOf(searchMods, searchMod));
            switch (Array.IndexOf(searchMods, searchMod)) //since switch can only use ints and enums and the searchMod string HAS to be in the array the index is just returned
            {
                case 0:  //search with the name modifier
                    for (int i = 0; i < AllCards.Count; i++)
                    {
                        if (AllCards[i].getName().Contains(searchVal))
                        {
                            searchResults.Add(AllCards[i]);
                        }
                    }
                    break;

                case 1: //search with the type modifier
                    for (int i = 0; i < AllCards.Count; i++)
                    {
                        //Debug.Log("Type Search");
                        string debug = AllCards[i].getType();
                        //Debug.Log("This is the type: " + debug);
                        if (debug.Contains(searchVal))
                        {
                            //Debug.Log("Type Searching");
                            searchResults.Add(AllCards[i]);
                        }
                    }
                    break;

                case 2: //search with the Family modifier
                    for (int i = 0; i < AllCards.Count; i++)
                    {
                        if (AllCards[i].getFam().getFam().Contains(searchVal))
                        {
                            searchResults.Add(AllCards[i]);
                        }
                    }
                    break;

                default:
                    Debug.Log("Invalid"); //This line should NEVER run
                    break;
            }

            return searchResults; //return the list of results
        }
    }
}
