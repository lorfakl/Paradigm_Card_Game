using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Mechanism: Card
{
    private Accessor bonded;
    private int power;

    public Accessor getBondedAccessor(){ return bonded; }
    public void setBondedAccessor(Accessor a) { bonded = a; }
    
    public void setPower(int p) { power = p; }
    public int getPower() { return power; }

    public Mechanism(string n, string[] e, string[] t, int p, bool s, string k)
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
        p.addToField(this);
    }

    public override void useEffect()
    {

    }
}

