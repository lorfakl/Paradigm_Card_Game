using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Builder
{
    public static class AbilityBuilder 
    {
        
        public static AbilityFunctionality CreateAbility(string cardName, string text, string abName = "")
        {

            return new AbilityFunctionality();
        }
       
    }
}
