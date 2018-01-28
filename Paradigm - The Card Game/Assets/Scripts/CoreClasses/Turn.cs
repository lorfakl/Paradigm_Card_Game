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
    public delegate void TurnPhaseFunction(GameEventsArgs e);
    TurnPhaseFunction turnPhaseFunction;
    private Dictionary<string, TurnPhase> phaseDict = new Dictionary<string, TurnPhase>();

    public Turn(Player p)
    {
        PrepareDictionary();
        this.owner = p;
        this.phase = TurnPhase.Start;
        this.isPhaseComplete = false;
        SetDelegate();
        
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

    public int MoveToNextPhase()
    {
        int currentPhase = (int)this.phase;
        if(currentPhase == (int)TurnPhase.End)
        {
            this.phase = TurnPhase.Gather;
        }
        else
        {
            currentPhase++;
            TurnPhase nextPhase = (TurnPhase)currentPhase;
            this.phase = nextPhase;
        }

        SetDelegate();
        return (int)this.phase;
    }

    public void PerformPhaseAction(GameEventsArgs e)
    {
        this.turnPhaseFunction(e);
    }

    private void SetDelegate()
    {
        if (Utilities.Dictionaries.Prepared)
        {
            this.turnPhaseFunction = Utilities.Dictionaries.turnDict[this.phase];
        }
        else
        {
            Utilities.Dictionaries.PrepareDictionaries();
            this.turnPhaseFunction = Utilities.Dictionaries.turnDict[this.phase];
        }
    }

    private void PrepareDictionary()
    {
        phaseDict.Add("Start", TurnPhase.Start);
        phaseDict.Add("Gather", TurnPhase.Gather);
        phaseDict.Add("Awaken", TurnPhase.Awaken);
        phaseDict.Add("Central", TurnPhase.Central);
        phaseDict.Add("Crystallize", TurnPhase.Crystallize);
        phaseDict.Add("End", TurnPhase.End);
    }
     
    
}

