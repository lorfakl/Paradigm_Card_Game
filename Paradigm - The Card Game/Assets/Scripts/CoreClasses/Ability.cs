using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Ability
{
  
    private string text;
    private bool canActivate;
    private enum AbilityType {Optional, Mandatory, Patient }; //the ability type dictates how the ability will interact with the event stack
    private AbilityType type;
    private bool isLimited;
    private int timesUsed;
    private string name;
    private AbilityFunctionality abilityFunction;
    
    public bool ActivationStatus
    {
        get { return canActivate; }
        set { canActivate = value; }
    }

    public int TimesUsed
    {
        get { return timesUsed; }
        set { timesUsed = value; }
    }

    public bool getLimitedStatus() { return isLimited; }
    public string getAbilityName() { return name; }
    

}

