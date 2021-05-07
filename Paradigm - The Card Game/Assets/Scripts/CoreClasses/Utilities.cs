using System;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using TransportLayer;
using System.Reflection;

/// <summary>
/// The utilities class is for storing the general purpose commonly used functions and data structures 
/// </summary>
namespace Utilities
{
    
    public static class HelperFunctions
    {
        private static string conn = "URI=file:" + Application.dataPath + "/CardDataBase.db";
        #region General Instantiation Functions
        /// <summary>
        /// TODO ADD A FILTER TO CHOOSE WHICH CARDS ARE DISPLAYED(most work to be 
        /// done in DisplaySelectionCards.cs)
        /// This is the simplife function call to display a list of cards to the 
        /// player so that they can select x amount of cards and those cards are 
        /// sent to the destination to be process or whatever
        /// </summary>
        /// <param name="source">The location object the cards are being taken from</param>
        /// <param name="destination">The location object the cards are being place in</param>
        /// <param name="numToSelect">The number of cards the user is allowed to select</param>
        public static GameObject SelectCards(Location source, Location destination, int numToSelect)
        {
            GameObject display = GameObject.Instantiate(Resources.Load("DisplayOverlay")) as GameObject;
            if(source.Owner.PlayerID == destination.Owner.PlayerID)
            {
                Debug.Log("DSC Instantiation Success");
            }
            display.GetComponentInChildren<DisplaySelectionCards>().SetCardPath(source, destination, numToSelect);
            return display;
        }

       

        /// <summary>
        /// This function Instantiates a Card prefab for you
        /// </summary>
        /// <param name="c">Card object that contains the data the Gameobject needs to have</param>
        /// <param name="inDisplayMode">Boolean to choose whether this GameObject is in DisplaySelection mode</param>
        /// <param name="parent">Transform of the parent</param>
        /// <param name="cardPrefab">Optional: If you have a prefab reference or nah</param>
        /// <returns>GameOject so that the number of objects created can be kept by the caller</returns>
        public static GameObject CreateCard(Card c, bool inDisplayMode, Transform parent, bool isfaceDown = false, GameObject cardPrefab = null)
        {
            GameObject cardObject = null; 
            if (cardPrefab == null)
            {
                cardObject = GameObject.Instantiate(Resources.Load("Card", typeof(GameObject)), parent) as GameObject;

                if (isfaceDown)
                {
                    cardObject.transform.Find("cardBack").GetComponent<SpriteRenderer>().gameObject.SetActive(true);
                    cardObject.transform.Find("cardtemplate").GetComponent<SpriteRenderer>().gameObject.SetActive(false);
                }
                
               
                
            }
            else
            {
                cardObject = GameObject.Instantiate(cardPrefab, parent) as GameObject;
            }
           
            cardObject.SendMessage("SetMode", inDisplayMode);
            cardObject.GetComponent<CardScript>().SetCard(c);
            Transform cardName = cardObject.transform.FindDeepChild("cardName");
            cardName.GetComponent<Text>().text = c.Name;
            Transform content = cardObject.transform.FindDeepChild("Content");
            content.GetComponent<Text>().text = c.GetAbilityText();
            if (c == null)
            {
                throw new Exception("The Card's null dumbass!(CreateCard)");
            }
            //position = cardObject.transform.position;
            c.GameObj = cardObject;

            return cardObject;
        }

        public static void RaiseNewUIEvent(Action action, EventType stackNotification)
        {
            throw new NotImplementedException();
        }

        public static void ScaleCard(GameObject c, Vector3 scale)
        {
            RectTransform rectTrans = c.GetComponent<RectTransform>();
            

            rectTrans.localScale = scale;
        }

        public static void FlipCard(GameObject c)
        {
            if(c.GetComponent<CardScript>() == null)
            {
                Debug.LogWarning("GameObjects that are not Cards can not be passed into this function");
                return;
            }

            c.transform.Find("cardBack").GetComponent<SpriteRenderer>().gameObject.SetActive(false);
            c.transform.Find("cardtemplate").GetComponent<SpriteRenderer>().gameObject.SetActive(true);
        }

        public static MonoBehaviour AccessMonoBehaviour()
        {
            GameObject monoAccess = GameObject.FindWithTag("MonobehaviourAccessor");
            return monoAccess.GetComponent<MonoBehaviourAccessor>();
        }

        /// <summary>
        /// Raise a new event that is the direct result of a card effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="boardChanges"></param>
        /// <param name="source"></param>
        /// <param name="moveAction"></param>
        /// <param name="notMoveAction"></param>
        /// <param name="cardTargets"></param>
        #endregion

        #region Raise Event Overloads

        public static void RaiseNewEvent(object sender, List<LocationChanges> boardChanges, Card source, MoveAction moveAction,
                                NonMoveAction notMoveAction, List<Card> cardTargets)
        {
            GameEventsArgs newEvent = new GameEventsArgs(boardChanges, source, moveAction, notMoveAction, cardTargets);
            EventIngestion.EventIntake(sender, newEvent);
        }

        /// <summary>
        /// Raise a new event that is not related to a specific card but instead its a core game event like a turn phase transition or dimension twist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="owner"></param>
        /// <param name="target"></param>
        public static GameEventsArgs RaiseNewEvent(object sender, Player owner, Player target, GameAction action)
        {
            GameEventsArgs newEvent = new GameEventsArgs(owner, target, action);
            UiEvents uiEvents = new UiEvents(owner, target, action);
            //EventIngestion.EventIntake(sender, newEvent);
            EventIngestion.EventIntake(sender, uiEvents);
            return newEvent;
        }

        public static void RaiseNewEvent(object sender, Card origin, ValidLocations source, ValidLocations destination, MoveAction moveAction, Card target)
        {
            GameEventsArgs newEvent = new GameEventsArgs(origin, source, destination, moveAction, target);
            EventIngestion.EventIntake(sender, newEvent);
        }

        /// <summary>
        /// Raise a new event that is directly related to Card movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="boardChanges"></param>
        /// <param name="moveAction"></param>
        public static void RaiseNewEvent(object sender, List<LocationChanges> boardChanges, MoveAction moveAction)
        {
            GameEventsArgs newEvent = new GameEventsArgs(boardChanges, moveAction);
            EventIngestion.EventIntake(sender, newEvent);
        }

        /// <summary>
        /// Raise an event to specifically check if the action is legal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="nonMoveAction"></param>
        /// <param name="eventType"></param>
        /// <param name="source"></param>
        public static void RaiseNewEvent(object sender, GameAction action, EventType eventType, Card source)
        {
            Print("Needs to go to Transport Layer then LegalityCheck In GameManager");
        }

        public static void RaiseNewEvent(object sender, Card cardSource, NonMoveAction notMoveAction, Card cardTarget)
        {
            GameEventsArgs newEvent = new GameEventsArgs(cardSource, notMoveAction, cardTarget);
            EventIngestion.EventIntake(sender, newEvent);
        }

        public static void RaiseNewUIEvent(object sender, ValidLocations source, ValidLocations destination, MoveAction moveAction, Card c)
        {
            UiEvents uiEvent = new UiEvents(source, destination, moveAction, c);
            EventIngestion.EventIntake(sender, uiEvent);
        }

        /// <summary>
        /// This Raise event overload is specifically for Initate Events, it requires the 
        /// Internal Initate details containing the actions taken and targets involved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="initiateEventInfo"></param>
        /// <param name="initiatingCard"></param>
        public static void RaiseNewEvent(object sender, GameEventsArgs initiateEventInfo, Card initiatingCard)
        {
            GameEventsArgs newEvent = new GameEventsArgs(initiateEventInfo.EventOwner, initiateEventInfo.PlayerTarget,
                    initiateEventInfo.CardTargets, new GameAction(MoveAction.None, NonMoveAction.Initiate, initiateEventInfo.TurnPhase),
                    EventType.Gameplay);
            newEvent.EventOriginCard = initiatingCard;
            newEvent.InitiateEventDetails = initiateEventInfo;
            EventIngestion.EventIntake(sender, newEvent);
        }
        #endregion

        #region Generate Return Event Overloads

        public static GameEventsArgs GenerateReturnEvent(Player owner, Player target, List<Card> cardTargets, GameAction action, EventType type)
        {
            GameEventsArgs e = new GameEventsArgs(owner, target, cardTargets, action, type);
            return e;
        }

        

        public static GameEventsArgs GenerateReturnEvent(Card source, NonMoveAction nonMove, Card targetCard)
        {
            GameEventsArgs e = new GameEventsArgs(source, NonMoveAction.DeclaredAttack, targetCard);
            return e;
        }
        #endregion

        #region Debug Utility Functions

        public static void Error(string msg)
        {
            throw new Exception(msg);
        }

        public static void Print(string msg)
        {
            Debug.Log(msg);
        }

        public static void CatchException(Exception e)
        {
            Debug.LogWarning(e.Source);
            Debug.LogWarning(e.Message);
            Debug.LogWarning(e.StackTrace);
            Debug.LogWarning(e.InnerException);
        }

        public static void PrintObjectProperties<T>(T src)
        {
            Type type = typeof(T);

            PropertyInfo[] propertyInfo = type.GetProperties();

            foreach (PropertyInfo pInfo in propertyInfo)
            {
                string val = type.GetProperty(pInfo.Name)?.GetValue(src, null)?.ToString();
                if(!String.IsNullOrEmpty(val))
                {
                    Print(pInfo.Name + ": " + val);
                }
                    

            }
        }
        #endregion

        #region Misc

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static Player GetReferenceToOtherPlayer(Player p, GameEventsArgs e)
        {
            if(p.ID != e.EventOwner.ID)
            {
                return e.EventOwner;
            }
            else if(p.ID != e.PlayerTarget.ID)
            {
                return e.PlayerTarget;
            }
            else
            {
                Print("ID of the Known Player: " + p.ID);
                e.Print();
                throw new Exception("There was not a different Player reference found in event \n " +
                    "This means that there is an issue in the assignment of the events somewhere \n" +
                    "This is a MAJOR bug and need immediate attention");

            }
        }
        #endregion
    }


    public static class Dictionaries
    {
        //this static class will be responsible for mmapping card names to their abilities and traits
        //which map to the functions that actually do the things for abilities and traits
        private static bool arePrepared = false;
        
        
        public static bool Prepared
        {
            get { return arePrepared; }
        }

        public static void PrepareAllDictionaries()
        {


            arePrepared = true;

        }

        
    }
}
