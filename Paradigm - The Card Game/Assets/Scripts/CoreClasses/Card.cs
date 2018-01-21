using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


 public abstract class Card
{
        //TODO TRAITS NEED TO BE THOUGHT OUT
        private string name;
        private List<Ability> abilities = new List<Ability>();
        private List<Trait> traits = new List<Trait>();
        private Family fam;
        private Player owner;
        private bool isShard;
        private bool inPlay;
        private bool isBarrier;
        private bool isDestroyed;
        private string currentLocation;

        //Getters
        public string getName() { return name; }
        public List<Ability> getAbilities() { return abilities; }
        public List<Trait> getTraits() { return traits; }
        public bool getShard() { return isShard; }
        public Player getOwner() { return owner; }
        public Family getFam() { return fam; }
        public string getLocation() { return currentLocation; }
        public bool getBarrierStatus() { return isBarrier; }
        public bool getPlayStatus() { return inPlay; }
        

        //Setters
        public void setName(string n) { name = n; }
        public void addAbility(Ability a) { abilities.Add(a); }
        public void addTrait(Trait t) { traits.Add(t); }
        public void setShard(bool sh) { isShard = sh; }
        public void setOwner(Player p) { owner = p; }
        public void setFam(Family f) { fam = f; }
        public void setLocation(string l) { currentLocation = l; }
        public void setBarrierStatus(bool b) { isBarrier = b; }
        public void setPlayStatus(bool b) { inPlay = b; }
        public void setDestoyedStatus(bool s) { isDestroyed = s; }

        public void setAbilities(string cName, string text, string aName = "")
        {
            addAbility(new Ability(cName, text));
        }

        public void setTraits(string text, string cName)
        {
            addTrait(new Trait(text, cName));
        }

        public void ConstructTraits(string[] t, string n)
        {
            foreach (string tr in t) { this.setTraits(tr, n); }
        }

        protected string[] SplitTrait(string s) { return s.Split(','); }

    public abstract void playCard(); //To be defined MUCH later

        public abstract void useEffect(); //To be defined MUCH later
        
}

