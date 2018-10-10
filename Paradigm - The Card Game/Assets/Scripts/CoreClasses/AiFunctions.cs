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

        /// <summary>
        /// Basic AI just chooses 2 random accessors from its hand and then attacks randomly
        /// </summary>
        /// <param name="p"></param>
        public static void ChooseCentralPhaseActions(Player p)
        {
            List<Card> hand = p.GetLocation(ValidLocations.Hand).GetContents(typeof(Accessor));
            if (hand != null) //there are accessors in the AI's hand
            {
                for (int i = 0; i < 2; i++)
                {
                    if (hand != null)
                    {
                        int index = UnityEngine.Random.Range(0, hand.Count);
                        Card c = hand[index];
                        p.GetLocation(ValidLocations.Hand).MoveContent(c, p.GetLocation(ValidLocations.Field));
                        hand = p.GetLocation(ValidLocations.Hand).GetContents(typeof(Accessor));
                    }
                }

                int chance = UnityEngine.Random.Range(0, p.AttackChance);
                if (chance == p.AttackChance)
                {
                    Debug.Log("Launch an Attack!");
                }
            }
            else //there are not any accessors in the AI's hand
            {
                //skip turn?
            }
        }

        public static void ChooseCrystalPhaseActions(Player p)
        {
            Location sc = p.GetLocation("SC");
            if(sc.Count >= 3)
            {
                List<Card> shardList = sc.GetContents();
                Enum[] validLocations = new Enum[] { ValidLocations.Deck, ValidLocations.Grave, ValidLocations.BZ };
                for(int i = 0; i<3; i++)
                {
                    shardList = sc.GetContents();
                    int index = UnityEngine.Random.Range(0, shardList.Count);
                    Card c = shardList[index];
                    Location destination = p.GetLocation(validLocations[i].ToString());
                    sc.MoveContent(c, destination);
                }
            }
        }

    }
}
