using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public enum TurnPhase
{
    Start, Gather, Awaken, Central, Crystallize, End
}

public class Turn
{
    private Player owner;
    private TurnPhase phase;
    private bool isPhaseComplete;
    private bool isActiveTurn;
    private bool isStartDone;
    private bool isDictionaryPrepared = false;
    public delegate void TurnPhaseFunction();
    TurnPhaseFunction turnPhaseFunction;
    private Dictionary<TurnPhase, TurnPhaseFunction> phaseDict = new Dictionary<TurnPhase, TurnPhaseFunction>();
    private EventManager eventManager;
    


    public Turn(Player p)
    {
        if (!isDictionaryPrepared)
        {
            PrepareDictionary();
        }

        this.owner = p;
        this.phase = TurnPhase.Start;
        this.isPhaseComplete = false;
        this.isStartDone = false;

        if (owner == null)
        {
            throw new Exception("Big ol prollem");
        }
        eventManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>();
        SetDelegate();
        Debug.Log("New Turn was just created at the " + this.phase.ToString() + " turnPhase which is before the game starts");
    }

    public TurnPhase Phase
    {
        get { return phase; }
        set { phase = value; }
    }

    public bool IsPhaseComplete
    {
        get { return this.isPhaseComplete; }
    }

    public bool ActiveTurn
    {
        get { return this.isActiveTurn; }
    }

    public Player Owner
    {
        get { return this.owner; }
        //set { owner = value; }
    }

    public void StartTurn()
    {
        this.isActiveTurn = true;
        this.phase = TurnPhase.Gather;
        
        if(eventManager == null)
        {
            throw new Exception("Event manager in turn class is null");
        }
        else
        {
            Debug.Log("not null" + eventManager);
        }
        this.MoveToNextPhase();
    }

    public void EndTurn()
    {
        this.phase = TurnPhase.End;
        this.isActiveTurn = false;
    }

    private void MoveToNextPhase()
    {
        foreach (TurnPhase t in Enum.GetValues(typeof(TurnPhase)))
        {
            this.phase = t;
            SetDelegate();
            //GameEventsArgs turnPhaseEvent = Utilities.HelperFunctions.RaiseNewEvent(this, (Player)this.owner, (Player)this.owner, NonMoveAction.TurnPhase);
            this.PerformPhaseAction(/*turnPhaseEvent*/);
            //Debug.Log(owner.GetType() + "'s turn phase: " + phase);
        }

    }

    private void PerformPhaseAction(/*GameEventsArgs e*/)
    {
        this.turnPhaseFunction();
    }

    /// <summary>
    /// Changes the delegate to point to the function that corressponds to
    /// the turn phase in the turn phase dictionary
    /// </summary>
    private void SetDelegate()
    {
        if (!isDictionaryPrepared)
        {
            PrepareDictionary();
        }

        this.turnPhaseFunction = phaseDict[this.phase];
        //Debug.Log("Current turn phase function: " + phaseDict[this.phase].ToString());

    }

    private void PrepareDictionary()
    {
        phaseDict.Add(TurnPhase.Start, this.StartGamePhase);
        phaseDict.Add(TurnPhase.Gather, this.StartGatherPhase);
        phaseDict.Add(TurnPhase.Awaken, this.StartAwakenPhase);
        phaseDict.Add(TurnPhase.Central, this.StartCentralPhase);
        phaseDict.Add(TurnPhase.Crystallize, this.StartCrystalPhase);
        phaseDict.Add(TurnPhase.End, this.StartEndPhase);
        isDictionaryPrepared = true;
    }

    private void StartGamePhase(/*GameEventsArgs e*/)
    {
        Debug.Log("Value of Start Bool:" + isStartDone);
        if (!isStartDone)
        {
            //HANDLED BY PLAYERINTERACTION this.owner.PlayerDeck.Draw(5);
            Debug.Log("5 Cards shouldve been added to the hand");
            this.isStartDone = true;
        }
    }

    private void StartGatherPhase(/*GameEventsArgs e*/)
    {
        Debug.Log("Its the beginning of a duel");
        if(owner == null)
        {
            throw new Exception("damn bro dont know how that happened");
        }
        if(eventManager == null)
        {
            throw new Exception("the value changed somehow");
        }
        //Owner.GamePlayHook.GatherPhaseAction();
        //GameObject.FindGameObjectWithTag(GetTag(owner.GetPlayerUIStatus())).GetComponent<PlayerInteraction>().GatherPhaseAction();
        eventManager.StartCoroutine(owner.PerformGather());
    }

    private void StartAwakenPhase(/*GameEventsArgs e*/)
    {
       eventManager.StartCoroutine(owner.PerformAwaken());
    }

    private void StartCentralPhase(/*GameEventsArgs e*/)
    {
        eventManager.StartCoroutine(owner.PerformCentral());
    }

    private void StartCrystalPhase(/*GameEventsArgs e*/)
    {
        Debug.Log(Owner.GetPlayerUIStatus());
        Debug.Log(Owner.PlayerID);
        eventManager.StartCoroutine(owner.PerformCrystal());
    }

    private void StartEndPhase(/*GameEventsArgs e*/)
    {
        eventManager.StartCoroutine(owner.PerformEnd());
    }

    private string GetTag(bool status)
    {
        if(status)
        {
            return "Player";
        }
        else
        {
            return "AiPlayer";
        }
    }
}