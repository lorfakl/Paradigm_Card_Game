using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The utilities class is for storing the general purpose commonly used functions and data structures 
/// </summary>
namespace Utilities
{
    public static class HelperFuncs
    {
        public static GameEventsArgs CreateGameEvent()
        {

            
        }
    }

    public static class Dictionaries
    {
        //this static class will be responsible for mmapping card names to their abilities and traits
        //which map to the functions that actually do the things for abilities and traits
        private static bool arePrepared = false;
        public static Dictionary<string, Turn.TurnPhaseFunction> turnDict = new Dictionary<string, Turn.TurnPhaseFunction>();
       
        
        public static bool Prepared
        {
            get { return arePrepared; }
        }

        public static void PrepareDictionaries()
        {

            arePrepared = true;
        }

        
    }

    static class CardFunctions
    {
        private static void StartElementalEffect(GameEvents e)
        {

        }
    }
}
