using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ShapeTrait
{
    Circle, Square, Triangle, None
}

public class Trait
{
    private string text;
    private string cardName;
    private bool isEnabled;
    private ShapeTrait shape;
    public delegate void ActivateTrait(GameEvents e);
    ActivateTrait traitFunction;

    public ShapeTrait Shape
    {
        get { return this.shape; }
    }

    public string Text
    {
        get { return this.text; }
    }

    private void CheckNewEvent(object sender, GameEventsArgs e)
    {
        if (this.isEnabled)
        {
            Debug.Log("I'm an ability, and I know a new game event was added to the queue");
        }

    }

    public Trait(string t, string cN)
    {
        GameEventsManager.NotifySubsOfEvent += CheckNewEvent;
        this.text = t;
        this.cardName = cN;
        this.isEnabled = false;
        this.shape = ShapeTrait.None;
        foreach(var v in Enum.GetValues(typeof(ShapeTrait)))
        {
            //Debug.Log("Trait Constructor");
            //Debug.Log("Raw Text: " + this.text + "Current Enum val: " + v.ToString());

            if (this.text == v.ToString())
            {
                Debug.Log("Trait Constructor");
                Debug.Log("Raw Text: " + this.text + "Current Enum val: " + v.ToString());
                this.shape = (ShapeTrait)v;
            }
        }

        if(this.shape == ShapeTrait.None)
        {
            Debug.Log("Trait Constructor: Not a Landscape");
        }



        
    }

}

