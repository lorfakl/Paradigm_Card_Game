﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    private static Stack<GameEvents> eventStack = new Stack<GameEvents>();  //there's only ever gonna be one of these
    private static Queue<GameEvents> eventQueue = new Queue<GameEvents>();

    public delegate void EventAddedHandler(object sender, GameEventsArgs data); //the delegate
    public static event EventAddedHandler CallEventAddedSubs; // an instance of the delegate

    private void OnEventAdd (object sender, GameEventsArgs data)
    {
        if(CallEventAddedSubs != null) //if there are subcribers
        {
            CallEventAddedSubs(this, data);
        }
    }
    /// <summary>
    /// This function is used to add GameEvents to the queue, more logic is needed to differeniate GameEvents from one another
    /// Which will lead into adding things to the stack and doing timer things 
    /// </summary>
    /// <param name="e"></param>
    public void EnqueueEvent(GameEvents e) 
    {                                                   
        eventQueue.Enqueue(e);
        OnEventAdd(this, new GameEventsArgs());
    }

    
    
}
