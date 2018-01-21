using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum TraitType
{

}

public class Trait
{
    private string text;
    private string cardName;
    

    public Trait(string t, string cN)
    {
        this.text = t;
        this.cardName = cN;
        
    }

}

