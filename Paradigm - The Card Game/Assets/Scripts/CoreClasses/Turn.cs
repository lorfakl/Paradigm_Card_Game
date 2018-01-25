using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;


public class Turn
{
    public enum TurnPhase
    {
        Gather, Awaken, Central, Crystallize, End
    }

    private Player owner;
    private TurnPhase phase;
    public delegate void TurnPhaseFunction();
    TurnPhaseFunction turnPhaseFunction;
    private Dictionary<string, TurnPhase> phaseDict = new Dictionary<string, TurnPhase>();

    public Turn(Player p)
    {
        PrepareDictionary();
        this.owner = p;
        this.phase = TurnPhase.Gather;
        SetDelegate();
    }

    public TurnPhase Phase
    {
        get { return phase; }
        set { phase = value; }
    }

    public void PreformPhaseAction(GameEventsArgs e)
    {

    }

    private void SetDelegate()
    {
        if (Utilities.Dictionaries.Prepared)
        {
            turnPhaseFunction = Utilities.Dictionaries.turnDict[this.phase.ToString()];
        }
        else
        {
            Utilities.Dictionaries.PrepareDictionaries();
            turnPhaseFunction = Utilities.Dictionaries.turnDict[this.phase.ToString()];
        }
    }

    private void PrepareDictionary()
    {
        phaseDict.Add("Gather", TurnPhase.Gather);
        phaseDict.Add("Awaken", TurnPhase.Awaken);
        phaseDict.Add("Central", TurnPhase.Central);
        phaseDict.Add("Crystallize", TurnPhase.Crystallize);
        phaseDict.Add("End", TurnPhase.End);
    }
     
    
}

