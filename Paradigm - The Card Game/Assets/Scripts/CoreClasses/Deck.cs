using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


 public class Deck: Location
    {
        private Family deckFam;
        private Card nextCard;

        public Deck(string name, Player p)
        {
            this.Name = name;
            this.Owner = p;
            this.deckFam = null;
        }

        public bool AddCard(Card c)
        {
            if (deckFam == null)
            {
                this.AddContent(c);
                deckFam = c.getFam();  //set the family of the deck to the family of the first card added
                c.setLocation(this);
                return true;
            }
            else if (c.getFam().FamString != deckFam.FamString)
            {
                //Debug.Log("Cannot add Card, does not Match Deck Family");
                return false;
            }
            else
            {
                this.AddContent(c);
                c.setLocation(this);
                return true;
            }
        }

        public void RemoveCard(Card c)
        {
            this.RemoveContent(c);
        }

        public List<Card> Draw(int drawVal = 1)
        {
            List<Card> cardsDrawn = new List<Card>();
            if (drawVal == 1)
            {
                cardsDrawn.Add(this.GetContents()[0]);
                return cardsDrawn;
            }
            else
            {
                for (int i = 0; i < drawVal; i++)
                {
                    cardsDrawn.Add(this.GetContents()[i]);
                    this.RemoveCard(this.GetContents()[i]);
                }
                return cardsDrawn;
            }
        }

        public Accessor GetMajesty()
        {
            Accessor c = new Accessor();
            for (int i = 0; i < this.Count; i++)
            {
                if (this.GetContents()[i] is Majesty)
                {
                    c = (Accessor)this.GetContents()[i];
                }
            }

            return c;
        }


        public List<Card> GetLandscapesInDeck()
        {
            List<Card> landscapesInDeck = new List<Card>();

            foreach (Card c in this.GetContents())
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

