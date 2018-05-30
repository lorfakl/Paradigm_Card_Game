using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Player
    {
       
        //private Turn playerTurn;
       
        private Dictionary<string, Location> cardLocations = new Dictionary<string, Location>();
        private static string[] validLocations = { "Hand", "Grave", "LockZ", "BZ", "LandZ", "SC", "PZ", "DZ", "Field", "Deck" };
        private Deck playerDeck;
        private int playerID;
        //private AuxiliaryCard tcLandscape = null;
        private Majesty majesty;
        private Turn turn;
        private static List<Player> currentPlayers = new List<Player>();

        public Player(GameTimeManager mgmt, int addTo = 0)
        {
        
            this.playerID = UnityEngine.Random.Range(0,256);
            if (addTo != 0)
            {
                this.playerID = this.playerID + UnityEngine.Random.Range(510, 2048);
            }

            this.majesty = null;
            this.turn = new Turn(this, mgmt);

            foreach (string s in validLocations)
            {
                cardLocations.Add(s, new Location(s, this));
            }
            //Debug.Log("Dictionary Size: " + cardLocations.Count);
            this.playerDeck = new Deck("Deck", this);
            cardLocations["Deck"] = this.playerDeck;
        }

        public Deck PlayerDeck
        {
            get { return playerDeck; }
            set { playerDeck = value; }
        }

        public Majesty Majesty
        {
            get { return majesty; }
            set { majesty = value; }
        }

        public int PlayerID
        {
            get { return playerID; }
        }

        public Turn PlayerTurn
        {
            get { return this.turn; }
            set { this.turn = value; }
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

