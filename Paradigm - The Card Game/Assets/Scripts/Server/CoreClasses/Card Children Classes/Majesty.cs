﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


 public class Majesty: Accessor
 {
    private bool secrets = false;
    private bool dormant = true;
    public bool GetSecretStatus(){ return secrets; }
    public void SetSecretStatus(bool s) { secrets = s; }

    public Majesty()
    {
        this.SetValidity(false);
    }

    public Majesty(string n, string k,  System.Int64 p, System.Int64 h, string t, string a, string a2, string a3)
    {
        this.setName(n);
        //this.SetAbilities(a, a2, a3);
        this.SetTraits(t);
        this.SetPower((int)p);
        this.SetMaxHp((int)h);
        this.SetHp((int)h);
        Family fam = new Family(k);
        this.setFam(fam);
        this.SetValidity(true);
    }


 
}
