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
        private List<Landscape> lands;
        private Turn turn;
        private bool isAI;
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
                cardLocations.Add(s, new Location(s, this));
            }
            //Debug.Log("Dictionary Size: " + cardLocations.Count);
            this.playerDeck = new Deck("Deck", this);
            cardLocations["Deck"] = this.playerDeck;
            this.isAI = isAI;
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
        
   
        /*public AuxiliaryCard TCLandscape
        {
            get { return tcLandscape; }
            set { tcLandscape = value; }
        }*/
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


        public void AddToField(Card c)
        {
            cardLocations["Hand"].MoveContent(c, cardLocations["Field"]);   
        }
    
        public void AddBarrier(Card c)
        {
            c.setShard(true);
            c.setBarrierStatus(true);
            c.getLocation().MoveContent(c, cardLocations["BZ"]);
        }

        public void AddToHand(Card c)
        {
            c.getLocation().MoveContent(c, cardLocations["Hand"]);
        }
        //End Adds


        //Housing Keeping functions
        public void DestroyBarrier()
        {
            Card c = cardLocations["BZ"].GetContents()[0];
            c.setBarrierStatus(false);
            c.getLocation().MoveContent(c, cardLocations["SC"]);
        }
        //End Housing Keeping functions

        //Card Transit
        public void SendToGrave(Card c)
        {
            c.setDestoyedStatus(true);
            c.getLocation().MoveContent(c, cardLocations["Grave"]);
        }

        public void SendToShardPile(Card c)
        {
            c.getLocation().MoveContent(c, cardLocations["SC"]);
        }

        public void DrawFromDeck(int drawVal = 1)
        {
            List<Card> cardsDrawn = playerDeck.Draw(drawVal);
            foreach (Card c in cardsDrawn)
            {
                c.getLocation().MoveContent(c, cardLocations["Hand"]);
            }
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
                if(temp.Count == 0)
                {
                    Debug.Log("Choosing for you");
                    lands.MoveRandomContent(temp);
                    GameObject cd = GameObject.FindWithTag("CardSelectionDisplay");
                    cd.GetComponentInChildren<DisplaySelectionCards>().SendSelectionEnd();

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
            }
        }
        
        private void GetReturnedLocation(object s, Location d)
        {
            //Debug.Log(s + " just returned to us a location with " + d.Count + " cards");
            //this.ReturnedLocation = d;
        }
        
        
}

