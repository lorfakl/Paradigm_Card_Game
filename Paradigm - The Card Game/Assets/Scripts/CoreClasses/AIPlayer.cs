﻿using AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    private int aiAttackChance;
    

    public AIPlayer(int id) : base(id)
    {
        //Calls constructor defined in Player class
        this.type = "AI";
        
    }

    public new bool UIStatus
    {
        get { return true; }
    }

    public int AttackChance
    {
        get { return aiAttackChance; }
        set { aiAttackChance = value; }
    }

    public override IEnumerator PerformAwaken()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator PerformCentral()
    {
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

        yield return 5;
    }

    public override IEnumerator PerformCrystal()
    {
        Location sc = GetLocation(ValidLocations.SC);
        if (sc.Count >= 3)
        {
            List<Card> shardList = sc.GetContents();
            Enum[] validLocations = new Enum[] { ValidLocations.Deck, ValidLocations.Grave, ValidLocations.BZ };
            for (int i = 0; i < 3; i++)
            {
                shardList = sc.GetContents();
                int index = UnityEngine.Random.Range(0, shardList.Count);
                Card c = shardList[index];
                Location destination = GetLocation((ValidLocations)validLocations[i]);
                sc.MoveContent(c, destination);
            }
        }

        yield return 5;
    }

    public override IEnumerator PerformGather()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator ChooseTerritoryChallengeCard(Location t)
    {
        Card chosenLand = null;
        Debug.Log("I think its empty: " + GetLocation(ValidLocations.DZ).Count);
        foreach (Card c in GetLocation(ValidLocations.DZ).GetContents())
        {
            if (c.GetType() == typeof(Landscape))
            {
                chosenLand = c;
            }
        }

        if (chosenLand != null)
        {
            Debug.Log("AI is a TC card");
            TCCard = chosenLand;
            GetLocation(ValidLocations.DZ).MoveContent(chosenLand, t);
            yield return 5;
        }
    }

    public override IEnumerator ChooseBarriers(int barrierCount)
    {
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
        yield return 5;
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

    public override IEnumerator PerformEnd()
    {
        throw new NotImplementedException();
    }
}
