﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;
using AI;

public enum ValidLocations { Hand, Grave, LockZ, BZ, LandZ, SC, PZ, DZ, Field, Deck}

[RequireComponent(typeof(PlayerInteraction))]
public abstract class Player:IPlayable
{

    //private Turn playerTurn;
    
        
    private Dictionary<string, Location> cardLocations = new Dictionary<string, Location>();
    private static string[] validLocations = { "Hand", "Grave", "LockZ", "BZ", "LandZ", "SC", "PZ", "DZ", "Field", "Deck" };
    private Deck playerDeck;
    private int playerID;
 
    private Majesty majesty;
    protected Landscape tcCard;
    private List<Landscape> lands;
    private Turn turn;

    protected string type;
    
    private bool isPreparedToStart;
    private Location returnedLocation;
    public static readonly int timerTime = 45;
    protected int timeLeftOnTimer = timerTime;
    protected PlayerInteraction gamePlayHook;

    public Player(GameTimeManager mgmt, int addTo = 0)
    {
        this.playerID = UnityEngine.Random.Range(0,256);
        if (addTo != 0)
        {
            this.playerID = this.playerID + UnityEngine.Random.Range(510, 2048);
        }

        this.turn = new Turn(this, mgmt);

        foreach (string s in validLocations)
        {
            this.cardLocations.Add(s, new Location(s, this));
        }
        this.playerDeck = new Deck("Deck", this);
        this.cardLocations["Deck"] = this.playerDeck;
        this.majesty = playerDeck.GetMajesty();

        this.isPreparedToStart = false;    
    }

    public Deck PlayerDeck
    {
        get { return playerDeck; }
        set { playerDeck = value; }
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

    public string PlayerName
    {
        get { return type; }
    }

    public Turn PlayerTurn
    {
        get { return this.turn; }
        set { this.turn = value; }
    }

    public Location ReturnedLocation
    {
        get { return this.returnedLocation; }
        set { this.returnedLocation = value; }
    }
        
    public string Type
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

    public PlayerInteraction GamePlayHook
    {
        get { return gamePlayHook; }
        set { gamePlayHook = value; }
    }

    public void LoadDeckFromDataBase()
    {
        playerDeck.MoveContent(DataBase.CardDataBase.LoadPlayerDeck(), playerDeck);
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
            Debug.Log("Invalid Argument!");
            return null;
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

    protected PlayerInteraction FindPlayerInteraction(string tag)
    {
        GameObject go = GameObject.FindGameObjectWithTag(tag);
        if (go != null)
        {
            PlayerInteraction gph = go.GetComponent<PlayerInteraction>();
            if(gph != null)
            {
                Debug.Log("Not null");
                return gph;
            }
        }
        return null;
    }

    public abstract PlayerInteraction GetInteraction();
    public abstract IEnumerator ChooseTerritoryChallengeCard(Location t);
    public abstract IEnumerator ChooseBarriers(int barrierCount);
    public abstract IEnumerator PerformGather();
    public abstract IEnumerator PerformAwaken();
    public abstract IEnumerator PerformCentral();
    public abstract IEnumerator PerformCrystal();
    public abstract IEnumerator PerformEnd();
    public abstract void PlayCard();
    public abstract bool GetPlayerUIStatus();
}

