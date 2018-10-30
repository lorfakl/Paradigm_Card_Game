using AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player, IPlayable
{
    private int aiAttackChance;

    public AIPlayer(GameTimeManager mgmt, int id) : base(mgmt, id)
    {
        //Calls constructor defined in Player class
        this.type = "AI";
    }

    public int AttackChance
    {
        get { return aiAttackChance; }
        set { aiAttackChance = value; }
    }

    public IEnumerator PerformAwaken()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator PerformCentral()
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

    public IEnumerator PerformCrystal()
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

    public IEnumerator PerformGather()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator ChooseTerritoryChallengeCard(Location t)
    {
        Card chosenLand = null;
        int index = UnityEngine.Random.Range(0, 2);
        foreach (Card c in GetLocation(ValidLocations.DZ).GetContents())
        {
            if (c.GetType() == typeof(Landscape))
            {
                chosenLand = c;
            }
        }
        TCCard = chosenLand;
        GetLocation(ValidLocations.DZ).MoveContent(chosenLand, t);
        yield return 5;
    }

    public IEnumerator ChooseBarriers(int barrierCount)
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

    public void PlayCard()
    {
        throw new NotImplementedException();
    }
}
