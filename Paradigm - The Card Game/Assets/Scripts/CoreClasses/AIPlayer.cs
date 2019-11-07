using AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class AIPlayer : Player
{
    private int aiAttackChance;
    

    public AIPlayer(int id) : base(id)
    {
        //Calls constructor defined in Player class
        this.type = "AI";
        this.turn = new Turn(this);
    }

    public new bool UIStatus
    {
        get { return false; }
    }

    public int AttackChance
    {
        get { return aiAttackChance; }
        set { aiAttackChance = value; }
    }

    public override IEnumerator PerformGather()
    {
        Debug.Log("AI Gather phase");//throw new System.NotImplementedException();
        PlayerTurn.IsPhaseActive = true;
        Debug.Log("TurnPhase is now Active: " + PlayerTurn.IsPhaseActive + " All other turnphases are locked");
        HelperFunctions.RaiseNewEvent(this, this, TurnPhase.Gather);

        
        PlayerTurn.IsPhaseActive = false;
        Debug.Log("TurnPhase is no longer Active: " + PlayerTurn.IsPhaseActive + " All other turnphases are unlocked");
        yield return new WaitForSeconds(1);
    }

    public override IEnumerator PerformAwaken()
    {
        Debug.Log("AI Awaken phase");//throw new System.NotImplementedException();
        PlayerTurn.IsPhaseActive = true;
        Debug.Log("TurnPhase is now Active: " + PlayerTurn.IsPhaseActive + " All other turnphases are locked");
        HelperFunctions.RaiseNewEvent(this, this, TurnPhase.Awaken);


        
        PlayerTurn.IsPhaseActive = false;
        Debug.Log("TurnPhase is no longer Active: " + PlayerTurn.IsPhaseActive + " All other turnphases are unlocked");
        yield return new WaitForSeconds(1);
    }

    public override IEnumerator PerformCentral()
    {
        Debug.Log("AI Central Phase");
        PlayerTurn.IsPhaseActive = true;
        Debug.Log("TurnPhase is now Active: " + PlayerTurn.IsPhaseActive + " All other turnphases are locked");
        HelperFunctions.RaiseNewEvent(this, this, TurnPhase.Central);
        List<Card> hand = GetLocation(ValidLocations.Hand).GetContents(typeof(Accessor));
        if (hand != null) //there are accessors in the AI's hand
        {
            for (int i = 0; i < 2; i++)
            {
                if (hand != null)
                {
                    int index = UnityEngine.Random.Range(0, hand.Count);
                    Card c = hand[index];
                    GetLocation(ValidLocations.Hand).MoveContent(c, GetLocation(ValidLocations.Field));
                    hand = GetLocation(ValidLocations.Hand).GetContents(typeof(Accessor));
                }
            }

            int chance = UnityEngine.Random.Range(0, AttackChance);
            if (chance == AttackChance)
            {
                Debug.Log("Launch an Attack!");
            }
        }
        else //there are not any accessors in the AI's hand
        {
            //skip turn?
        }

        //yield return new WaitForSeconds(1);
        PlayerTurn.IsPhaseActive = false;
        Debug.Log("TurnPhase is no longer Active: " + PlayerTurn.IsPhaseActive + " All other turnphases are unlocked");
        yield return new WaitForSeconds(1);
    }

    public override IEnumerator PerformCrystal()
    {
        Debug.Log("AI Crystal Phase");
        PlayerTurn.IsPhaseActive = true;
        Debug.Log("TurnPhase is now Active: " + PlayerTurn.IsPhaseActive + " All other turnphases are locked");
        HelperFunctions.RaiseNewEvent(this, this, TurnPhase.Crystallize);
        Location sc = GetLocation(ValidLocations.SC);
        if (sc.Count >= 3)
        {
            List<Card> shardList = sc.Content;
            Enum[] validLocations = new Enum[] { ValidLocations.Deck, ValidLocations.Grave, ValidLocations.BZ };
            for (int i = 0; i < 3; i++)
            {
                shardList = sc.Content;
                int index = UnityEngine.Random.Range(0, shardList.Count);
                Card c = shardList[index];
                Location destination = GetLocation((ValidLocations)validLocations[i]);
                sc.MoveContent(c, destination);
            }
        }

        //yield return new WaitForSeconds(1);
        PlayerTurn.IsPhaseActive = false;
        Debug.Log("TurnPhase is no longer Active: " + PlayerTurn.IsPhaseActive + " All other turnphases are unlocked");
        yield return new WaitForSeconds(1);
    }

    public override IEnumerator PerformEnd()
    {
        Debug.Log("AI End phase");//throw new System.NotImplementedException();
        PlayerTurn.IsPhaseActive = true;
        Debug.Log("TurnPhase is now Active: " + PlayerTurn.IsPhaseActive + " All other turnphases are locked");
        HelperFunctions.RaiseNewEvent(this, this, TurnPhase.End);


        //yield return new WaitForSeconds(1);
        PlayerTurn.IsPhaseActive = false;
        Debug.Log("TurnPhase is no longer Active: " + PlayerTurn.IsPhaseActive + " All other turnphases are unlocked");
        yield return new WaitForSeconds(1);
    }

    public override IEnumerator ChooseTerritoryChallengeCard(Location t)
    {
        Card chosenLand = null;
        //Debug.Log("I think its empty: " + this.GetLocation(ValidLocations.DZ).Count);
        //Debug.Log("This ID: " + this.PlayerID + " the location ID: " + GetLocation(ValidLocations.DZ).Owner.PlayerID);
        foreach (Card c in GetLocation(ValidLocations.DZ))
        {
            if (c.GetType() == typeof(Landscape))
            {
                chosenLand = c;
            }
        }

        if (chosenLand != null)
        {
            //Debug.Log("AI is a TC card");
            TCCard = chosenLand;
            //Debug.Log("This ID: " + TCCard.Owner.PlayerID);
            TCCard.Owner = this;
            //Debug.Log("Post Change This ID: " + TCCard.Owner.PlayerID);

            GetLocation(ValidLocations.DZ).MoveContent(chosenLand, t);
            GameObject gm = GameObject.FindWithTag("GameManager");
            gm.GetComponent<EventManager>().NoUiPlayerReturnedLocation = t;
            Debug.Log("Frpm AI " + t.Owner.PlayerID);
            yield return new WaitForSeconds(1);
        }
    }

    public override IEnumerator ChooseBarriers(int barrierCount)
    {
        Debug.Log("Does this even get called? AI BZ");
        List<Card> barriers = new List<Card>();
        for (int i = 0; i <= barrierCount; i++)
        {
            Card c = PlayerDeck.SelectRandomContent();
            while (barriers.Contains(c))
            {
                c = PlayerDeck.SelectRandomContent();
            }
            barriers.Add(c);
        }
        PlayerDeck.MoveContent(barriers, GetLocation(ValidLocations.BZ));
        yield return new WaitForSeconds(1);
        this.IsPreparedToStart = true;
    }

    public override void PlayCard()
    {
        throw new NotImplementedException();
    }

    public override PlayerInteraction GetInteraction()
    {
        gamePlayHook = FindPlayerInteraction("AiPlayer");
        if (gamePlayHook == null)
        {
            throw new Exception("GamePlay Hook is null, check that the IPlayable instances are linked properly to the PlayerInteraction script");
        }
        return gamePlayHook; ;
    }

    public override bool GetPlayerUIStatus()
    {
        return false;
    }

    public override void ListLocationSizes()
    {
        foreach (string l in validLocations)
        {
            Location loc = GetLocation(l);
            Debug.Log(false + " Name: " + loc.Name + " Count: " + loc.Count);
        }
    }
}
