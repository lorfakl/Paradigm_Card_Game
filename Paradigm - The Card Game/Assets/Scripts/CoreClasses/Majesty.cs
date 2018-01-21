using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


 public class Majesty: Accessor
 {
    private bool secrets = false;
    private bool dormant = true;
    public bool getSecretStatus(){ return secrets; }
    public void setSecretStatus(bool s) { secrets = s; }
        
    public Majesty(string n, string[] e, string[] t, int p, int h, bool s, string k)
    {
        this.setName(n);
        foreach(string a in e) { this.setAbilities(n, a); }
        foreach(string tr in t) { this.setTraits(tr, n); }
        this.setPower(p);
        this.setMaxHp(h);
        this.setHp(h);
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

