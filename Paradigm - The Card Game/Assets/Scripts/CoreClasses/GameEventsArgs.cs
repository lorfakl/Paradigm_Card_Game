using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameEventsArgs: EventArgs
{
    public readonly string owner;
    public readonly string target;
    public readonly string source;

    public GameEventsArgs()
    {
        this.owner = "This";
        this.target = "Issa";
        this.source = "Test";
        Debug.Log("Event Data Created!");
    }

    public GameEventsArgs(string p, string p2, string c)
    {
        this.owner = p;
        this.target = p2;
        this.source = c;

        Debug.Log("Event Data Created!");
    }
   
}
