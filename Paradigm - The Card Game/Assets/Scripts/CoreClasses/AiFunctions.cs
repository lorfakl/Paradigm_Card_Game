using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// Contains the functions for the AI to make decisions. Right now issa dumb. But later its gonna upgrade thing of 
    /// using some GOP
    /// </summary>
    public static class AiFunctions
    {
        public static void ChooseTCCard(Player p, Location destination)
        {
            Card chosenLand = null;
            int index = UnityEngine.Random.Range(0, 2);
            foreach(Card c in p.GetLocation(ValidLocations.DZ).GetContents())
            {
                if(c.GetType() == typeof(Landscape))
                {
                    chosenLand = c;
                }
            }
            p.TCCard = chosenLand;
            p.GetLocation(ValidLocations.DZ).MoveContent(chosenLand, destination);
        }

        public static void ChooseBarriers(Player p, int barrierCount)
        {
           

        }

        /// <summary>
        /// Basic AI just chooses 2 random accessors from its hand and then attacks randomly
        /// </summary>
        /// <param name="p"></param>
        public static void ChooseCentralPhaseActions(Player p)
        {
            
        }

        public static void ChooseCrystalPhaseActions(Player p)
        {
           
        }

    }
}
