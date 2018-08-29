using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;

/// <summary>
/// The purpose of this class is to processor the card events as they are published to the event queue.
/// Once received the events will be parsed and translated into actions this include animations, sound, etc
/// </summary>
public class EventProcessor: MonoBehaviour
{
    private struct ParameterBundle
    {
        public String functionID;
    }

    private bool areDictsPrepared;
    //each type will map to a dictionary containing a string id key with a function value
    private Dictionary<Type, Dictionary<String, Func<ParameterBundle, bool>>> senderFunctionPoolDictionary = new Dictionary<Type, Dictionary<String, Func<ParameterBundle, bool>>>();
    //not sure what the functions should return if anything so right now they return a bool it might be placeholder
    private Dictionary<String, Func<ParameterBundle, bool>> functionDictionary = new Dictionary<string, Func<ParameterBundle, bool>>();

    private void Awake()
    {
        GameEventsManager.NotifySubsOfEvent += ProcessEvent;
        if(senderFunctionPoolDictionary.Count < 1)
        {
            areDictsPrepared = false;
            PrepareDictionaries();
            areDictsPrepared = true;
        }
    }

    public void ProcessEvent(object s, GameEventsArgs e)
    {
        ExecuteFunction(senderFunctionPoolDictionary[s.GetType()], e);
    }

    /// <summary>
    /// BEGIN DICTIONARY PREPARE FUNCTIONS
    /// Effectivly this section just adds all the dictionary key value pairs to all the dictionaries that will be used for
    /// Event Processing
    /// </summary>
    private void PrepareDictionaries()
    {
        PrepareSenderDict();
    }

    private void PrepareLocationDict()
    {

    }

    private void PrepareSenderDict()
    {
      
    }
    //END DICTIONARY PREP FUNCTIONS

    private void ExecuteFunction(Dictionary<String, Func<ParameterBundle, bool>> functionDict, GameEventsArgs e)
    {
        ParameterBundle parameters = ParseEvent(e);
        Func<ParameterBundle, bool> function = functionDict[parameters.functionID];
        function(parameters);

    }

    private ParameterBundle ParseEvent(GameEventsArgs e)
    {
        ParameterBundle p = new ParameterBundle();

        return p;
    }

    private static void DrawEvent(GameEventsArgs e)
    {

    }
}

