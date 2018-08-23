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
    public delegate IEnumerator TurnPhaseFunction(GameEventsArgs e);
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
        this.isStartDone = false;
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

    public bool ActiveTurn
    {
        get { return this.isActiveTurn; }
    }

    public IEnumerator StartTurn()
    {
        this.isActiveTurn = true;
        yield return this.MoveToNextPhase();
        
    }

    public void EndTurn()
    {
        this.phase = TurnPhase.End;
        this.isActiveTurn = false;
    }

    private IEnumerator MoveToNextPhase()
    {
        manager.AdvanceGameTime();
        SetDelegate();
        GameEventsArgs turnPhaseEvent = Utilities.HelperFunctions.RaiseNewEvent(this, this.owner, this.owner, NonMoveAction.TurnPhase);
        Debug.Log(this.phase.ToString());

        if (isStartDone && this.Phase == TurnPhase.Start)
        {
            Debug.Log("Start already ran");
            this.phase++;
            Debug.Log("New phase is " + this.phase);
        }
        yield return this.PerformPhaseAction(turnPhaseEvent); //once this line finishes the turn phase is over

        TurnPhase currentPhase = this.phase; 
        
        if(currentPhase == TurnPhase.End)//if the End phase was the last turn phase to occur
        {
            Debug.Log("This turn is over should be ending");
            this.phase = TurnPhase.Gather; //set the turn phase to Gather for the next turn
            yield break;
            
        }
        else
        { 
            currentPhase++;
            TurnPhase nextPhase = currentPhase;//(TurnPhase)currentPhase;
            this.phase = nextPhase;
        }

        
               
        this.MoveToNextPhase(); //a recursive call to move through all the phases
    }

    private IEnumerator PerformPhaseAction(GameEventsArgs e)
    {
        MonoBehaviour mono = HelperFunctions.AccessMonoBehaviour();
        yield return mono.StartCoroutine(this.turnPhaseFunction(e));
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

    private IEnumerator StartGamePhase(GameEventsArgs e)
    {
        //Debug.Log("This is the game start phase, should only happen once a game");
        //Debug.Log("Creating Hand for AI?: " + this.owner.IsAI);
        Debug.Log("Value of Start Bool:" + isStartDone);
        if(!isStartDone)
        {
            this.owner.PlayerDeck.Draw(5);
            Debug.Log("5 Cards shouldve been added to the hand");
            this.isStartDone = true;
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator StartGatherPhase(GameEventsArgs e)
    {
        
        //Debug.Log("Is this the AI?: " + this.owner.IsAI);
        Debug.Log("Gather Phase: Draw Card and Abilities check the event queue");
            
        if(e.EventOwner.PlayerDeck.Count == 0)
        {
            Debug.Log("You Lose");
            HelperFunctions.RaiseNewEvent(this, this.owner, this.owner, NonMoveAction.GameEnd);
           
        }
        e.EventOwner.PlayerDeck.Draw();
        //Debug.Log("Player:" + e.EventOwner.PlayerID + " cards in hand " + e.EventOwner.GetLocation("Hand").Count);
        yield return new WaitForSeconds(1);

    }
    private IEnumerator StartAwakenPhase(GameEventsArgs e)
    {
        //I have changed the Awaken phase to randomly select
        /* if (this.owner.IsAI)
         {
             Debug.Log("Do some ai stuff");
         }
         else
         {
             Debug.Log("Awaken Phase: Choose your philosophers");
             //use is display to show the DZ and highlight which cards can be awakens includes philosophers and phantoms
         }*/
        yield return new WaitForSeconds(1);
    }

    private IEnumerator StartCentralPhase(GameEventsArgs e)
    {

        while(e.EventOwner.TimeLeftOnTimer > 0)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("The IEnumerator upstream works like a charm");
            e.EventOwner.TimeLeftOnTimer--;
        }

        /*
        if (this.owner.IsAI)
        {
            Debug.Log("Do some ai stuff");
        }
        else
        {
            yield return new WaitForSeconds(1);
            Debug.Log("Central Phase: Play cards and shit");
            //for now only spawning will be available activating effects requires them to be implemented
            //attacks will be do able from a UI option that is displayed or a button press (if its a button it will have to be done in update)
            //for attacks, blocks, and effect activation these will be members of an CardAction class
        }
        */
    }

    private IEnumerator StartCrystalPhase(GameEventsArgs e)
    {
        if (this.owner.IsAI)
        {
            //Debug.Log("Do some ai stuff");
        }
        else
        {
            yield return new WaitForSeconds(1);
            //Debug.Log("Crystallize Phase: collect your shards are whatever, still dont get why there's like 4 different words that describe the same thing");
            //display the SC and allow for selecting 3 different cards one to rebarrier, one to the bottom of the deck, and the other is sent to the grave
            //this can only be done if SC contains 3 or more shards
        }
    }

    private IEnumerator StartEndPhase(GameEventsArgs e)
    {
        yield return new WaitForSeconds(1);
        //Card abilities
    }


}

