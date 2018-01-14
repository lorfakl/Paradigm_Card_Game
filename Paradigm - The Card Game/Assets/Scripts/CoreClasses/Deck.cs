using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


 public class Deck
    {
        private List<Card> deck = new List<Card>();

        private Family deckFam;
        private Player ownership;
        private Card nextCard;

        public Deck()
        {
            deckFam = null;
        }

        public bool addCard(Card c)
        {
            if (deckFam == null)
            {
                deck.Add(c);

                deckFam = c.getFam();  //set the family of the deck to the family of the first card added
                return true;
            }
            else if (c.getFam().getFam() != deckFam.getFam())
            {
                //Debug.Log("Cannot add Card, does not Match Deck Family");
                return false;
            }
            else
            {
                deck.Add(c);

                return true;
            }
        }

        public void removeCard(Card c)
        {
            deck.Remove(c);
        }

        public List<Card> Draw(int drawVal = 1)
        {
            List<Card> cardsDrawn = new List<Card>();
            if (drawVal == 1)
            {
                cardsDrawn.Add(deck[0]);
                return cardsDrawn;
            }
            else
            {
                for (int i = 0; i < drawVal; i++)
                {
                    cardsDrawn.Add(deck[i]);
                    removeCard(deck[i]);
                }
                return cardsDrawn;
            }
        }

        public List<Card> getDeck()
        {
            return deck;
        }

        public int getDeckSize()
        {
            return deck.Count;
        }

        public Accessor getMajesty()
        {
            Accessor c = new Accessor();
            for (int i = 0; i < deck.Count; i++)
            {
                if (deck[i] is Majesty)
                {
                    c = (Accessor)deck[i];
                }
            }

            return c;
        }


        public List<Card> getLandscapesInDeck()
        {
            List<Card> landscapesInDeck = new List<Card>();

            foreach (Card c in deck)
            {
                //Debug.Log(c.getName() + " " + c.getType());
                if (c is AuxiliaryCard)
                {
                    //Debug.Log("It's a Landscape " + c.getName() + " " + c.getType());
                    landscapesInDeck.Add(c);
                }
            }

            //Debug.Log("Landscapes in Deck: " + landscapesInDeck.Count);

            return landscapesInDeck;
        }
    }

