using System;
using System.Collections;
using HelperFunctions;
using System.Reflection;
using System.Collections.Generic;
using System.Security.Permissions;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine;

namespace Builder
{
    public static class AbilityBuilder
    {
        public delegate bool ConditionCheck(GameEventsArgs e, Card a, string mod = "");
        private static Dictionary<string, ConditionCheck> ConditionDict = new Dictionary<string, ConditionCheck>();

        public static void AddCondition(string key, ConditionCheck value)
        {
            ConditionDict.Add(key, value);
            ConditionDict.Add("trash", ThisActive);
        }

        public static ConditionCheck GetConditionCheck(string key)
        {
            return ConditionDict[key];
        }

        public static bool ThisActive(GameEventsArgs e, Card a, string m = "")
        {
            if (e.EventOriginCard == a)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ThisOwnerTurnEnd(GameEventsArgs e, Card a, string m = "")
        {
            if (e.ActionEvent == NonMoveAction.TurnPhase && e.EventOwnerTurn == TurnPhase.End)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Despawned(GameEventsArgs e, Card c, string m = "")
        {
            if(e.MoveActionEvent == MoveAction.Despawn)
            {
                if(e.TargetCard.Name.Contains(m))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ThisTurn(GameEventsArgs e, Card c, string m = "")
        {
            if (e.ActionEvent == NonMoveAction.TurnPhase && e.EventOwnerTurn == TurnPhase.Start)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Spawned(GameEventsArgs e, Card c, string m = "")
        {

            if (e.MoveActionEvent == MoveAction.Spawn)
            {
                if (m == "") //if the condition is This Card Spawned
                {
                    if(e.TargetCard.Name == c.Name)
                    {
                        return true;
                    }
                }
                else if(m.Contains("not This.Owner;")) //anything just not owned by the player that owns the condition being checked
                {
                    return true;
                }
                else if(e.TargetCard.Name.Contains(m)) //if the spawn is something specific
                {
                    return true;
                }
            }
            return false;
        }


        public static bool TurnGatherPhase(GameEventsArgs e, Card c, string m = "")
        {
            if (e.ActionEvent == NonMoveAction.TurnPhase && e.EventOwnerTurn == TurnPhase.Gather)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ActivatedElementTrait(GameEventsArgs e, Card c, string m = "")
        {
            string[] possibleTraits = { "Earth", "Fire", "Water", "Wind"}; //change to search a file containing ALL traits
            List<string> traitPool = new List<string>(possibleTraits);

            if(traitPool.Contains(m))
            {
                if(e.ActionEvent == NonMoveAction.Activate)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool GameStateIsDamageCalculation(GameEventsArgs e, Card c, string m = "")
        {
            //IMPLEMENT A GAME STATE SYSTEM THING
            //if(Game.State == GameState.DamageCalc)
            //{
                return false;
            //}
        }

        public static bool CheckLocation(GameEventsArgs e, Card c, string m = "")
        {
            //IMPLEMENT A HASH THAT HOLDS ALL THE EXTRA BITS FOR THINGS
            return false;
        }

        /*
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")
        public static bool (GameEventsArgs e, Card c, string m = "")*/
    }
}
