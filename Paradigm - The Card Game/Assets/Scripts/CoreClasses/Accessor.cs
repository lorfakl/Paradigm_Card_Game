using System;
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

        public void setPower(int p) { power = p; }
        public void setMaxHp(int h) { maxHp = h; }
        public void setHp(int h) { hp = h; }
        public void addAttacks(int a) { numOfAttacks = numOfAttacks + a; }
        public void reduceAttacks(int a)
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

        public Accessor()
        {

        }

        public Accessor(string n, string[] e, string[] t, int p, int h, bool s, string k)
        {
            this.setName(n);
            this.setEffect(e);
            this.setTraits(t);
            this.setPower(p);
            this.setMaxHp(h);
            this.setHp(h);
            this.setShard(s);
            Family fam = new Family(k);
            this.setFam(fam);
            for(int i = 0; i < t.Length; i++)
            {
                if(t[i] == "Elemental")
                {
                    elemental = true;
                }
            }
        }

        public override void playCard()
        {
            Player p = this.getOwner();
            p.addToField(this);
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
                    a.setHp(a.getHp() - this.getPower());
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

