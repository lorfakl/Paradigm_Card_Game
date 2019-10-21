﻿using System;
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
    private IPlayable owner;
    private TurnPhase phase;
    private bool isPhaseComplete;
    private bool isActiveTurn;
    private bool isStartDone;
    private static bool isDictionaryPrepared = false;
    public delegate IEnumerator TurnPhaseFunction(GameEventsArgs e);
    TurnPhaseFunction turnPhaseFunction;
    private static Dictionary<TurnPhase, TurnPhaseFunction> phaseDict = new Dictionary<TurnPhase, TurnPhaseFunction>();
    private EventManager eventsManager;
    private PlayerInteraction playerAction = new PlayerInteraction();


    public Turn(IPlayable p)
    {
        if(!isDictionaryPrepared)
        {
            PrepareDictionary();
        }
      
        this.owner = p;
        this.phase = TurnPhase.Gather;
        this.isPhaseComplete = false;
        this.isStartDone = false;
       
        if(owner == null)
        {
            throw new Exception("Big ol prollem");
        }
        
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

    public void StartTurn()
    {
        this.isActiveTurn = true;
        this.phase = TurnPhase.Gather;
        if(playerAction == null)
        {
            PlayerInteraction pi = owner.GetInteraction();
            if(pi == null)
            {
                throw new Exception("PlayerInteraction is Turn.cs is null");
            }
            else
            {
                playerAction = pi;
            }
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
        foreach(TurnPhase t in Enum.GetValues(typeof(TurnPhase)))
        {
            this.phase = t;
            SetDelegate();
            GameEventsArgs turnPhaseEvent = Utilities.HelperFunctions.RaiseNewEvent(this, (Player)this.owner, (Player)this.owner, NonMoveAction.TurnPhase);
            this.PerformPhaseAction(turnPhaseEvent);
            //Debug.Log(owner.GetType() + "'s turn phase: " + phase);
        }
       
    }

    private void PerformPhaseAction(GameEventsArgs e)
    {
        this.turnPhaseFunction(e);
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
        Debug.Log("Current turn phase function: " + phaseDict[this.phase].ToString());
        
    }

    private void PrepareDictionary()
    {
        phaseDict.Add(TurnPhase.Gather, this.StartGatherPhase);
        phaseDict.Add(TurnPhase.Awaken, this.StartAwakenPhase);
        phaseDict.Add(TurnPhase.Central, this.StartCentralPhase);
        phaseDict.Add(TurnPhase.Crystallize, this.StartCrystalPhase);
        phaseDict.Add(TurnPhase.End, this.StartEndPhase);
        isDictionaryPrepared = true;
    }

    

    public IEnumerator StartGatherPhase(GameEventsArgs e)
    {


        Debug.Log("Its the beginning of a duel");
        
        /*if (e.EventOwner.PlayerDeck.Count == 0)
        {
            Debug.Log("You Lose");
            HelperFunctions.RaiseNewEvent(this, (Player)this.owner, (Player)this.owner, NonMoveAction.GameEnd);

        }*/
        //e.EventOwner.PlayerDeck.Draw();
        return playerAction.GatherPhaseStart();


    }

    private IEnumerator StartAwakenPhase(GameEventsArgs e)
    {
        StartCentralPhase(e);
        yield return playerAction.AwakenPhaseStart();
        StartCentralPhase(e);
    }

    private IEnumerator StartCentralPhase(GameEventsArgs e)
    {
        Debug.Log("Central Phase");         
        yield return playerAction.CentralPhaseStart();
        StartCrystalPhase(e);
    }

    private IEnumerator StartCrystalPhase(GameEventsArgs e)
    {
        
        Debug.Log("Crystal Phase");
        yield return playerAction.CrystalPhaseStart();
        StartEndPhase(e);
    }

    private IEnumerator StartEndPhase(GameEventsArgs e)
    {
        Debug.Log("End Phase");          
        return playerAction.EndPhaseStart();
    }

}

