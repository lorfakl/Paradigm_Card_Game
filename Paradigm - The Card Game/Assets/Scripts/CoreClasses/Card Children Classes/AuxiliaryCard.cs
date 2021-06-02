using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AuxiliaryCard: Card
{

    public AuxiliaryCard(string n, string k, string t, string a, string a2 = "", string a3 = "")
    {
        

        if ((t != "") && (a == ""))
        {
            string temp = t; 
            t = a;
            a = temp;  
        }
        
       

        this.setName(n);
        //this.SetAbilities(a, a2, a3); 
        SetTraits(t);
        Family fam = new Family(k);
        this.setFam(fam);
    }
}

