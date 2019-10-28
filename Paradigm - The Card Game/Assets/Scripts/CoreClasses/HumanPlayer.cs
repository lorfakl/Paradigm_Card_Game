using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class HumanPlayer : Player, IPlayable
{
    

    public HumanPlayer(int id) :base(id)
    {
        //Calls constructor defined in Player class
        this.type = "Human";
        this.turn = new Turn(this);
    }

    public new bool UIStatus
    {
        get { return true; }
    }

    public int OriginalTimerTime
    {
        get { return timerTime; }
    }

    public override IEnumerator PerformAwaken()
    {
        Debug.Log("Awaken phase");//throw new System.NotImplementedException();
        yield return new WaitForSeconds(1);
    }

    public override IEnumerator PerformGather()
    {
        Debug.Log("Gather phase");//throw new System.NotImplementedException();
        yield return new WaitForSeconds(1);
    }

    public override IEnumerator PerformCentral()
    {
        Debug.Log("Central phase");//throw new System.NotImplementedException();
        yield return new WaitForSeconds(1);
    }

    public override IEnumerator PerformCrystal()
    {
        //Debug.Log("SC count: " + this.GetLocation(ValidLocations.SC).Count);
        Debug.Log("If its less than three then issa not gonna crystalize which it should be");
        if (this.GetLocation(ValidLocations.SC).Count >= 3)
        {
            Debug.Log("Human just started crystallized");
            HelperFunctions.SelectCards(this.PlayerDeck, this.GetLocation(ValidLocations.BZ), 1);
            HelperFunctions.SelectCards(this.PlayerDeck, this.GetLocation(ValidLocations.Grave), 1);
            HelperFunctions.SelectCards(this.PlayerDeck, this.GetLocation(ValidLocations.Deck), 1);
            while (this.TimeLeftOnTimer > 0)
            {
                yield return new WaitForSeconds(1);
                Debug.Log("Time left on timer: " + this.TimeLeftOnTimer);
                this.TimeLeftOnTimer--;
            }

            this.TimeLeftOnTimer = timerTime;
        }
    }

    public override IEnumerator PerformEnd()
    {
        Debug.Log("End phase");//throw new System.NotImplementedException();
        yield return new WaitForSeconds(1);
    }

    public override void PlayCard()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator ChooseTerritoryChallengeCard(Location temp)
    {
        //Debug.Log("I'd expect this to be AI's ID: " + this.GetLocation(ValidLocations.DZ).Content[0].Owner.PlayerID);
        Location lands = this.PlayerDeck.GetLandsAsLocation(this.GetLocation(ValidLocations.DZ));
        //Debug.Log("Show me your size: " + lands.Count);
        //Debug.Log("Show me your ID: " + lands.Owner.PlayerID);
        GameObject cardDisplay = HelperFunctions.SelectCards(lands, temp, 1);
        //Debug.Log("Now we wait!");
        cardDisplay = GameObject.FindWithTag("CardSelectionDisplay");

        while (this.TimeLeftOnTimer > 0)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("Time left on timer: " + this.TimeLeftOnTimer);
            this.TimeLeftOnTimer--;
        }

        this.TimeLeftOnTimer = timerTime;
        try
        {
            tcCard = (Landscape)temp.Content[0];
        }
        catch
        {
            //Debug.Log("Choosing for you");
            lands.MoveRandomContent(temp);
            GameObject cd = GameObject.FindWithTag("CardSelectionDisplay");
            cd.GetComponentInChildren<DisplaySelectionCards>().SendSelectionEnd();
            this.TimeLeftOnTimer = timerTime;

        }

        GameObject gm = GameObject.FindWithTag("GameManager");
        gm.GetComponent<EventManager>().UiPlayerReturnedLocation = temp;

        //Debug.Log("Frpm human " + temp.Owner.PlayerID);
        //Debug.Log("This ID: " + temp.Content[0].Owner.PlayerID);
    }

    public override IEnumerator ChooseBarriers(int barrierAmount)
    {
        GameObject cardDisplay = HelperFunctions.SelectCards(this.PlayerDeck, this.GetLocation(ValidLocations.BZ), barrierAmount);

        while (this.TimeLeftOnTimer > 0)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("Time left on timer: " + this.TimeLeftOnTimer);
            this.TimeLeftOnTimer--;
        }
        this.TimeLeftOnTimer = timerTime;

        try //when the timer is up and the human hasnt selected the barrierAmount barriers do it for them
        {
            DisplaySelectionCards displayScript = cardDisplay.GetComponentInChildren<DisplaySelectionCards>();
            if (displayScript.CardsSelected < barrierAmount)
            {
                for (int i = displayScript.CardsSelected; i < barrierAmount; i++)
                {
                    Card c = PlayerDeck.SelectRandomContent();
                    while (displayScript.SelectedCards.Contains(c))
                    {
                        c = PlayerDeck.SelectRandomContent();
                    }

                    //Debug.Log(c.Name);
                    displayScript.UpdateSelectedCards(c, true);
                }

                displayScript.SendSelectionEnd();
            }
        }
        catch (Exception)
        {

        }

        this.IsPreparedToStart = true;
    } 

    public override PlayerInteraction GetInteraction()
    {
        gamePlayHook = FindPlayerInteraction("Player");
        if(gamePlayHook == null)
        {
            throw new Exception("GamePlay Hook is null, check that the IPlayable instances are linked properly to the PlayerInteraction script");
        }
        return gamePlayHook;
    }

    public override bool GetPlayerUIStatus()
    {
        return true;
    }

    public override void ListLocationSizes()
    {
        foreach (string l in validLocations)
        {
            Location loc = GetLocation(l);
            //Debug.Log(true + " Name: " + loc.Name + " Count: " + loc.Count);
        }
    }
}
