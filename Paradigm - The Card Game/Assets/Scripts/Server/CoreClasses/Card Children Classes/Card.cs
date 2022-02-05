using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;

public abstract class Card
{
    //TODO TRAITS NEED TO BE THOUGHT OUT
    private string name;
    private string classType;
    private int id;
    private List<Ability> abilities = new List<Ability>();
    private List<Trait> traits = new List<Trait>();
    private Family fam;
    private Player owner;
    private bool isShard;
    protected bool canSpawn;
    private bool inPlay;
    private bool isBarrier;
    private bool isDestroyed;
    private bool isValid;
    private bool isActive;
    protected GameObject g;
    protected Location currentLocation;


    //Properties
    public int ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public Guid InstanceID
    {
        get;
        private set;
    }

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public List<Ability> Abilities
    {
        get { return this.abilities; }
        private set { this.abilities = value; }
    }

    public List<Trait> Traits
    {
        get { return this.traits; }
        private set { this.traits = value; }
    }

    public bool Shard
    {
        get { return this.isShard; }
        set { this.isShard = value; }
    }

    public Player Owner
    {
        get { return this.owner; }
        set { this.owner = value; }
    }

    public string ClassType
    {
        get { return this.classType; }
        set { this.classType = value; }
    }

    public bool CanSpawn
    {
        get { return canSpawn; }
    }

    public ValidLocations CurrentLocation
    {
        get { return (ValidLocations)Location.ConvertFromLocation(currentLocation); }
    }

    public bool IsValid
    {
        get { return this.isValid; }
    }

    public bool IsActive
    {
        get { return this.isActive; }
        set { this.isActive = value; }
    }
        
    public List<string> CustomData
    {
        get;
        set;
    }

    public string Family
    {
        get { return this.fam.Name; }
    }

    public object this[string propertyName]
    {
        get
        {
            // probably faster without reflection:
            // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
            // instead of the following
            Type myType = this.GetType();
            System.Reflection.PropertyInfo myPropInfo = myType.GetProperty(propertyName);
            return myPropInfo?.GetValue(this, null);
        }
        set
        {
            Type myType = this.GetType();
            System.Reflection.PropertyInfo myPropInfo = myType.GetProperty(propertyName);
            myPropInfo.SetValue(this, value, null);
        }
    }
    /// <summary>
    /// The card constructor creates the first abl for all Cards. While the name is OnSpawn
    /// What this does it sets the card to active. Which for accessor cards is the spawning.
    /// But for non unit cards it just sets them to active and doesn't "spawn" them for rule
    /// purposes. The reason for this was to enable card abls to block cards from hitting the 
    /// board altogether. Instead of blocking a specific abl of cards.
    /// 
    /// Without this, cards would just hit the field and be dealt with when they abls were used.
    /// This adds the layer of strategy, allowing for cards that prevent a card from hitting the 
    /// field at all and cards that prevent abls on cards from activating
    /// </summary>
    public Card()
    {
       
        this.Abilities.Add(new Ability(new Condition(), new Action()));
        this.Abilities[0].Name = "OnSpawn";
        this.Abilities[0].IsMandatory = true;
        this.Abilities[0].CardOwner = this;
        this.Abilities[0].Actions[0].Ability = this.Abilities[0];
        this.Abilities[0].Actions[0].Card = this;
        this.Abilities[0].Actions[0].EventType = "NonMove";
        this.Abilities[0].Actions[0].EventName = "Active";
        this.Abilities[0].Actions[0].Target = "This";
        this.Abilities[0].Conditions[0].EventType = "Move";
        this.Abilities[0].Conditions[0].EventName = "Spawn";
        this.Abilities[0].Conditions[0].Target = "This";
        this.Abilities[0].Conditions[0].SearchCriteria = null;
        this.Abilities[0].Conditions[0].Ability = this.Abilities[0];
        this.Abilities[0].Conditions[0].Card = this;
    }

   

    /// <summary>
    /// Provides a link between the Card class instance and the physical GameObject using it's data
    /// </summary>
    public GameObject GameObj
    {
        get { return g; }
        set { g = value; }
    }

    public string[] SetAbilityText { get; private set; }

    //Getters
    public string getName() { return name; }
    public List<Ability> getAbilities() { return abilities; }
    public List<Trait> getTraits() { return traits; }
    public Player getOwner() { return owner; }
    public Family getFam() { return fam; }
    public Location getLocation() { return currentLocation; }
    

    //Setters
    protected void setName(string n) { name = n; }
  
    private void addTrait(Trait t) { traits.Add(t); }
    public void setShard(bool sh) { isShard = sh; }
    public void setOwner(Player p) { owner = p; }
    protected void setFam(Family f) { fam = f; }
    public void setLocation(Location l) { currentLocation = l; }
    public void setBarrierStatus(bool b) { isBarrier = b; }
   
    public void setDestoyedStatus(bool s) { isDestroyed = s; }

    public void SetValidity(bool s)
    {
        this.isValid = s;
    }

    public void SetTraits(string text)
    {
        foreach (string tr in SplitTrait(text)) { this.addTrait(new Trait(tr, this.name)); }
    }

    public void GenerateInstanceID()
    {
        this.InstanceID = Guid.NewGuid();
    }

    public string GetAbilityText()
    {
        string abText = "";

        foreach(Ability a in abilities)
        {
            abText = abText + a.AbilityText + System.Environment.NewLine;
        }

        return abText;
    }

    public string GetTraitText()
    {
        string abText = "";

        foreach (Trait a in traits)
        {
            abText = abText + a.TraitText + System.Environment.NewLine;
        }

        return abText;
    }

    public static ShapeTrait GetShape(Card c)
    {
        //Debug.Log(c.name + " is a " + c.GetType().ToString());
        if(c == null)
        {
            throw new Exception("This card is null");
        }
        if (c.GetType() == typeof(Landscape))
        {
            Landscape land = (Landscape)c;
            return land.Shape;
        }
        
        throw new Exception("This Card is not a Landscape and thus doesnt have a shape");
    }

    private string[] SplitTrait(string s) { return s.Split(','); }

    public int MoveToGameStartLocation()
    {
        int cardsMoved = 0;
        string[] typesMoved = {"Source", "Philosopher", "Majesty", "Landscape"};
        for(int i = 0; i<typesMoved.Length; i++)
        {
            if(this.GetType().ToString() == typesMoved[i])
            {
                //Debug.Log("Is this right? " + this.Name + " is of type " + this.GetType().ToString());
                this.getLocation().MoveContent(this, this.getOwner().GetLocation(ValidLocations.DZ));
                cardsMoved++;
            }
        }
        return cardsMoved;
    }
    
    public void PlayCard()
    {
        if(CurrentLocation == ValidLocations.Hand)
        {
            HelperFunctions.RaiseNewEvent(this, this, ValidLocations.Hand, ValidLocations.Field, MoveAction.Spawn, (Card)this);
            HelperFunctions.RaiseNewUIEvent(this, ValidLocations.Hand, ValidLocations.Field, MoveAction.Spawn, (Card)this);
        }
    }

    public void UseEffect()
    {
        HelperFunctions.RaiseNewEvent(this, new GameAction(MoveAction.None,NonMoveAction.Initiate), EventType.LegalCheck, this);
    }
//To be defined MUCH later
        
}

