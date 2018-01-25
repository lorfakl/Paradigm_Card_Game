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

    public Mechanism(string n, string k, System.Int64 p, string t, string a, string a2, string a3 )
    {
        this.setName(n);
        this.SetAbilities(a, a2, a2);
        this.SetTraits(t);
        this.setPower((int)p);
        Family fam = new Family(k);
        this.setFam(fam);
    }

    public override void playCard()
    {
        Player p = this.getOwner();
        
    }

    public override void useEffect()
    {

    }
}

