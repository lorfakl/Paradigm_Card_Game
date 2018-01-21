using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Builder
{
    public static class AbilityBuilder 
    {
        private static Dictionary<string, Ability.ActivateAbility> functionDictionary = new Dictionary<string, Ability.ActivateAbility>();

        public static void SomeThing(GameEvents e)
        {
            Debug.Log("Some shit");
        }
        
        
        public static Ability.ActivateAbility CreateAbility(string cardName, string text, string abName = "")
        {

            return new Ability.ActivateAbility(SomeThing);
        }
       
    }
}
