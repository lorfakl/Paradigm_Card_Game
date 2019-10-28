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
                c.MoveToGameStartLocation();
                //Debug.Log("Name: " + c.Name + " ID: " + c.Owner.PlayerID);
                //Debug.Log("Location Name: " + c.getLocation().Name + " ID: " + c.getLocation().Owner.PlayerID);

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
                c.MoveToGameStartLocation();
                //Debug.Log("Name: " + c.Name + " ID: " + c.Owner.PlayerID);
                //Debug.Log("Location Name: " + c.getLocation().Name + " ID: " + c.getLocation().Owner.PlayerID);
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
                if (this.Content[i] is Majesty)
                {
                    c = (Majesty)this.Content[i];
                }
            }
            
            //MoveContent(c, c.Owner.GetLocation(ValidLocations.DZ), true);
            if(c.HP == 0)
            {
                foreach(Card crd in this.Owner.GetLocation(ValidLocations.DZ))
                {
                    if(crd is Majesty)
                    {
                        c = (Majesty)crd;
                    }
                }
            }

            return c;
        }


        public List<Card> GetLandscapesInDeck()
        {
            List<Card> landscapesInDeck = new List<Card>();

            foreach (Card c in this)
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
            Debug.Log("New location ID: " + lands.Owner.PlayerID);
            foreach (Card c in this)
            {
                //Debug.Log(c.getName() + " " + c.Owner.PlayerID);
                if (c is Landscape)
                {
                    //Debug.Log("It's a Landscape " + c.getName() + " " + c.GetType().ToString());
                    lands.AddContent(c);
                }
            }

            if (lands.Count == 0)//check the dormant zone
            {
                foreach(Card c in this.Owner.GetLocation(ValidLocations.DZ))
                {
                    Debug.Log(c.getName() + " " + c.Owner.PlayerID);
                    Debug.Log("Is the ID still the same: " + this.Owner.GetLocation(ValidLocations.DZ).Owner.PlayerID);
                    if (c is Landscape)
                    {
                        //Debug.Log("It's a Landscape " + c.getName() + " " + c.GetType().ToString());
                        lands.AddContent(c);
                    }
                }
            }

            if(this.Owner == lands.Owner)
            {
                Debug.Log("We Good");
            }

            return lands;
        }

        public Location GetLandsAsLocation(Location l)
        {
            Location lands = new Location("Landscapes", this.Owner);
            //Debug.Log("What should be other location ID: " + l.Owner.PlayerID);
            //Debug.Log("New location ID: " + lands.Owner.PlayerID);
            if (lands.Count == 0)//check the dormant zone
            {
                foreach (Card c in l)
                {
                    Debug.Log(c.getName() + " " + c.Owner.PlayerID);
                    c.Owner = this.Owner;
                    //Debug.Log("Post Change " + c.getName() + " " + c.Owner.PlayerID);
                    //Debug.Log("Is the ID still the same: " + l.Owner.PlayerID);
                    if (c is Landscape)
                    {
                        //Debug.Log("It's a Landscape " + c.getName() + " " + c.GetType().ToString());
                        lands.AddContent(c);
                    }
                }
            }

            return lands;
        }

        public void GameStartSetup(bool status)
        {
            int count = 0;
            foreach (Card c in this.contents.ToArray())
            {
                count = count + c.MoveToGameStartLocation();
                
            }
            
            //Debug.Log(status + " Moved " + count + " cards");
            
            
        }
    }

