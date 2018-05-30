using System;
using System.Collections;
using Utilities;
using System.Reflection;
using System.Collections.Generic;
using System.Security.Permissions;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine;

namespace Builder
{
    /// <summary>
    /// This class will call out to the database and grab the encoded ability, then this class will parse that encoded ability to make the proper 
    /// Action and Condition objects. These objects will be sent back to the Ability object instance that called CreateAbility, the Action and 
    /// Condition objects will be attached to the Ability object instance. In the Ability class is where the SomeThing function will be moved and
    /// renamed to ActivateAbility
    /// </summary>
    public class AbilityBuilder
    {
        private static Dictionary<string, Ability.ActivateAbility> functionDictionary = new Dictionary<string, Ability.ActivateAbility>();

        public AbilityBuilder()
        {

        }

        /// <summary>
        /// This function will called from CardDataBase to prepare the dictionary for creating the Card abilities
        /// </summary>
        public void PrepareDictionary()
        {
            //functionDictionary.Add()
            Dictionary<int, List<System.Object>> tableData = HelperFunctions.AccessDataBaseTable("AbilityEncodes");
            for (int i = 0; i < tableData.Count; i++)
            {
                List<System.Object> record = tableData[i];
                //Change the abilityencodes table to have an ID, AbilityText, FunctionName
                //throw new Exception("Change the abilityencodes table to have an ID, AbilityText, FunctionName");
                string name = record[2].ToString();
                MethodInfo method = this.GetType().GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
                functionDictionary.Add(tableData[0].ToString(), //first half get the ID
                    (Ability.ActivateAbility)Delegate.CreateDelegate(typeof(Ability.ActivateAbility), method)); //cast the method into the proper
                                                                                                                //delegate type
            }
        }

        private static void SomeThing(GameEventsArgs e)
        {

            Debug.Log("Some shit");
        }


        public static Ability.ActivateAbility CreateAbility(string cardName, string text, string abName = "")
        {
            return new Ability.ActivateAbility(SomeThing); //placeholder as fuck
        }

        private static void Move()
        {

        }


/*
        private static void AbilityID1A(GameEventsArgs e)
        {

        }

        private static void AbilityID1B(GameEventsArgs e)
        {

        }


        private static void AbilityID2A(GameEventsArgs e) { }
        private static void AbilityID2B(GameEventsArgs e) { }
        private static void AbilityID3A(GameEventsArgs e) { }
        private static void AbilityID3B(GameEventsArgs e) { }
        private static void AbilityID4A(GameEventsArgs e) { }
        private static void AbilityID4B(GameEventsArgs e) { }
        private static void AbilityID5A(GameEventsArgs e) { }
        private static void AbilityID5B(GameEventsArgs e) { }
        private static void AbilityID6A(GameEventsArgs e) { }
        private static void AbilityID6B(GameEventsArgs e) { }
        private static void AbilityID7A(GameEventsArgs e) { }
        private static void AbilityID7B(GameEventsArgs e) { }
        private static void AbilityID8A(GameEventsArgs e) { }
        private static void AbilityID8B(GameEventsArgs e) { }
        private static void AbilityID9A(GameEventsArgs e) { }
        private static void AbilityID9B(GameEventsArgs e) { }
        private static void AbilityID10A(GameEventsArgs e) { }
        private static void AbilityID10B(GameEventsArgs e) { }
        private static void AbilityID11A(GameEventsArgs e) { }
        private static void AbilityID12A(GameEventsArgs e) { }
        private static void AbilityID13A(GameEventsArgs e) { }
        private static void AbilityID14A(GameEventsArgs e) { }
        private static void AbilityID15A(GameEventsArgs e) { }
        private static void AbilityID15B(GameEventsArgs e) { }
        private static void AbilityID16A(GameEventsArgs e) { }
        private static void AbilityID16B(GameEventsArgs e) { }
        private static void AbilityID17A(GameEventsArgs e) { }
        private static void AbilityID17B(GameEventsArgs e) { }
        private static void AbilityID18A(GameEventsArgs e) { }
        private static void AbilityID18B(GameEventsArgs e) { }
        private static void AbilityID19A(GameEventsArgs e) { }
        private static void AbilityID19B(GameEventsArgs e) { }
        private static void AbilityID20A(GameEventsArgs e) { }
        private static void AbilityID20B(GameEventsArgs e) { }
        private static void AbilityID21A(GameEventsArgs e) { }
        private static void AbilityID21B(GameEventsArgs e) { }
        private static void AbilityID22A(GameEventsArgs e) { }
        private static void AbilityID22B(GameEventsArgs e) { }
        private static void AbilityID23A(GameEventsArgs e) { }
        private static void AbilityID24A(GameEventsArgs e) { }
        private static void AbilityID25A(GameEventsArgs e) { }
        private static void AbilityID26A(GameEventsArgs e) { }
        private static void AbilityID27A(GameEventsArgs e) { }
        private static void AbilityID28A(GameEventsArgs e) { }
        private static void AbilityID29A(GameEventsArgs e) { }
        private static void AbilityID30A(GameEventsArgs e) { }
        private static void AbilityID31A(GameEventsArgs e) { }
        private static void AbilityID32A(GameEventsArgs e) { }
        private static void AbilityID33A(GameEventsArgs e) { }
        private static void AbilityID34A(GameEventsArgs e) { }
        private static void AbilityID35A(GameEventsArgs e) { }
        private static void AbilityID36A(GameEventsArgs e) { }
        private static void AbilityID37A(GameEventsArgs e) { }
        private static void AbilityID38A(GameEventsArgs e) { }
        private static void AbilityID39A(GameEventsArgs e) { }
        private static void AbilityID40A(GameEventsArgs e) { }
        private static void AbilityID41A(GameEventsArgs e) { }
        private static void AbilityID42A(GameEventsArgs e) { }
        private static void AbilityID42B(GameEventsArgs e) { }
        private static void AbilityID43A(GameEventsArgs e) { }
        private static void AbilityID43B(GameEventsArgs e) { }
        private static void AbilityID44A(GameEventsArgs e) { }
        private static void AbilityID44B(GameEventsArgs e) { }
        private static void AbilityID45A(GameEventsArgs e) { }
        private static void AbilityID45B(GameEventsArgs e) { }
        */
    }
}
