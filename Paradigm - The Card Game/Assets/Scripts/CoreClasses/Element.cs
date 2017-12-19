using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Element: Card
    {
        private int power;
        public void setPower(int p) { power = p; }
        public int getPower() { return power; }


        public Element(string n, string[] e, string[] t, int p, bool s, string k)
        {
            this.setName(n);
            this.setEffect(e);
            this.setTraits(t);
            this.setPower(p);
            this.setShard(s);
            Family fam = new Family(k);
            this.setFam(fam);
        }

        public override void playCard()
        {
            Player p = this.getOwner();
            for(int i = 0; i < p.getPlayerField().Count; i++)
            {
                if(p.getPlayerField()[i] is Accessor)
                {
                    Accessor a = (Accessor)p.getPlayerField()[i];
                    if (a.getElementStatus())
                    {
                        p.addToField(this);
                    }
                }
            }
        }

        public override void useEffect()
        {

        }
    }

