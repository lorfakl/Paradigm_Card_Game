using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class CardSO : ScriptableObject
{

    #region Private Fields
    [SerializeField] private new string name;
    [SerializeField] private int id;
    [SerializeField] private bool isShard;
    [SerializeField] private Player owner;
    [SerializeField] private List<Ability> abilities = new List<Ability>();
    [SerializeField] private List<Trait> traits = new List<Trait>();
    [SerializeField] private Family fam;
    [SerializeField] private bool inPlay;
    [SerializeField] private bool isBarrier;
    [SerializeField] private bool isDestroyed;
    [SerializeField] private bool isValid;
    [SerializeField] protected GameObject g;
    [SerializeField] protected Location currentLocation;
    [SerializeField] private string classType;
    [SerializeField] private Location location;
    [SerializeField] private Sprite cardArt;
    #endregion

    #region Properties

    //Properties
    public int ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public List<Ability> Abilities
    {
        get { return this.abilities; }
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

    public Location Location
    {
        get { return location; }
        set { location = value; }
    }

    public string ClassType
    {
        get { return this.classType; }
        set { this.classType = value; }
    }

    public bool InPlay
    {
        get { return inPlay; }
        set { inPlay = value; }
    }

    public bool IsBarrier
    {
        get { return isBarrier; }
        set { isBarrier = value; }
    }

    public bool IsDestroyed
    {
        get { return isDestroyed; }
        set { isDestroyed = value; }
    }

    public GameObject GameObj
    {
        get { return g; }
        set { g = value; }
    }

    public Family Family
    {
        get { return fam; }
        set { fam = value; }
    }

    public Sprite CardArt
    {
        set { cardArt = value; }
    }

    #endregion
    

    private void addAbility(Ability a) { abilities.Add(a); }
    private void addTrait(Trait t) { traits.Add(t); }
 



  

    public void SetValidity(bool s)
    {
        this.isValid = s;
    }

    public void SetTraits(string text)
    {
        foreach (string tr in SplitTrait(text)) { this.addTrait(new Trait(tr, this.name)); }
    }

    protected void SetAbilities(string a, string a2, string a3)
    {
        string[] abs = { a, a2, a3 };
        for (int i = 0; i < 3; i++)
        {
            if (abs[i] != "")
            {
                this.addAbility(new Ability(this.name, abs[i]));
            }
        }
    }

    public string GetAbilityText()
    {
        string abText = "";

        foreach (Ability a in abilities)
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
        string[] typesMoved = { "Phantom", "Source", "Philosopher", "Majesty", "Landscape" };
        for (int i = 0; i < typesMoved.Length; i++)
        {
            if (this.GetType().ToString() == typesMoved[i])
            {
                //Debug.Log("Is this right? " + this.Name + " is of type " + this.GetType().ToString());
                //this.getLocation().MoveContent(this, this.getOwner().GetLocation(ValidLocations.DZ));
                cardsMoved++;
            }
        }
        return cardsMoved;
    }

    public void PlayCard()
    {
        //HelperFunctions.RaiseNewUIEvent(this, ValidLocations.Hand, ValidLocations.Field, MoveAction.Spawn, (Card)this);
    }

    public void UseEffect()
    {
        //HelperFunctions.RaiseNewEvent(this, new GameAction(MoveAction.None, NonMoveAction.Initiate), EventType.LegalCheck, this);
    }
}
