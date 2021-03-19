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
    private IPlayable owner;
    private TurnPhase phase;
    private bool isPhaseComplete;
    private bool isActiveTurn;
    private bool isStartDone;
    private static bool isDictionaryPrepared = false;
    public delegate void TurnPhaseFunction(GameEventsArgs e);
    TurnPhaseFunction turnPhaseFunction;
    private static Dictionary<TurnPhase, TurnPhaseFunction> phaseDict = new Dictionary<TurnPhase, TurnPhaseFunction>();
    private GameTimeManager timeManager;
    private GameEventsManager eventsManager;
    private PlayerInteraction playerAction;


    public Turn(IPlayable p, GameTimeManager timeManager)
    {
        if(!isDictionaryPrepared)
        {
            PrepareDictionary();
        }
      
        this.owner = p;
        this.phase = TurnPhase.Start;
        this.isPhaseComplete = false;
        this.isStartDone = false;
        this.timeManager = timeManager;
        this.playerAction = owner.GetInteraction();
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
            GameEventsArgs tturnPhaseEvent = Utilities.HelperFunctions.RaiseNewEvent(this, (Player)this.owner, (Player)this.owner, NonMoveAction.TurnPhase);
            timeManager.AdvanceGameTime();
            this.PerformPhaseAction(tturnPhaseEvent);
            Debug.Log(owner.GetType() + "'s turn phase: " + phase);
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
        phaseDict.Add(TurnPhase.Start, this.StartGamePhase);
        phaseDict.Add(TurnPhase.Gather, this.StartGatherPhase);
        phaseDict.Add(TurnPhase.Awaken, this.StartAwakenPhase);
        phaseDict.Add(TurnPhase.Central, this.StartCentralPhase);
        phaseDict.Add(TurnPhase.Crystallize, this.StartCrystalPhase);
        phaseDict.Add(TurnPhase.End, this.StartEndPhase);
        isDictionaryPrepared = true;
    }

    private void StartGamePhase(GameEventsArgs e)
    {
        Debug.Log("Value of Start Bool:" + isStartDone);
        if (!isStartDone)
        {
            //HANDLED BY PLAYERINTERACTION this.owner.PlayerDeck.Draw(5);
            Debug.Log("5 Cards shouldve been added to the hand");
            this.isStartDone = true;
        }
    }

    private void StartGatherPhase(GameEventsArgs e)
    {


        Debug.Log("Its the beginning of a duel");
        /*
        if (e.EventOwner.PlayerDeck.Count == 0)
        {
            Debug.Log("You Lose");
            HelperFunctions.RaiseNewEvent(this, (Player)this.owner, (Player)this.owner, NonMoveAction.GameEnd);

        }
        e.EventOwner.PlayerDeck.Draw();
        */
    }

    private void StartAwakenPhase(GameEventsArgs e)
    {
        Debug.Log("Awaken Phase");
        //playerAction.AwakenPhaseStart();
    }

    private void StartCentralPhase(GameEventsArgs e)
    {
        Debug.Log("Central Phase");         
        //playerAction.CentralPhaseStart();  
    }

    private void StartCrystalPhase(GameEventsArgs e)
    {
        
        Debug.Log("Crystal Phase");
        //playerAction.CrystalPhaseStart();
    }

    private void StartEndPhase(GameEventsArgs e)
    {
        Debug.Log("End Phase");          
        //playerAction.EndPhaseStart();
    }

}

