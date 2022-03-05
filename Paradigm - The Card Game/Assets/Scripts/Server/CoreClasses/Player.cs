using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;
using AI;

public enum ValidLocations { Hand, Grave, LockZ, BZ, LandZ, SC, PZ, DZ, Field, Deck}

public enum PlayerType { Human, AI, MainHuman}

public abstract class Player: IPlayable
{

    //private Turn playerTurn;
    
        
    private Dictionary<string, Location> cardLocations = new Dictionary<string, Location>();
    private static string[] validLocations = { "Hand", "Grave", "LockZ", "BZ", "LandZ", "SC", "PZ", "DZ", "Field", "Deck" };
    private Deck playerDeck;
    private int playerID;
 
    private Majesty majesty;
    protected Landscape tcCard;
    private List<Landscape> lands;


    protected PlayerType type;
    
    private bool isPreparedToStart;
    private Location returnedLocation;
    public static readonly int timerTime = 45;
    protected int timeLeftOnTimer = timerTime;
    protected int centralActions = 3;

    public Player(GameTimeManager mgmt, int addTo = 0)
    {
        this.playerID = UnityEngine.Random.Range(0,256);
        if (addTo != 0)
        {
            this.playerID = this.playerID + UnityEngine.Random.Range(510, 2048);
        }

        foreach (string s in validLocations)
        {
            this.cardLocations.Add(s, new Location(s, this));
        }
        this.playerDeck = new Deck("Deck", this);
        this.cardLocations["Deck"] = this.playerDeck;
        this.majesty = playerDeck.GetMajesty();

        this.isPreparedToStart = false;    
    }

    public Player(Guid id)
    {
        this.playerID = UnityEngine.Random.Range(0, 256);

        this.ID = id.ToString();

        foreach (string s in validLocations)
        {
            Location l = new Location(s, this);
            this.cardLocations.Add(s, l);
        }
        this.playerDeck = new Deck("Deck", this);
        Debug.Log("Player GUID contructor created deck");
        this.cardLocations["Deck"] = this.playerDeck;
        this.majesty = playerDeck.GetMajesty();

        this.isPreparedToStart = false;
    }

    public Player(string id)
    {
        this.ID = id;

        foreach (string s in validLocations)
        {
            Location l = new Location(s, this);
            this.cardLocations.Add(s, l);
        }
        this.playerDeck = new Deck("Deck", this);
        Debug.Log("Player GUID contructor created deck");
        this.cardLocations["Deck"] = this.playerDeck;
        this.majesty = playerDeck.GetMajesty();

        this.isPreparedToStart = false;
    }

    public Deck PlayerDeck
    {
        get { return playerDeck; }
        set { playerDeck = value; }
    }

    public Location Hand
    {
        get { return GetLocation(ValidLocations.Hand); }
    }

    public Location Grave
    {
        get { return GetLocation(ValidLocations.Grave); }
    }
    public Location ShardCollection
    {
        get { return GetLocation(ValidLocations.SC); }
    }
    public Location BZ
    {
        get { return GetLocation(ValidLocations.BZ); }
    }

    public Location Field
    {
        get { return GetLocation(ValidLocations.Field); }
    }
  
    public int CentralActions
    {
        get { return centralActions; }
        set { centralActions = value; }
    }

    public Majesty Majesty
    {
        get { return majesty; }
        set { majesty = value; }
    }

    public int PlayerID
    {
        get { return playerID; }
    }

    public string ID { get; protected set; }

    public PlayerType PlayerName
    {
        get { return type; }
    }

    public Location ReturnedLocation
    {
        get { return this.returnedLocation; }
        set { this.returnedLocation = value; }
    }
        
    public PlayerType Type
    {
        get { return this.type; }
    }
        
    public Card TCCard
    {
        get { return this.tcCard; }
        set { this.tcCard = (Landscape)value; }
    }
   
    public bool IsPreparedToStart
    {
        get { return isPreparedToStart; }
        set { isPreparedToStart = value; }
    }

    public int TimeLeftOnTimer
    {
        get { return this.timeLeftOnTimer; }
        set { this.timeLeftOnTimer = value; }
    }

    public Location GetLocation(ValidLocations l)
    {
        return GetLocation(l.ToString());
    }

    private Location GetLocation(string l)
    {
        bool validVal = false;
        foreach (string s in validLocations)
        {
            if(s.Contains(l))
            {
                validVal = true;
            }
        }

        if(!validVal)
        {
            throw new Exception("Invalid Location Argument!: " + l);
        }

        return cardLocations[l];
    }

    public int GetLocationCount(string name)
    {
        return this.GetLocation(name).Count;
    }

    public void AddToField(Card c)
    {
    this.cardLocations["Hand"].MoveContent(c, this.cardLocations["Field"]);   
    }
    
    public void AddBarrier(Card c)
    {
        c.setShard(true);
        c.setBarrierStatus(true);
        c.getLocation().MoveContent(c, this.cardLocations["BZ"]);
    }

    public void AddToHand(Card c)
    {
        c.getLocation().MoveContent(c, this.cardLocations["Hand"]);
    }
    //End Adds


    //Housing Keeping functions
    public void DestroyBarrier()
    {
        Card c = this.cardLocations["BZ"].GetContents()[0];
        c.setBarrierStatus(false);
        c.getLocation().MoveContent(c, this.cardLocations["SC"]);
    }
    //End Housing Keeping functions

    //Card Transit
    public void SendToGrave(Card c)
    {
        c.setDestoyedStatus(true);
        c.getLocation().MoveContent(c, this.cardLocations["Grave"]);
    }

    public void SendToShardPile(Card c)
    {
        c.getLocation().MoveContent(c, this.cardLocations["SC"]);
    }

    //End Card Transit

    public void PlayCard(Card c)
    {
        c.PlayCard();
    }

    public abstract IEnumerator ChooseTerritoryChallengeCard(Location t);
    public abstract IEnumerator ChooseBarriers(int barrierCount);
    public abstract IEnumerator PerformGather();
    public abstract IEnumerator PerformAwaken();
    public abstract IEnumerator PerformCentral();
    public abstract IEnumerator PerformCrystal();
    public abstract IEnumerator PerformEnd();
    public abstract bool GetPlayerUIStatus();
    public abstract IEnumerator PerformAttack();
}

