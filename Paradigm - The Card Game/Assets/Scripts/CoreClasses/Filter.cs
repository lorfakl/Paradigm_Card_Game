using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HelperFunctions;
/// <summary>
/// THIS CLASS IS ON HOLD RIGHT NOW IT COULD BE A GOOD IDEA IN THE FUTURE BUT NOT RIGHT NOW
/// YOU CAN DO A SWITCH INSIDE A SWITCH SO THATS COOL
/// </summary>
public enum FilterType
{
   Name, Trait, Type, Status, DataType, Power, HP, Family, Ownership
}

public enum LogicType
{
    Is, IsNot, Contains, LessThan, GreaterThan
}

/// <summary>
/// The Filter class is used to filter out cards that we dont want the player to be able to select
/// </summary>
public class Filter
{
    public Filter(FilterType t, LogicType l, Card c)
    {
        switch(t)
        {
            case FilterType.Name:
                switch(l)
                {
                    case LogicType.Is:
                        Debug.Log("junk");
                        break;
                }
                Debug.Log("Some shit");
                break;
            default: throw new Exception("Not Valid Logic combination");

        }
    }

    public Filter(FilterType t, LogicType l, Player p)
    {

    }


}

