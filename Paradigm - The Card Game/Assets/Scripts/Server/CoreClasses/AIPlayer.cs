using AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class AIPlayer : Player
{
    
    private int initChance = 41;
    private int spawnChance = 60;
    private int aiAttackChance = 50;
    private bool isAlwaysAttacking = false;
    private bool isAlwaysSpawning = true;

    public AIPlayer(GameTimeManager mgmt, int id) : base(mgmt, id)
    {
        //Calls constructor defined in Player class
        this.type = PlayerType.AI;
    }

    public AIPlayer(Guid id) : base(id)
    {
        this.type = PlayerType.AI;
    }

    public int AttackChance
    {
        get { return aiAttackChance; }
    }

    public override IEnumerator PerformAwaken()
    {
        Debug.Log("Wanna do more UI work before implementation");
        HelperFunctions.RaiseNewEvent(this, this, this, new GameAction(NonMoveAction.Turn, TurnPhase.Awaken));
        yield return new WaitForSeconds(3);
    }

    private void GiveAlwaysReminder(bool anAlways)
    {
        if(anAlways == true)
        {
            Debug.LogWarning("One or more always flags are sent be sure thats on purpose");
        }
    }

    public override IEnumerator PerformCentral()
    {
        Debug.Log("AI Central");
        HelperFunctions.RaiseNewEvent(this, this, this, new GameAction(NonMoveAction.Turn, TurnPhase.Central));
        do
        {
            Location hand = GetLocation(ValidLocations.Hand);

            if (GetChanceSuccess(spawnChance) || isAlwaysSpawning)//if random suceeds
            {
                GiveAlwaysReminder(isAlwaysSpawning);
                Debug.Log("AI Spawn");
                if (hand.Contains(typeof(Accessor)))
                {
                    List<Card> accessors = hand.GetContents(typeof(Accessor));
                    HelperFunctions.Print("AI hand count: " + hand.Count);
                    int index = GetRandomVal(accessors.Count);
                    try
                    {
                        Card choice = accessors[index];
                        PlayCard(choice);
                    }
                    catch(Exception e)
                    {
                        HelperFunctions.Print("This is that weird Index error I bet");
                        HelperFunctions.Print(e.Message);
                        HelperFunctions.Print(e.StackTrace);
                    }
                    
                    centralActions--;
                }
            }

            if (GetChanceSuccess(initChance))
            {
                List<Card> targetCards = null;
                Debug.Log("AI Initiate");

                if (hand.Contains(typeof(Element)))
                {
                    targetCards = hand.GetContents(typeof(Element));
                }
                else if(hand.Contains(typeof(Mechanism)))
                {
                    targetCards = hand.GetContents(typeof(Mechanism));
                }

                
                try
                {
                    int index = GetRandomVal(targetCards.Count);
                    Card choice = targetCards[index];
                    PlayCard(choice);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    Debug.Log(ex.StackTrace);
                    Debug.Log(ex.ToString());
                }

                
                centralActions--;
            }

            if (GetChanceSuccess(aiAttackChance) || isAlwaysAttacking)
            {
                GiveAlwaysReminder(isAlwaysAttacking);
                Debug.Log("AI Attack");
                //PerformAttack();
                centralActions--;
            }
        } while (centralActions > 0);

        yield return new WaitForSeconds(5);
    }

    public override IEnumerator PerformCrystal()
    {
        Debug.Log("AI Crystallize");
        HelperFunctions.RaiseNewEvent(this, this, this, new GameAction(NonMoveAction.Turn, TurnPhase.Crystallization));
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
        yield return new WaitForSeconds(5);
        
    }

    public override IEnumerator PerformGather()
    {
        try
        {
            HelperFunctions.RaiseNewEvent(this, this, this, new GameAction(NonMoveAction.Turn, TurnPhase.Gather));
            Debug.Log("AI Draw a Card!");
            if (PlayerDeck.Count > 0)
            {
                PlayerDeck.Draw();
            }
            else
            {
                HelperFunctions.RaiseNewEvent(this, this, this, new GameAction(NonMoveAction.GameEnd, TurnPhase.Gather));
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.Log(ex.StackTrace);
            Debug.Log(ex.ToString());
        }
        yield return new WaitForSeconds(3);

    }

    public override IEnumerator ChooseTerritoryChallengeCard(Location t)
    {
        Location lands = this.PlayerDeck.GetLandsAsLocation();
        Debug.Log("Show me your size: " + lands.Count);
        Card chosenLand = lands.SelectRandomContent();
        TCCard = chosenLand;
        GetLocation(ValidLocations.DZ).MoveContent(chosenLand, t);
        yield return null;
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

        //PlayerDeck.Draw(5);
        yield return null;
    }

 

    public override bool GetPlayerUIStatus()
    {
        return false;
    }

    public override IEnumerator PerformEnd()
    {
        Debug.Log("AI EndPhase");
        HelperFunctions.RaiseNewEvent(this, this, this, new GameAction(NonMoveAction.Turn, TurnPhase.End));
        yield return new WaitForSeconds(3);
    }

    public override IEnumerator PerformAttack()
    {
        throw new NotImplementedException();
    }

    private int GetRandomVal(int max)
    {
        return UnityEngine.Random.Range(0, max + 1);
    }

    private bool GetChanceSuccess(int chanceVal)
    {
        int thresh = UnityEngine.Random.Range(0, 101);
        if(thresh > chanceVal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
