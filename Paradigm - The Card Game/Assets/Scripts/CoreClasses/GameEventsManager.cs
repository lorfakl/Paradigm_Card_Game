using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    private static Stack<GameEventsArgs> eventStack = new Stack<GameEventsArgs>();  //there's only ever gonna be one of these
    private static Queue<GameEventsArgs> eventQueue = new Queue<GameEventsArgs>();

    public delegate void EventAddedHandler(object sender, GameEventsArgs data); //the delegate
    public static event EventAddedHandler NotifyEventAddedSubs; // an instance of the delegate only ever gonna be one

    private void OnEventAdd (object sender, GameEventsArgs data)
    {
        if(NotifyEventAddedSubs != null) //if there are subcribers
        {
            NotifyEventAddedSubs(this, data);
        }
    }

    /// <summary>
    /// This function is used to add GameEvents to the queue, more logic is needed to differeniate GameEvents from one another
    /// Which will lead into adding things to the stack and doing timer things 
    /// </summary>
    /// <param name="e"></param>
    public void PublishEvent(GameEventsArgs e) 
    {                                                   
        eventQueue.Enqueue(e);
        OnEventAdd(this, e);
    }

    /// <summary>
    /// GameEventManager will end up attached to an empty gameobject when the game starts to well...manage game events
    /// Thats why it extends Monobehaviour and has Update and Start functions
    /// </summary>
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
