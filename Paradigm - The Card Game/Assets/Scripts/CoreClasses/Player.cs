using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Player
    {
       
        //private Turn playerTurn;
       
        private Dictionary<string, Locations> cardLocations;
        private string[] validLocations = { "Hand", "Grave", "LockZ", "BZ", "LandZ", "SC", "PZ", "DZ", "Field" };
        private Deck playerDeck;
        
        private AuxiliaryCard tcLandscape = null;
        private Card majesty;

        public Player()
        {
            foreach(string s in validLocations)
            {
                cardLocations.Add(s, new Locations(s));
            }
        }

        public Player(Deck d)
        {
            this.playerDeck = d;
            this.majesty = playerDeck.getMajesty();
            foreach (string s in validLocations)
            {
                cardLocations.Add(s, new Locations(s));
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

        public AuxiliaryCard TCLandscape
        {
            get { return tcLandscape; }
            set { tcLandscape = value; }
        }
        

        public Locations GetLocation(string l)
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
            cardLocations["Hand"].RemoveContent(c);
            cardLocations["Field"].AddContent(c);
        }
    
        public void AddBarrier(Card c)
        {
            c.setShard(true);
            c.setBarrierStatus(true);
            cardLocations["BZ"].AddContent(c);
        }

        public void AddToHand(Card c)
        {
            cardLocations["Hand"].AddContent(c);
        }
        //End Adds


        //Housing Keeping functions
        public void DestroyBarrier()
        {
            cardLocations["BZ"].GetContents()[0].setBarrierStatus(false);
            SendToShardPile(cardLocations["BZ"].GetContents()[0]);
            cardLocations["BZ"].RemoveContent();
        }
        //End Housing Keeping functions



        //Card Transit
        public void SendToGrave(Card c)
        {
            c.setDestoyedStatus(true);
            cardLocations["Grave"].AddContent(c);
        }

        public void SendToShardPile(Card c)
        {
            cardLocations["SC"].AddContent(c);
        }

        public void DrawFromDeck(int drawVal = 1)
        {
            List<Card> cardsDrawn = playerDeck.Draw(drawVal);
            foreach (Card c in cardsDrawn)
            {
                cardLocations["Hand"].AddContent(c);
            }
        }
        //End Card Transit
    }

