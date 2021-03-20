using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;


/// <summary>
/// The purpose of this class is to process the card events as they are published to the event queue.
/// A Note about Abilties: The abilities need to be encoded to contain the function or series of functions
/// that need to be called and the required parameters for the effect to take place
/// Once received the events will be parsed and translated into actions this include animations, sound, etc
/// </summary>
public class EventProcessor: MonoBehaviour
{
    private struct ParameterBundle
    {
        public String functionID;
        public EventType type;

    }

    
    /// <summary>
    /// There should be only one dictionary, the game shouldnt care what object generated the event.
    /// This dictionary should map a specific ID to a specific function this ID should also be used when
    /// abilities wish to call said function
    /// </summary>
    private Dictionary<String, Func<ParameterBundle, bool>> functionDictionary = new Dictionary<string, Func<ParameterBundle, bool>>();
    private bool areDictsPrepared;

    /*private void Awake()
    {
        GameEventsManager.NotifySubsOfEvent += ProcessEvent;
        if(functionDictionary.Count < 1)
        {
            areDictsPrepared = false;
            PrepareDictionaries();
            areDictsPrepared = true;
        }
    }

    public void ProcessEvent(object s, GameEventsArgs e)
    {
        ExecuteFunction(e);
    }

    private void PrepareDictionaries()
    {
        //Prepare the function dictionary
    }

    private void ExecuteFunction(GameEventsArgs e)
    {
        return;
        ParameterBundle parameters = ParseEvent(e);
        Func<ParameterBundle, bool> function = functionDictionary[parameters.functionID];
        function(parameters);
    }

    private ParameterBundle ParseEvent(GameEventsArgs e)
    {
        ParameterBundle p = new ParameterBundle();

        return p;
    }

    private static void DrawEvent(GameEventsArgs e)
    {

    }*/
}

