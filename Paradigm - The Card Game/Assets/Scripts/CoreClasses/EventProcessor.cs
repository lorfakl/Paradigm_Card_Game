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
        
    }

    private bool areDictsPrepared;
    private Dictionary<String, Type> senderDictionary = new Dictionary<string, Type>();
    private Dictionary<Type, Dictionary<String, Func<ParameterBundle, bool>>> senderFunctionPoolDictionary = new Dictionary<Type, Dictionary<String, Func<ParameterBundle, bool>>>();
    private Dictionary<String, Func<ParameterBundle, bool>> functionDictionary = new Dictionary<string, Func<ParameterBundle, bool>>();

    private void Awake()
    {
        GameEventsManager.NotifySubsOfEvent += ProcessEvent;
        if(senderDictionary.Count < 1)
        {
            areDictsPrepared = false;
            PrepareDictionaries();
            areDictsPrepared = true;
        }
    }

    public void ProcessEvent(object s, GameEventsArgs e)
    {
        
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
        senderDictionary.Add("Ability", typeof(Ability));
        senderDictionary.Add("Accessor", typeof(Accessor));
        senderDictionary.Add("Action", typeof(Action));
        senderDictionary.Add("Block", typeof(Block));
        senderDictionary.Add("Condition", typeof(Condition));
        senderDictionary.Add("Cost", typeof(Cost));
        senderDictionary.Add("Deck", typeof(Deck));
        senderDictionary.Add("Element", typeof(Element));
        senderDictionary.Add("Family", typeof(Family));
        senderDictionary.Add("Landscape", typeof(Landscape));
        senderDictionary.Add("Location", typeof(Location));
        senderDictionary.Add("Majesty", typeof(Majesty));
        senderDictionary.Add("Mechanism", typeof(Mechanism));
        senderDictionary.Add("Phantom", typeof(Phantom));
        senderDictionary.Add("Philosopher", typeof(Philosopher));
        senderDictionary.Add("Source", typeof(Source));;
        senderDictionary.Add("Trait", typeof(Trait));
        senderDictionary.Add("Turn", typeof(Turn));
        //senderDictionary.Add("Ability", typeof(Ability));
    }
    //END DICTIONARY PREP FUNCTIONS

    private static void DrawEvent(GameEventsArgs e)
    {

    }
}

