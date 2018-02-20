using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The utilities class is for storing the general purpose commonly used functions and data structures 
/// </summary>
namespace Utilities
{
    

    public static class HelperFunctions
    {
        /// <summary>
        /// Raise a new event that is the direct result of a card effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="boardChanges"></param>
        /// <param name="source"></param>
        /// <param name="moveAction"></param>
        /// <param name="notMoveAction"></param>
        /// <param name="cardTargets"></param>
        public static void RaiseNewEvent(object sender, List<LocationChanges> boardChanges, Card source, MoveAction moveAction,
                                NonMoveAction notMoveAction, List<Card> cardTargets)
        {
            GameEventsArgs newEvent = new GameEventsArgs(boardChanges, source, moveAction, notMoveAction, cardTargets);
            GameEventsManager.PublishEvent(sender, newEvent);
        }

        /// <summary>
        /// Raise a new event that is not related to a specific card but instead its a core game event like a turn phase transition or dimension twist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="owner"></param>
        /// <param name="target"></param>
        public static GameEventsArgs RaiseNewEvent(object sender, Player owner, Player target, NonMoveAction nonMoveAction = NonMoveAction.None)
        {
            GameEventsArgs newEvent = new GameEventsArgs(owner, target, nonMoveAction);
            GameEventsManager.PublishEvent(sender, newEvent);
            return newEvent;
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
            GameEventsManager.PublishEvent(sender, newEvent);
        }

        public static void RaiseNewEvent(object sender, Card cardSource, NonMoveAction notMoveAction, Card cardTarget)
        {
            GameEventsArgs newEvent = new GameEventsArgs(cardSource, notMoveAction, cardTarget);
            GameEventsManager.PublishEvent(sender, newEvent);
        }

        
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
