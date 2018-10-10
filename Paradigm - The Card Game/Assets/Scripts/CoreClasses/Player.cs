using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;
using AI;

public enum ValidLocations { Hand, Grave, LockZ, BZ, LandZ, SC, PZ, DZ, Field, Deck}
public class Player
    {

    //private Turn playerTurn;
    public static readonly int timerTime = 45;
    private int timeLeftOnTimer = timerTime;
        
        
    private Dictionary<string, Location> cardLocations = new Dictionary<string, Location>();
    private static string[] validLocations = { "Hand", "Grave", "LockZ", "BZ", "LandZ", "SC", "PZ", "DZ", "Field", "Deck" };
    private Deck playerDeck;
    private int playerID;
    //private AuxiliaryCard tcLandscape = null;
    private Majesty majesty;
    private Landscape tcCard;
    private List<Landscape> lands;
    private Turn turn;
    private bool isAI;
    private string type;
    private int aiAttackChance;
    private bool isPreparedToStart;
    private Location returnedLocation;
    private static List<Player> currentPlayers = new List<Player>();

    public Player(GameTimeManager mgmt, int addTo = 0, bool isAI = false)
    {
        
        this.playerID = UnityEngine.Random.Range(0,256);
        if (addTo != 0)
        {
            this.playerID = this.playerID + UnityEngine.Random.Range(510, 2048);
        }

        this.majesty = null;
        this.turn = new Turn(this, mgmt);

        foreach (string s in validLocations)
        {
            this.cardLocations.Add(s, new Location(s, this));
        }
        //Debug.Log("Dictionary Size: " + cardLocations.Count);
        this.playerDeck = new Deck("Deck", this);
        this.cardLocations["Deck"] = this.playerDeck;
        this.isAI = isAI;
        this.isPreparedToStart = false;
        DisplaySelectionCards.IsDoneChoosing += GetReturnedLocation;
        if(this.isAI)
        {
            this.type = "AI";
        }
        else
        {
            this.type = "Human";
        }
    }

    public Deck PlayerDeck
    {
        get { return playerDeck; }
        set { playerDeck = value; }
    }

    public Majesty Majesty
    {
        get { return majesty; }
        set { majesty = value; }
    }

    public int PlayerID
    {
        get { return playerID; }
    }

    public string PlayerName
    {
        get { return type; }
    }

    public Turn PlayerTurn
    {
        get { return this.turn; }
        set { this.turn = value; }
    }

    public bool IsAI
    {
        get { return this.isAI; }
    }

    public Location ReturnedLocation
    {
        get { return this.returnedLocation; }
        set { this.returnedLocation = value; }
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
        
    public Card TCCard
    {
        get { return this.tcCard; }
        set { this.tcCard = (Landscape)value; }
    }
   
    public bool IsPreparedToStart
    {
        get { return isPreparedToStart; }
        set { isPreparedToStart = value; }
    }

    public int AttackChance
    {
        get { return aiAttackChance; }
        set { aiAttackChance = value; }
    }

    public void LoadDeckFromDataBase()
    {
        playerDeck.MoveContent(DataBase.CardDataBase.LoadPlayerDeck(), playerDeck);
    }

    public Location GetLocation(ValidLocations l)
    {
        return GetLocation(l.ToString());
    }

    public Location GetLocation(string l)
    {
        bool validVal = false;
        foreach (string s in validLocations)
        {
            if(s.Contains(l))
            {
                validVal = true;
            }
        }

        if(!validVal)
        {
            Debug.Log("Invalid Argument!");
            return null;
        }

        return cardLocations[l];
    }

    public int GetLocationCount(string name)
    {
        return this.GetLocation(name).Count;
    }

    public void AddToField(Card c)
    {
    this.cardLocations["Hand"].MoveContent(c, this.cardLocations["Field"]);   
    }
    
    public void AddBarrier(Card c)
    {
        c.setShard(true);
        c.setBarrierStatus(true);
        c.getLocation().MoveContent(c, this.cardLocations["BZ"]);
    }

    public void AddToHand(Card c)
    {
        c.getLocation().MoveContent(c, this.cardLocations["Hand"]);
    }
    //End Adds


    //Housing Keeping functions
    public void DestroyBarrier()
    {
        Card c = this.cardLocations["BZ"].GetContents()[0];
        c.setBarrierStatus(false);
        c.getLocation().MoveContent(c, this.cardLocations["SC"]);
    }
    //End Housing Keeping functions

    //Card Transit
    public void SendToGrave(Card c)
    {
        c.setDestoyedStatus(true);
        c.getLocation().MoveContent(c, this.cardLocations["Grave"]);
    }

    public void SendToShardPile(Card c)
    {
        c.getLocation().MoveContent(c, this.cardLocations["SC"]);
    }

    //End Card Transit

    public IEnumerator ChooseTerritoryChallengeCard(Location temp)
    {
        if (isAI)
        {
            AiFunctions.ChooseTCCard(this, temp);
        }
        else
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
    }

    public IEnumerator ChooseBarriers()
    {
        if (this.isAI)
        {
        //AI Namespace Call
            AiFunctions.ChooseBarriers(this, 12);
        }
        else
        {
            GameObject cardDisplay = HelperFunctions.SelectCards(this.PlayerDeck, this.GetLocation("BZ"), 12);
                
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
                    for(int i = displayScript.CardsSelected; i < 12; i++)
                    {
                        Card c = PlayerDeck.SelectRandomContent();
                        while(displayScript.SelectedCards.Contains(c))
                        {
                            c = PlayerDeck.SelectRandomContent();
                        }

                        Debug.Log(c.Name);
                        displayScript.UpdateSelectedCards(c, true);
                    }

                    displayScript.SendSelectionEnd();
                }
            }
            catch(Exception)
            {
                    
            }
        }
    }
        
    public IEnumerator ChooseCentralPhaseActions()
    {
        Debug.Log("Is timer set to 0:" + this.TimeLeftOnTimer);
        if (this.isAI)
        {
            AiFunctions.ChooseCentralPhaseActions(this);
        }
        else
        {
            Debug.Log("Human central should just be 45 seconds of stillness");
            while (this.TimeLeftOnTimer > 0)
            {
                yield return new WaitForSeconds(1);
                this.TimeLeftOnTimer--;
            }
            
            this.TimeLeftOnTimer = timerTime;
        }
        
    }

    public IEnumerator ChooseCrystalPhaseActions()
    {
        Debug.Log("Is timer set to 0:" + this.TimeLeftOnTimer);
        if (this.isAI)
        {
            AiFunctions.ChooseCrystalPhaseActions(this);
            Debug.Log("AI just crystallized");
        }
        else
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
        
    }

    private void GetReturnedLocation(object s, Location d)
    {
        //Debug.Log(s + " just returned to us a location with " + d.Count + " cards");
        //this.ReturnedLocation = d;
    }
            
    public void PlayCard(String cardType)
    {

    }
        
}

