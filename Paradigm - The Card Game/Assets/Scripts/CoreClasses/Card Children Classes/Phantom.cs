using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;

public class Phantom : Accessor
{
    private string spawnCondText;
    private bool canSpawn;
    private Ability spawnCondition;

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
        this.spawnCondText = sp;
        this.canSpawn = false;
        this.spawnCondition = new Ability(this.getName(), spawnCondText);
    }

    public string SpawnConditionText
    {
        get { return this.spawnCondText; }
        set { this.spawnCondText = value; }
    }

    public bool CanSpawn
    {
        get { return this.canSpawn; }
        set { this.canSpawn = value; }
    }
}
