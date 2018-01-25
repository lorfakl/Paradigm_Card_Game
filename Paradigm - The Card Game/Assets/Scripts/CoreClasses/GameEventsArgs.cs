using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum MoveAction
{
    Break, Build, Collect, Crystallize, Delete, Despawn, Draw, Lock, Rest, Return, Spawn, Unlock
}

public class GameEventsArgs: EventArgs
{
    public readonly string owner;
    public readonly string target;
    public readonly string source;
    public readonly Player p;
    public readonly Turn t;
    public readonly Location.LocationChanges boardChanges;
    public readonly MoveAction action;


    public GameEventsArgs()
    {
        
        Debug.Log("Event Data Created!");
    }

    
   
}
