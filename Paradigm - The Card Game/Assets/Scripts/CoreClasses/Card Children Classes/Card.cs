using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


 public abstract class Card
{
        //TODO TRAITS NEED TO BE THOUGHT OUT
        private string name;
        private int id;
        private List<Ability> abilities = new List<Ability>();
        private List<Trait> traits = new List<Trait>();
        private Family fam;
        private Player owner;
        private bool isShard;
        private bool inPlay;
        private bool isBarrier;
        private bool isDestroyed;
        private Location currentLocation;

        protected void RemoveAttribute(string l)
        {
            if (l == "a")
            {
                //Debug.Log("Removing Ability");
                abilities.RemoveAt(abilities.Count - 1);
            }
            else if (l == "t")
            {
                //Debug.Log("Removing Trait");
                traits.RemoveAt(traits.Count - 1);
            }
            else
            {
                throw new Exception("Invalid Parameter to Card.RemoveAttribute");
            }
        }

        //Properties
        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public List<Ability> Abilities
        {
            get { return this.abilities; }
        }

        public List<Trait> Traits
        {
            get { return this.traits; }
        }

        public bool Shard
        {
            get { return this.isShard; }
            set { this.isShard = value; }
        }

        public Player Owner
        {
            get { return this.owner; }
            set { this.owner = value; }
        }

        //Getters
        public string getName() { return name; }
        public List<Ability> getAbilities() { return abilities; }
        public List<Trait> getTraits() { return traits; }
        public bool getShard() { return isShard; }
        public Player getOwner() { return owner; }
        public Family getFam() { return fam; }
        public Location getLocation() { return currentLocation; }
        public bool getBarrierStatus() { return isBarrier; }
        public bool getPlayStatus() { return inPlay; }
    

        //Setters
        public void setName(string n) { name = n; }
        public void addAbility(Ability a) { abilities.Add(a); }
        public void addTrait(Trait t) { traits.Add(t); }
        public void setShard(bool sh) { isShard = sh; }
        public void setOwner(Player p) { owner = p; }
        public void setFam(Family f) { fam = f; }
        public void setLocation(Location l) { currentLocation = l; }
        public void setBarrierStatus(bool b) { isBarrier = b; }
        public void setPlayStatus(bool b) { inPlay = b; }
        public void setDestoyedStatus(bool s) { isDestroyed = s; }

        public void SetTraits(string text)
        {
            foreach (string tr in SplitTrait(text)) { this.addTrait(new Trait(tr, this.name)); }
        }
        
        public void SetAbilities(string a, string a2, string a3)
        {
            string[] abs = { a, a2, a3 };
            for (int i = 0; i < 3; i++)
            {
                if (abs[i] != "")
                {
                    this.addAbility(new Ability(this.name, abs[i]));
                }      
            }
        }

        public string GetAbilityText()
        {
            string abText = "";

            foreach(Ability a in abilities)
            {
                abText = abText + a.AbilityText + System.Environment.NewLine;
            }

            return abText;
        }

        public static ShapeTrait GetShape(Card c)
        {
            Debug.Log(c.name + " is a " + c.GetType().ToString());
            if (c.GetType() == typeof(Landscape))
            {
                Landscape land = (Landscape)c;
                return land.Shape;
            }
        
            throw new Exception("This Trait is not a Landscape and thus doesnt have a shape");
        }

        private string[] SplitTrait(string s) { return s.Split(','); }

        public abstract void playCard(); //To be defined MUCH later

        public abstract void useEffect(); //To be defined MUCH later
        
}

