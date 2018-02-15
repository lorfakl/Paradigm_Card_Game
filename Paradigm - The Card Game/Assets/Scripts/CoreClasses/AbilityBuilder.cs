using System;
using System.Collections;
using System.Collections.Generic;
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
    public static class AbilityBuilder 
    {
        private static Dictionary<string, Ability.ActivateAbility> functionDictionary = new Dictionary<string, Ability.ActivateAbility>();

        private static void SomeThing(GameEventsArgs e)
        {
            
            Debug.Log("Some shit");
        }
        
        
        public static Ability.ActivateAbility CreateAbility(string cardName, string text, string abName = "")
        {

            return new Ability.ActivateAbility(SomeThing);
        }
       
    }
}
