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

    public override void PlayCard()
    {
        throw new NotImplementedException();
    }

    public override void UseEffect()
    {

    }
}

