using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Phantom : Accessor
{
    private Ability spawnCondition = null;

    public Phantom(string n, string k, System.Int64 p, System.Int64 h, string t, string a, string a2, string a3, string sp)
        :base(n, k, p, h, t, a, a2, a3)
    {
        if(a3 != "" && sp == "")
        {
            sp = a3;
            RemoveAttribute("a");
        }
        else if(a2 != "" && sp == "")
        {
            sp = a2;
            RemoveAttribute("a");
        }
        this.SpawnConditionText = sp;
        this.CanSpawn = false;
        this.spawnCondition = new Ability(this.getName(), SpawnConditionText);
    }

    public string SpawnConditionText { get; set; }

    public bool CanSpawn { get; set; }
}
