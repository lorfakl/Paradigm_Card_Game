using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Decorations { Combat, Null }
public abstract class CardDecoration
{
    protected Decorations type;
    protected Card c;

    public Decorations Type
    {
        get { return type; }
    }

    public Card Card
    {
        get { return c; }
    }

    public static CardDecoration CheckForDecoration(Card card, Decorations d)
    {
        foreach(CardDecoration decor in card.Decorations)
        {
            if(decor.Type == d)
            {
                return decor;
            }
        }

        return new NullDecoration();
    }

    //public abstract CardDecoration GetDecorator(Decorations d);
    public abstract void PerformAction();

}
