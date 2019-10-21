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
    private string cardName;
    private Card cardOwner;
    private bool isEnabled;

    public delegate void ActivateTrait(GameEventsArgs e);
    ActivateTrait traitFunction;

    public ShapeTrait Shape { get; private set; }

    public string TraitText { get; private set; }

    public void LinkToCard(string n)
    {
        Card card = DataBase.CardDataBase.GetCard(n);
        this.cardOwner = card;
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
        EventManager.NotifySubsOfEvent += CheckNewEvent;
        this.TraitText = t;
        cardName = cN;
        this.isEnabled = false;
        this.Shape = ShapeTrait.None;

        foreach(var v in Enum.GetValues(typeof(ShapeTrait)))
        {
            //Debug.Log("Trait Constructor");
            //Debug.Log("Raw Text: " + this.text + "Current Enum val: " + v.ToString());

            if (this.TraitText == v.ToString())
            {
                //Debug.Log("Trait Constructor");
                //Debug.Log("Raw Text: " + this.text + "Current Enum val: " + v.ToString());
                this.Shape = (ShapeTrait)v;
            }
        }

        if(this.Shape == ShapeTrait.None)
        {
            //Debug.Log("Trait Constructor: Not a Landscape");
        }  
    }

}

