using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Player
    {
       
        //private Turn playerTurn;
       
        private Dictionary<string, Location> cardLocations;
        private string[] validLocations = { "Hand", "Grave", "LockZ", "BZ", "LandZ", "SC", "PZ", "DZ", "Field" };
        private Deck playerDeck;
        private int playerID;
        //private AuxiliaryCard tcLandscape = null;
        private Card majesty;
        private static List<Player> currentPlayers = new List<Player>();

        public Player()
        {
            this.playerID = new System.Random().Next(256);
            foreach (Player p in currentPlayers)
            {
            
                while (this.playerID == p.playerID)
                {
                    this.playerID = new System.Random().Next(256);
                }
            }

            foreach (string s in validLocations)
            {
                cardLocations.Add(s, new Location(s, this));
            }
        }

        public Player(Deck d)
        {
            this.playerDeck = d;
            this.majesty = playerDeck.getMajesty();
            foreach (string s in validLocations)
            {
                cardLocations.Add(s, new Location(s, this));
            }
        }

        public Deck PlayerDeck
        {
            get { return playerDeck; }
            set { playerDeck = value; }
        }

        public Card Majesty
        {
            get { return majesty; }
            set { majesty = value; }
        }

        /*public AuxiliaryCard TCLandscape
        {
            get { return tcLandscape; }
            set { tcLandscape = value; }
        }*/
        

        public Location GetLocation(string l)
        {
            bool validVal = false;
            foreach (string s in validLocations)
            {
                if(s.Contains(l))
                {
                    validVal = true;
                }
            }

            if(!validVal)
            {
                Debug.Log("Invalid Argument!");
                return null;
            }

            return cardLocations[l];
        }


        public void AddToField(Card c)
        {
            cardLocations["Hand"].MoveContent(c, cardLocations["Field"]);   
        }
    
        public void AddBarrier(Card c)
        {
            c.setShard(true);
            c.setBarrierStatus(true);
            c.getLocation().MoveContent(c, cardLocations["BZ"]);
        }

        public void AddToHand(Card c)
        {
            c.getLocation().MoveContent(c, cardLocations["BZ"]);
        }
        //End Adds


        //Housing Keeping functions
        public void DestroyBarrier()
        {
            Card c = cardLocations["BZ"].GetContents()[0];
            c.setBarrierStatus(false);
            c.getLocation().MoveContent(c, cardLocations["SC"]);
        }
        //End Housing Keeping functions

        //Card Transit
        public void SendToGrave(Card c)
        {
            c.setDestoyedStatus(true);
            c.getLocation().MoveContent(c, cardLocations["Grave"]);
        }

        public void SendToShardPile(Card c)
        {
            c.getLocation().MoveContent(c, cardLocations["SC"]);
        }

        public void DrawFromDeck(int drawVal = 1)
        {
            List<Card> cardsDrawn = playerDeck.Draw(drawVal);
            foreach (Card c in cardsDrawn)
            {
                c.getLocation().MoveContent(c, cardLocations["Hand"]);
            }
        }
        //End Card Transit
    }

