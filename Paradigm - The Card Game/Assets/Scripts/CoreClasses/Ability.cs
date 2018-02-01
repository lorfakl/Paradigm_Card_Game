using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum AbilityType { Optional, Mandatory};

public class Ability
{ 
    private string text;
    private string abilityName;
    private string cardName;
    private bool isPatient;
    private bool isLimited;
    private bool canActivate;
    private bool canCheckForEvents; //so abilities dont check for events from the deck
    private int timesUsed;
    private Player playerOwner;
    private Card cardOwner;
    public static int numOfAbilities = 0;
    private AbilityType type; //ability type dictates how the abilities is activated
    public delegate void ActivateAbility(GameEvents e);
    ActivateAbility abilityFunction; //this is an identifier, in the constructor this will be assigned to a function grabbed 
                                     //from the hashtable of ability functions the AbilityBuilder contains
     
    public bool ActivationStatus
    {
        get { return canActivate; }
        set { canActivate = value; }
    }

    public Card CardOwner
    {
        get { return this.cardOwner; }
        set { this.cardOwner = value; }
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

    public string AbilityName
    {
        get { return this.abilityName; }
        set { this.abilityName = value; }
    }

    public bool IsLimited
    {
        get { return this.isLimited; }
        set { this.isLimited = value; }
    }

    public bool IsPatient
    {
        get { return this.isPatient; }
        set { this.isPatient = value; }
    }

    public void LinkToCard(string n)
    {
        Card card = DataBase.CardDataBase.GetCard(n);
        this.CardOwner = card;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CheckNewEvent (object sender, GameEventsArgs e)
    {
        if(this.canCheckForEvents)
        {
            Debug.Log("I'm an ability, and I know a new game event was added to the queue");
        }
            
    }

    public Ability(string cName, string text, string aName = "") //contains an optional parameter for the ability name because not all abilities are named
    {
        GameEventsManager.NotifySubsOfEvent += CheckNewEvent;
        this.cardName = cName;
        this.text = text;
        this.canActivate = false;
        this.canCheckForEvents = false;
        numOfAbilities++;
    }

    
    

}

