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


        public Element(string n, string k, string t, int p, string a, string a2 = "", string a3 = "")
        {
            this.setName(n);
            this.setAbilities(n, a);
            foreach (string tr in SplitTrait(t)) { this.setTraits(tr, n); }
            this.setPower(p);
            Family fam = new Family(k);
            this.setFam(fam);
        }

        public Element(string n, string k, int p, string a, string a2 = "", string a3 = "")
        {
            this.setName(n);
            this.setAbilities(n, a); 
            this.setPower(p);
            Family fam = new Family(k);
            this.setFam(fam);
        }

        

        public override void playCard()
        {
            Player p = this.getOwner();
            for(int i = 0; i < p.GetLocation("Field").Count; i++)
            {
                if(p.GetLocation("Field").GetContents()[i] is Accessor)
                {
                    Accessor a = (Accessor)p.GetLocation("Field").GetContents()[i];
                    if (a.getElementStatus())
                    {
                        p.GetLocation("Field").AddContent(this);
                    }
                }
            }
        }

        public override void useEffect()
        {

        }
    }

