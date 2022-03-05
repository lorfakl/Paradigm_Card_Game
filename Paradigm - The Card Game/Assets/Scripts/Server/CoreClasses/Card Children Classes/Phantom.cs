using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;

public class Phantom : Accessor
{
    private string spawnCondText;
    

    public Phantom(string n, string k, System.Int64 p, System.Int64 h, string t, string a, string a2, string a3, string sp)
        :base(n, k, p, h, t, a, a2, a3)
    {
        this.spawnCondText = sp;
        this.canSpawn = false;
    }

    public string SpawnConditionText
    {
        get { return this.spawnCondText; }
        set { this.spawnCondText = value; }
    }

    
}
