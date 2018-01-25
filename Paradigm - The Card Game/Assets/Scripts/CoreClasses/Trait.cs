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
    private bool isEnabled;

    public delegate void ActivateTrait(GameEvents e);
    ActivateTrait traitFunction;
    

    public Trait(string t, string cN)
    {
        this.text = t;
        this.cardName = cN;
        this.isEnabled = true;
        
    }

}

