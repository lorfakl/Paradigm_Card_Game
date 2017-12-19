using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Player
    {
        private Deck playerDeck = new Deck();
        private List<Card> barriers = new List<Card>();
        //private Turn playerTurn;
        private List<Card> hand = new List<Card>();
        private List<Card> grave = new List<Card>();
        private List<Card> shardPile = new List<Card>();
        private List<Card> field = new List<Card>();
        private AuxiliaryCard TCLandscape = null;
        private Card majesty;

        public Player()
        {

        }

        public Player(Deck d)
        {
            playerDeck = d;
            majesty = playerDeck.getMajesty();

        }

        //Getters
        public List<Card> getPlayerHand(){ return hand; }
        public List<Card> getPlayerGrave(){ return grave; }
        public List<Card> getPlayerField(){ return field; }
        public List<Card> getShardPile(){ return shardPile; }
        public Card getTCLandscapeCard(){ return TCLandscape; }
        public int getBarriers(){ return barriers.Count; }
        //End Getters


        //Setters
        public void setTCLandscapeCard(Card c){ TCLandscape = (AuxiliaryCard)c; }

        public void setDeck(Deck d)
        {
            playerDeck = d;
            majesty = d.getMajesty();
        }

        public void addToField(Card c)
        {
            hand.Remove(c);
            field.Add(c);
        }

        //End Setters

        //Removes
        public void removeTCLandscapeCard()
        {
            TCLandscape = null;
        }

        //End Removes

        //Adds
        public void AddBarrier(Card c)
        {
            c.setShard(true);
            c.setBarrierStatus(true);
            barriers.Add(c);
        }

        public void AddToHand(Card c)
        {
            hand.Add(c);
        }
        //End Adds


        //Housing Keeping functions
        public void DestroyBarrier()
        {
            barriers[0].setBarrierStatus(false);
            SendToShardPile(barriers[0]);
            barriers.RemoveAt(0);
        }
        //End Housing Keeping functions



        //Card Transit
        public void SendToGrave(Card c)
        {
            c.setDestoyedStatus(true);
            grave.Add(c);
        }

        public void SendToShardPile(Card c)
        {
            shardPile.Add(c);
        }

        public void DrawFromDeck(int drawVal = 1)
        {
            List<Card> cardsDrawn = playerDeck.Draw(drawVal);
            foreach (Card c in cardsDrawn)
            {
                hand.Add(c);
            }
        }
        //End Card Transit
    }

