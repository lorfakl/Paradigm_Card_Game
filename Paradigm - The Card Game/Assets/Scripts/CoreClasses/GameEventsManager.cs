using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    private static Stack<GameEventsArgs> eventStack = new Stack<GameEventsArgs>();  //there's only ever gonna be one of these
    private static Queue<GameEventsArgs> eventQueue = new Queue<GameEventsArgs>();
    //private static Landscape activeLand IMPLEMENT LANDSCAPES IN THE DATABASE
    public delegate void EventAddedHandler(object sender, GameEventsArgs data); //the delegate
    public static event EventAddedHandler NotifySubsOfEvent; // an instance of the delegate only ever gonna be one

    private static void OnEventAdd (object sender, GameEventsArgs data)
    {
        if(NotifySubsOfEvent != null) //if there are subcribers
        {
            NotifySubsOfEvent(sender,data);
        }
    }

    /// <summary>
    /// This function is used to add GameEvents to the queue, more logic is needed to differeniate GameEvents from one another
    /// Which will lead into adding things to the stack and doing timer things 
    /// </summary>
    /// <param name="e"></param>
    public static void PublishEvent(object s, GameEventsArgs e) 
    {                                                   
        eventQueue.Enqueue(e);
        OnEventAdd(s,e);
    }

    /// <summary>
    /// GameEventManager will end up attached to an empty gameobject when the game starts to well...manage game events
    /// Thats why it extends Monobehaviour and has Update and Start functions
    /// </summary>
    void Start()
    {
        //Player p1 = new Player(new Deck());
        //Player p2 = new Player(new Deck());
    }

    // Update is called once per frame
    void Update()
    {
    
    }

}
