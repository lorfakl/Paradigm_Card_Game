using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum AbilityType { Optional, Mandatory};

public class Ability
{ 
    private string text;
    private string cardName;
    private string abilityName;
    private bool isPatient;
    private bool isLimited;
    private bool canActivate;
    private int timesUsed;
    private Player owner;
    private AbilityType type; //ability type dictates how the abilities is activated
    public delegate void ActivateAbility(GameEvents e);
    ActivateAbility abilityFunction; //this is an identifier, in the constructor this will be assigned to a function grabbed 
                                     //from the hashtable of ability functions the AbilityBuilder contains
     
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

    public string AbilityText
    {
        get { return this.text; }
    }

    public bool getLimitedStatus() { return isLimited; }
    public string getAbilityName() { return abilityName; }

    private void CheckNewEvent (object sender, GameEventsArgs e)
    {
        Debug.Log("I'm an ability, and I know a new game event was added to the queue");
    }

    public Ability(string cName, string text, string aName = "") //contains an optional parameter for the ability name because not all abilities are named
    {
        GameEventsManager.NotifySubsOfEvent += CheckNewEvent;
        this.text = text;
    }

    
    

}

