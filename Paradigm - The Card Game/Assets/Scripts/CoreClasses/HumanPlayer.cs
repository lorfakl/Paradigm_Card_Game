using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class HumanPlayer : Player, IPlayable
{
    

    public HumanPlayer(GameTimeManager mgmt, int id) :base(mgmt, id)
    {
        //Calls constructor defined in Player class
        this.type = PlayerType.Human;
    }

    public HumanPlayer(Guid id) : base(id)
    {
        this.type = PlayerType.Human;
    }

    public int OriginalTimerTime
    {
        get { return timerTime; }
    }

    public override IEnumerator PerformGather()
    {
        Debug.Log("Human Draw a Card!");
        try {
            HelperFunctions.RaiseNewEvent(this, this, this, NonMoveAction.TurnPhase);
            Debug.Log("Human Draw a Card!");
            if (PlayerDeck.Count > 0)
            {
                PlayerDeck.Draw();
            }
            else
            {
                HelperFunctions.RaiseNewEvent(this, this, this, NonMoveAction.GameEnd);
            }
            
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.Log(ex.StackTrace);
            Debug.Log(ex.ToString());
        }
        yield return new WaitForSeconds(1);

    }

    public override IEnumerator PerformAwaken()
    {
        Debug.Log("Awaken my LOVE!");
        yield return null;
    }

    public override IEnumerator PerformCentral()
    {
        Debug.Log("CENTRAL");
        if (centralActions > 0)
        {
            Location hand = GetLocation(ValidLocations.Hand);
            //SetCardScript(false); //Disable the CardScript
            
            while(centralActions > 0)
            {
                Debug.Log("Not sure what to put in this while loop honestly");
                centralActions--;
            }

            //SetCardScript(true); //enable it back
        }

        yield return new WaitForSeconds(1);
    }

    public override IEnumerator PerformCrystal()
    {
        Debug.Log("SC count: " + this.GetLocation(ValidLocations.SC).Count);
        Debug.Log("If its less than three then issa not gonna crystalize which it should be");
        if (this.GetLocation(ValidLocations.SC).Count >= 3)
        {
            HelperFunctions.RaiseNewEvent(this, this, this, NonMoveAction.TurnPhase);
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
        else
        {
            Debug.Log("Less than three bro, no crystalize");
        }
    }

    public override IEnumerator ChooseTerritoryChallengeCard(Location temp)
    {
        Location lands = this.PlayerDeck.GetLandsAsLocation();
        //Debug.Log("Show me your size: " + lands.Count);
        GameObject cardDisplay = HelperFunctions.SelectCards(lands, temp, 1);
        GameObject timer = GameObject.FindGameObjectWithTag("timer");
        Text timerText = timer.GetComponent<Text>();
        //Debug.Log("Now we wait!");
        cardDisplay = GameObject.FindWithTag("CardSelectionDisplay");

        while (this.TimeLeftOnTimer > 0)
        {
            yield return new WaitForSeconds(1);
            timerText.text = "00:" + this.TimeLeftOnTimer;
            //Debug.Log("Time left on timer: " + this.TimeLeftOnTimer);
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
    }

    public override IEnumerator ChooseBarriers(int barrierAmount)
    {
        GameObject cardDisplay = HelperFunctions.SelectCards(this.PlayerDeck, this.GetLocation(ValidLocations.BZ), barrierAmount);
        GameObject timer = GameObject.FindGameObjectWithTag("timer");
        Text timerText = timer.GetComponent<Text>();

        while (this.TimeLeftOnTimer > 0)
        {
            yield return new WaitForSeconds(1);
            timerText.text = "00:" + this.TimeLeftOnTimer;
            //Debug.Log("WHATS GOING ON");
            //Debug.Log("Time left on timer: " + this.TimeLeftOnTimer);
            this.TimeLeftOnTimer--;
        }
        this.TimeLeftOnTimer = timerTime;
        if (this.GetLocation(ValidLocations.BZ).Count != barrierAmount)
        {//check the destination to see if we need to add cards automatically
            Debug.Log("Major issue here not enough cards in BarrierZone there are current only: " + this.GetLocation(ValidLocations.BZ).Count);

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

                        Debug.Log(c.Name);
                        displayScript.UpdateSelectedCards(c, true);
                    }

                    displayScript.SendSelectionEnd();
                    TimeLeftOnTimer = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log(ex.StackTrace);
                Debug.Log(ex.ToString());
            }
        }

        PlayerDeck.Draw(5);

    }

    public override bool GetPlayerUIStatus()
    {
        return true;
    }

    public override IEnumerator PerformEnd()
    {
        Debug.Log("Human EndPhase");
        yield return new WaitForSeconds(1);
    }

    public override IEnumerator PerformAttack()
    {
        throw new NotImplementedException();
    }

    private void SetCardScript(bool status)
    {
        throw new Exception("You tried to run this...idiot. This function shouldnt disable cardscript it should flip some sort of control switch within CardScript");
        foreach (Card c in GetLocation(ValidLocations.Hand).GetContents())
        {
            try
            {
                CardScript cs = c.GameObj.GetComponent<CardScript>();
                cs.enabled = status;
                //CentralPhaseCardScript cpcs = c.GameObj.GetComponent<CentralPhaseCardScript>();
                //cpcs.enabled = !status;
            }
            catch (NullReferenceException)
            {
                throw new Exception("There is no Cardscript or CentralPhaseCardScript attached to these Gameobjects");
            }
        }
    }
}
