using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AuxiliaryCard: Card
{
    private string shape;

    public string getShape() { return shape; }
    /*
    public AuxiliaryCard(string n, string k, string t, int p, string a, string a2 = "", string a3 = "")
    {
        this.setName(n);
        //this.setAbilities(n, a); 
        foreach (string tr in SplitTrait(t)) { this.setTraits(tr, n); }
        Family fam = new Family(k);
        this.setFam(fam);
    }

    public AuxiliaryCard(string n, string k, int p, string a, string a2 = "", string a3 = "")
    {
        this.setName(n);
        this.setAbilities(n, a); 
        Family fam = new Family(k);
        this.setFam(fam);
    }*/

    public override void playCard()
    {

    }

    public override void useEffect()
    {

    }
}

