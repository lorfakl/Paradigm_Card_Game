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
    private static bool isDictionaryPrepared = false;
    public delegate void TurnPhaseFunction(GameEventsArgs e);
    TurnPhaseFunction turnPhaseFunction;
    private static Dictionary<TurnPhase, TurnPhaseFunction> phaseDict = new Dictionary<TurnPhase, TurnPhaseFunction>();
    private GameTimeManager manager;


    public Turn(Player p, GameTimeManager manager)
    {
        if(!isDictionaryPrepared)
        {
            PrepareDictionary();
        }
      
        this.owner = p;
        this.phase = TurnPhase.Start;
        this.isPhaseComplete = false;
        this.manager = manager;
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

    public void EndTurn()
    {
        this.phase = TurnPhase.End;
    }

    private void MoveToNextPhase()
    {
        manager.AdvanceGameTime();
        SetDelegate();
        GameEventsArgs turnPhaseEvent = Utilities.HelperFunctions.RaiseNewEvent(this, this.owner, this.owner, NonMoveAction.TurnPhase);
        Debug.Log(this.phase.ToString());
        this.PerformPhaseAction(turnPhaseEvent); //once this line finishes the turn phase is over

        TurnPhase currentPhase = this.phase; 
        if(currentPhase == TurnPhase.End)//if the End phase was the last turn phase to occur
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
        Player p = e.EventOwner;
        
        List<Card> lands = p.PlayerDeck.GetLandscapesInDeck();
        Debug.Log(lands.Count);
        //instaniate the display
        //Debug.Log("Display Landscapes");
        foreach(Card c in lands)
        {
            //Debug.Log(c.getName());
        }
        //send the lands to the display function
        //get the selected land from the display function
        GameEventsManager.AddTCLand(lands[UnityEngine.Random.Range(0, 1)]); //choose a random landscape from the deck just for testing
        p.PlayerTurn.Phase = TurnPhase.End; //this line is kinda hacky



    }

    private static void StartGatherPhase(GameEventsArgs e)
    {
        Debug.Log("Gather Phase: Draw Card and Abilities check the event queue");
        e.EventOwner.DrawFromDeck();
        Debug.Log("Player:" + e.EventOwner.PlayerID + " cards in hand " + e.EventOwner.GetLocation("Hand").Count);
    }

    private static void StartAwakenPhase(GameEventsArgs e)
    {
        Debug.Log("Awaken Phase: Choose your philosophers");
        //use is display to show the DZ and highlight which cards can be awakens includes philosophers and phantoms
    }

    private static void StartCentralPhase(GameEventsArgs e)
    {
        Debug.Log("Central Phase: Play cards and shit");
        //for now only spawning will be available activating effects requires them to be implemented
        //attacks will be do able from a UI option that is displayed or a button press (if its a button it will have to be done in update)
        //for attacks, blocks, and effect activation these will be members of an CardAction class

    }

    private static void StartCrystalPhase(GameEventsArgs e)
    {
        Debug.Log("Crystallize Phase: collect your shards are whatever, still dont get why there's like 4 different words that describe the same thing");
        //display the SC and allow for selecting 3 different cards one to rebarrier, one to the bottom of the deck, and the other is sent to the grave
        //this can only be done if SC contains 3 or more shards
    }

    private static void StartEndPhase(GameEventsArgs e)
    {
        Debug.Log("End Phase: End of your turn fuck face! Abilities that want to will activate here");
        //Once Crystal phase ends the end phase is started automatically, this is just for abilities to activate really
    }


}

