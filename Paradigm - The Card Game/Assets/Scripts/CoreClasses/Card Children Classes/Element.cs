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


        public Element(string n, string k, System.Int64 p, string t, string a, string a2, string a3)
        {
            if ((t != "") && (a == ""))
            {
                string temp = t;
                t = a;
                a = temp;
            }

            this.setName(n);
            this.SetAbilities(a, a2, a3);
            this.SetTraits(t); 
            this.setPower((int)p);
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
                        this.getLocation().MoveContent(this, p.GetLocation("Field"));
                    }
                }
            }
        }

        public override void useEffect()
        {

        }
    }

