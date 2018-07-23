using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;
using AI;


public class Player
    {

        //private Turn playerTurn;
        public static int timerTime = 45;
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
        public void LoadDeckFromDataBase()
        {
            playerDeck.MoveContent(DataBase.CardDataBase.LoadPlayerDeck(), playerDeck);
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

                try
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
        
        private void GetReturnedLocation(object s, Location d)
        {
            //Debug.Log(s + " just returned to us a location with " + d.Count + " cards");
            //this.ReturnedLocation = d;
        }
        
        
}

