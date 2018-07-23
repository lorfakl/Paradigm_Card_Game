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
            foreach(Card c in p.GetLocation("DZ").GetContents())
            {
                if(c.GetType() == typeof(Landscape))
                {
                    chosenLand = c;
                }
            }
            p.TCCard = chosenLand;
            p.GetLocation("DZ").MoveContent(chosenLand, destination);
        }

        public static void ChooseBarriers(Player p, int barrierCount)
        {
            List<Card> barriers = new List<Card>();
            for (int i = 0; i <= barrierCount; i++)
            {
                Card c = p.PlayerDeck.SelectRandomContent();
                while(barriers.Contains(c))
                {
                    c = p.PlayerDeck.SelectRandomContent();
                }
                barriers.Add(c);
            }
            p.PlayerDeck.MoveContent(barriers, p.GetLocation("BZ"));

        }
    }
}
