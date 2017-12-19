using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class AuxiliaryCard: Card
{
    private string shape;

    public string getShape() { return shape; }

    public AuxiliaryCard(string n, string[] e, string[] t, bool s, string k)
    {
        this.setName(n);
        this.setEffect(e);
        this.setTraits(t);
        this.setShard(s);
        Family fam = new Family(k);
        this.setFam(fam);
    }

    public override void playCard()
    {

    }

    public override void useEffect()
    {

    }
}

