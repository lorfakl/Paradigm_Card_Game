using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
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
    private static bool isDictionaryPrepared = false;
    public delegate void TurnPhaseFunction(GameEventsArgs e);
    TurnPhaseFunction turnPhaseFunction;
    private static Dictionary<TurnPhase, TurnPhaseFunction> phaseDict = new Dictionary<TurnPhase, TurnPhaseFunction>();

    public Turn(Player p)
    {
        if(!isDictionaryPrepared)
        {
            PrepareDictionary();
        }
      
        this.owner = p;
        this.phase = TurnPhase.Start;
        this.isPhaseComplete = false;
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

    public void StartTurn()
    {
        this.MoveToNextPhase();
    }

    private void MoveToNextPhase()
    {
        SetDelegate();
        GameEventsArgs turnPhaseEvent = Utilities.HelperFunctions.RaiseNewEvent(this, this.owner, this.owner);
        Debug.Log(this.phase.ToString());
        this.PerformPhaseAction(turnPhaseEvent);

        TurnPhase currentPhase = this.phase; 
        if(currentPhase == TurnPhase.End)//if the End phase was the last turn phase
        {

            this.phase = TurnPhase.Gather; //set the turn phase to Gather for the next turn
            return; //exit this function
            
        }
        else
        { 
            currentPhase++;
            TurnPhase nextPhase = currentPhase;//(TurnPhase)currentPhase;
            this.phase = nextPhase;
        }
               
        this.MoveToNextPhase(); //a recursive call to move through all the phases
    }

    private void PerformPhaseAction(GameEventsArgs e)
    {
        this.turnPhaseFunction(e);
    }

    private void SetDelegate()
    {
        if (!isDictionaryPrepared)
        {
            PrepareDictionary();
        }

        this.turnPhaseFunction = phaseDict[this.phase];
        
    }

    private static void PrepareDictionary()
    {
        phaseDict.Add(TurnPhase.Start, StartGamePhase);
        phaseDict.Add(TurnPhase.Gather, StartGatherPhase);
        phaseDict.Add(TurnPhase.Awaken, StartAwakenPhase);
        phaseDict.Add(TurnPhase.Central, StartCentralPhase);
        phaseDict.Add(TurnPhase.Crystallize, StartCrystalPhase);
        phaseDict.Add(TurnPhase.End, StartEndPhase);
        isDictionaryPrepared = true;
    }

    private static void StartGamePhase(GameEventsArgs e)
    {
        Debug.Log("Game Start! Start Territory Challenge!");
    }

    private static void StartGatherPhase(GameEventsArgs e)
    {
        Debug.Log("Gather Phase: Draw Card and Abilities check the event queue");
    }

    private static void StartAwakenPhase(GameEventsArgs e)
    {
        Debug.Log("Awaken Phase: Choose your philosophers");
    }

    private static void StartCentralPhase(GameEventsArgs e)
    {
        Debug.Log("Central Phase: Play cards and shit");
    }

    private static void StartCrystalPhase(GameEventsArgs e)
    {
        Debug.Log("Crystallize Phase: collect your shards are whatever, still dont get why there's like 4 different words that describe the same thing");
    }

    private static void StartEndPhase(GameEventsArgs e)
    {
        Debug.Log("End Phase: End of your turn fuck face! Abilities that want to will activate here");
    }


}

