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
            int index = UnityEngine.Random.Range(0, 2);
            Location temp = p.PlayerDeck.GetLandsAsLocation();
            Card chosenLand = temp.GetContents()[index];
            temp.MoveContent(chosenLand, destination);
        }
    }
}
