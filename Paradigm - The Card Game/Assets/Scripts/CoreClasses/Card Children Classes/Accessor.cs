﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Accessor: Card
{
        private int hp;
        private int maxHp;
        private int power;
        private int numOfAttacks = 1;
        private bool elemental = false;

        public Accessor()
        {

        }

        public Accessor(string n, string k, System.Int64 p, System.Int64 h, string t, string a, string a2, string a3)
        {
            this.setName(n);
            this.SetAbilities(a,a2,a3);
            this.SetTraits(t);
            this.SetPower((int)p);
            this.SetMaxHp((int)h);
            this.SetHp((int)h);
            Family fam = new Family(k);
            this.setFam(fam);
            
        }

        public int Power
        {
            get { return this.power; }
            set { this.power = value; }
        }

        public int HP
        {
            get { return this.hp; }
            set { this.hp = value; }
        }

        public void SetPower(int p) { power = p; }
        public void SetMaxHp(int h) { maxHp = h; }
        public void SetHp(int h) { hp = h; }
        public void AddAttacks(int a) { numOfAttacks = numOfAttacks + a; }
        public void ReduceAttacks(int a)
        {
            numOfAttacks = numOfAttacks - a;
            if (numOfAttacks < 0)
            {
                numOfAttacks = 0;
            }

        }

        public int getPower() { return power; }
        public int getHp() { return hp; }
        public bool getElementStatus() { return elemental; }


        public override void playCard()
        {
            Player p = this.getOwner();
            this.getLocation().MoveContent(this, p.GetLocation("Field"));
        }

        public override void useEffect()
        {
            throw new NotImplementedException();
        }

        public void DeclareAttack(Card t)
        {
            //DISPLAY THAT AN ATTACK IS ABOUT TO HAPPEN AND
            //PRESENT PROMPT FOR BLOCKING
            /*Accessor blocker; // = return from notification and prompt function
            if (blocker == null)
            {
                Attack(t);
            }
            else
            {
                Attack(blocker);
            }*/
        }

        public void Attack(Card t)
        {
            if(t is Accessor)
            {
                Accessor a = (Accessor)t;
                if (!t.getBarrierStatus())
                {
                    a.SetHp(a.getHp() - this.getPower());
                    return;
                }
            }
            else
            {
                Player p = t.getOwner();
                p.DestroyBarrier();
            }
        }
}

