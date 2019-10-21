using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


 public class Deck: Location
    {
        private Family deckFam;
        private Card nextCard;

        public Deck(string name, Player p):base(name , p)
        {
            this.Name = name;
            this.Owner = p;
            this.deckFam = null;
        }

        public Deck(string name = "Deck")
        {
            this.Owner = null;
            this.deckFam = null;
            contents = new List<Card>();
            locations.Add(this);
            this.changes = new List<LocationChanges>();
            changesDict.Add(this, this.changes);
        }

        public bool AddCard(Card c)
        {
            c.Owner = this.Owner;
            if (deckFam == null)
            {
                if (this == null)
                {
                    Debug.Log("The Deck you're adding to is Null as fuck!");
                    return false;
                }

                this.AddContent(c);
                deckFam = c.getFam();  //set the family of the deck to the family of the first card added
                c.setLocation(this);
                return true;
            }
            else if (c.getFam().Name != deckFam.Name)
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

        public void Draw(int drawVal = 1)
        {
            //Debug.Log("Is this the AI" + this.Owner.IsAI);
            Debug.Log(this.Owner.Type +  " Cards in Deck " + this.Count);
            if (this.Count > 0)
            {
                if (drawVal == 1)
                {
                    this.MoveContent(this.Owner.GetLocation(ValidLocations.Hand));
                }
                else
                {
                    this.MoveContent(drawVal, this.Owner.GetLocation(ValidLocations.Hand));
                }
            }
        }

        public Majesty GetMajesty()
        {
            Majesty c = new Majesty();
            for (int i = 0; i < this.Count; i++)
            {
                if (this.GetContents()[i] is Majesty)
                {
                    c = (Majesty)this.GetContents()[i];
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
                if (c is Landscape)
                {
                    //Debug.Log("It's a Landscape " + c.getName() + " " + c.GetType().ToString());
                    landscapesInDeck.Add(c);
                }
            }

            //Debug.Log("Landscapes in Deck: " + landscapesInDeck.Count);

            return landscapesInDeck;
        }

        public Location GetLandsAsLocation()
        {
            Location lands = new Location("Landscapes", this.Owner);
            foreach (Card c in this.GetContents())
            {
                //Debug.Log(c.getName() + " " + c.getType());
                if (c is Landscape)
                {
                    //Debug.Log("It's a Landscape " + c.getName() + " " + c.GetType().ToString());
                    lands.AddContent(c);
                }
            }
            if(this.Owner == lands.Owner)
            {
                Debug.Log("We Good");
            }
            return lands;
        }

        public void GameStartSetup()
        {
            int count = 0;
            foreach (Card c in this.contents)
            {
                count = count + c.MoveToGameStartLocation();
            }
            if (Owner.Type == "AI")
            {
                Debug.Log("AI Moved " + count + " cards");
            }
            else
            {
                Debug.Log("HUman Moved " + count + " cards");
            }
            
        }
    }

