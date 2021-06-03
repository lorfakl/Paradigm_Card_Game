using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;

public enum AbilityType { Optional, Mandatory};

[Serializable]
public class Ability
{ 
    private string text;
    private string name;

    private int timesUsed;
    private bool isMandatory;
    private bool isPatient;
    private bool isLimited;
    private bool isConditionSatisfied;
    private bool canCheckForEvents; //so abilities dont check for events from the deck
    private bool canActivate;

    private Player playerOwner;

    private List<Condition> conditions;
    private List<Action> actions;
    private Card cardOwner;
    

    public delegate void ActivateAbility(GameEventsArgs e);
    //ActivateAbility abilityFunction; //this is an identifier, in the constructor this will be assigned to a function grabbed 
    //from the hashtable of ability functions the AbilityBuilder contains
    #region Properties

    public bool ActivationStatus
    {
        get { return canActivate; }
        set { canActivate = value; }
    }

    public bool CanCheckEvent
    {
        get { return canCheckForEvents; }
        set { canCheckForEvents = value; }
    }
    public Card CardOwner
    {
        get { return this.cardOwner; }
        set { this.cardOwner = value; }
    }

    public Player Owner
    {
        get { return this.cardOwner.Owner; }
        set { this.cardOwner.Owner = value; }
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

    public bool IsLimited
    {
        get { return this.isLimited; }
        set { this.isLimited = value; }
    }

    public bool IsMandatory
    {
        get { return this.isMandatory; }
        set { this.isMandatory = value; }
    }

    public bool IsPatient
    {
        get { return this.isPatient; }
        set { this.isPatient = value; }
    }

    public List<Action> Actions
    {
        get { return this.actions; }
    }

    public List<Condition> Conditions
    {
        get { return this.conditions; }
    }

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public string OwningCardName { get; set; }

    public GameEventsArgs ConditionMetEvent
    {
        get;
        private set;
    }
    #endregion

    public Ability(List<Condition> c, List<Action> a)
    {
        GameEventsManager.NotifySubsOfEvent += CheckNewEvent;
        conditions = c;
        actions = a;
        this.canActivate = false;
        this.canCheckForEvents = false;
    }

    public Ability(Condition c, Action action)
    {
        GameEventsManager.NotifySubsOfEvent += CheckNewEvent;
        conditions = new List<Condition>();
        actions = new List<Action>();
        conditions.Add(c);
        actions.Add(action);
        this.canActivate = false;
        this.canCheckForEvents = false;
    }

    public Ability(string cName, string text) 
    {
        GameEventsManager.NotifySubsOfEvent += CheckNewEvent;
        this.text = text;
        this.canActivate = false;
        this.canCheckForEvents = false;
        conditions = new List<Condition>();
        actions = new List<Action>();

    }

    public Ability(Ability copy)
    {
        GameEventsManager.NotifySubsOfEvent += CheckNewEvent;
        this.text = copy.AbilityText;

       
        if(copy.Conditions == null)
        {
            HelperFunctions.Print("This List itself is null for Conditions");
        }
        else if(copy.Conditions[0] == null)
        {
            HelperFunctions.Print("The Condition object inside the list is null");
        }

     
        if (copy.Actions == null)
        {
            HelperFunctions.Print("This List itself is null for Actions");
        }
        else if (copy.Actions[0] == null)
        {
            HelperFunctions.Print("The Condition object inside the list is null");
        }
        name = copy.Name;

        timesUsed = copy.timesUsed;

        isPatient = copy.IsPatient;
        isLimited = copy.IsLimited;
        isConditionSatisfied = false;
        canCheckForEvents = false; //so abilities dont check for events from the deck
        canActivate = false;

        playerOwner = copy.playerOwner;

        List<Condition> conds = new List<Condition>();
        foreach (var c in copy.Conditions)
        {
           Condition newC = new Condition(c);
            conds.Add(newC);
        }
        this.conditions = conds;
        //action = new Action(copy.Actions);

        List<Action> acts = new List<Action>();
        foreach (var c in copy.Actions)
        {
            Action newA = new Action(c);
            acts.Add(newA);
        }
        this.actions = acts;
    }

    public void LinkToCard(Card c)
    {
        if (c == null)
        {
            throw new Exception("Fuck you cards null in Ability LinktoCard");
        }

        this.CardOwner = c;
        Owner = c.Owner;

        foreach (var cond in Conditions)
        {
            cond.Card = c;
            cond.Ability = this;
            
            cond.Owner = c.Owner;
            //cond.Owner = Owner;
        }
        foreach(var act in Actions)
        {
            act.Card = c;
            act.Ability = this;
            act.Owner = c.Owner;
            //act.Owner = Owner;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CheckNewEvent(object sender, GameEventsArgs e)
    {
        if (this.canCheckForEvents)
        {
            if (!e.IsUIEvent)
            { 
                foreach (var cond in Conditions)
                {
                    if(cond.Ability == null || cond.Card == null)
                    {
                    cond.Ability = this;
                    //cond.Card = CardOwner;
                    }
                    
                    if (cond.CheckCondition(e)) //if one condition is false exit the function
                    {
                        ConditionMetEvent = e;
                        break;   
                    }
                    else
                    {
                        return;
                    }

                }

                ExecuteActions();

            }
        }
    }

    private void ExecuteActions()
    {
        HelperFunctions.Print("Condition met how manyt times is it called");
        TimesUsed++;
        GameEventsManager.AddToStack(Actions, ConditionMetEvent);
        HelperFunctions.Print("cONDITION MET! DOING THE ACTIONS ");
    }
}

