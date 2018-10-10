using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private static bool isDictionaryPrepared = false;
    public delegate void TurnPhaseFunction(GameEventsArgs e);
    TurnPhaseFunction turnPhaseFunction;
    private static Dictionary<TurnPhase, TurnPhaseFunction> phaseDict = new Dictionary<TurnPhase, TurnPhaseFunction>();
    private GameTimeManager timeManager;
    private GameEventsManager eventsManager;


    public Turn(Player p, GameTimeManager timeManager)
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
            SetDelegate();
            //if((!isStartDone) || (t != TurnPhase.Start))
            //{
                GameEventsArgs tturnPhaseEvent = Utilities.HelperFunctions.RaiseNewEvent(this, this.owner, this.owner, NonMoveAction.TurnPhase);
                timeManager.AdvanceGameTime();
                this.PerformPhaseAction(tturnPhaseEvent);
                Debug.Log(owner.PlayerName + "'s turn phase: " + t.ToString());
            //}
            
            
            
        }
        /*
        timeManager.AdvanceGameTime();
        SetDelegate();
        GameEventsArgs turnPhaseEvent = Utilities.HelperFunctions.RaiseNewEvent(this, this.owner, this.owner, NonMoveAction.TurnPhase);
        //Debug.Log(this.phase.ToString());

        if (isStartDone && this.Phase == TurnPhase.Start)
        {
            Debug.Log("Start already ran");
            this.phase++;
            Debug.Log("New phase is " + this.phase);
        }
        this.PerformPhaseAction(turnPhaseEvent); //once this line finishes the turn phase is over

        TurnPhase currentPhase = this.phase; 
        
        if(currentPhase == TurnPhase.End)//if the End phase was the last turn phase to occur
        {
            Debug.Log("This turn is over should be ending");
            this.phase = TurnPhase.Gather; //set the turn phase to Gather for the next turn
            return; 
        }
        else
        { 
            currentPhase++;
            TurnPhase nextPhase = currentPhase;//(TurnPhase)currentPhase;
            this.phase = nextPhase;
        }*/

        
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
            this.owner.PlayerDeck.Draw(5);
            Debug.Log("5 Cards shouldve been added to the hand");
            this.isStartDone = true;
        }
    }

    private void StartGatherPhase(GameEventsArgs e)
    {
        Debug.Log("Gather Phase: Draw Card and Abilities check the event queue");

        if (e.EventOwner.PlayerDeck.Count == 0)
        {
            Debug.Log("You Lose");
            HelperFunctions.RaiseNewEvent(this, this.owner, this.owner, NonMoveAction.GameEnd);

        }
        e.EventOwner.PlayerDeck.Draw();
    }

    private void StartAwakenPhase(GameEventsArgs e)
    {
        Debug.Log("Awaken Phase");
    }

    private void StartCentralPhase(GameEventsArgs e)
    {
        Debug.Log("Central Phase");
        PlayerInteraction pi = GetEventsManager(e);
        MonoBehaviour mono = HelperFunctions.AccessMonoBehaviour();
        mono.StartCoroutine(pi.CentralPhaseAction());
        
    }

    private void StartCrystalPhase(GameEventsArgs e)
    {
        if (e.EventOwner.GetLocation(ValidLocations.SC).Count < 3)
        {
            Debug.Log("Skipping this Crystallize phase not enough resources");
        }
        else
        {
            Debug.Log("Crystal Phase");
            PlayerInteraction pi = GetEventsManager(e);
            pi.CrystalPhaseAction();
        }
    }

    private void StartEndPhase(GameEventsArgs e)
    {
        
        //Card abilities
    }

    private PlayerInteraction GetEventsManager(GameEventsArgs e)
    {
        GameEventsManager gm = GameObject.FindWithTag("GameManager").GetComponent<GameEventsManager>();
        return gm.GetPlayerInteraction(e.EventOwner.IsAI);
    }
}

