using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class HumanPlayer : Player, IPlayable
{
    public static readonly int timerTime = 45;
    private int timeLeftOnTimer = timerTime;

    public HumanPlayer(GameTimeManager mgmt, int id) :base(mgmt, id)
    {
        //Calls constructor defined in Player class
        this.type = "Human";
    }

    public int TimeLeftOnTimer
    {
        get { return this.timeLeftOnTimer; }
        set { this.timeLeftOnTimer = value; }
    }

    public int OriginalTimerTime
    {
        get { return timerTime; }
    }

    public IEnumerator PerformAwaken()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator PerformCentral()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator PerformCrystal()
    {
        Debug.Log("SC count: " + this.GetLocation(ValidLocations.SC).Count);
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

    public IEnumerator PerformGather()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator ChooseTerritoryChallengeCard(Location temp)
    {
        Location lands = this.PlayerDeck.GetLandsAsLocation();
        //Debug.Log("Show me your size: " + lands.Count);
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
            Debug.Log("Choosing for you");
            lands.MoveRandomContent(temp);
            GameObject cd = GameObject.FindWithTag("CardSelectionDisplay");
            cd.GetComponentInChildren<DisplaySelectionCards>().SendSelectionEnd();
            this.TimeLeftOnTimer = timerTime;

        }

        GameObject gm = GameObject.FindWithTag("GameManager");
        gm.GetComponent<GameEventsManager>().UiPlayerReturnedLocation = temp;
    }

    public IEnumerator ChooseBarriers()
    {
        GameObject cardDisplay = HelperFunctions.SelectCards(this.PlayerDeck, this.GetLocation(ValidLocations.BZ), 12);

        while (this.TimeLeftOnTimer > 0)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("Time left on timer: " + this.TimeLeftOnTimer);
            this.TimeLeftOnTimer--;
        }
        this.TimeLeftOnTimer = timerTime;

        try //when the timer is up and the human hasnt selected the 12 barriers do it for them
        {
            DisplaySelectionCards displayScript = cardDisplay.GetComponentInChildren<DisplaySelectionCards>();
            if (displayScript.CardsSelected < 12)
            {
                for (int i = displayScript.CardsSelected; i < 12; i++)
                {
                    Card c = PlayerDeck.SelectRandomContent();
                    while (displayScript.SelectedCards.Contains(c))
                    {
                        c = PlayerDeck.SelectRandomContent();
                    }

                    Debug.Log(c.Name);
                    displayScript.UpdateSelectedCards(c, true);
                }

                displayScript.SendSelectionEnd();
            }
        }
        catch (Exception)
        {

        }
    }

    public void PlayCard()
    {
        throw new NotImplementedException();
    }
}
