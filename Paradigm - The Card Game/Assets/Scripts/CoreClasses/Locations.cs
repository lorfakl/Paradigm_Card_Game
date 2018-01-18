using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Locations
{

    private string name;
    private List<Card> contents;

    public Locations(string name)
    {
        this.name = name;
    }

    public Locations(string name, List<Card> l)
    {
        this.name = name;
        ListAdd(l);
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public List<Card> GetContents() { return contents; }

    public int Count
    {
        get { return contents.Count; }
    }
    public void AddContent(Card c)
    {
        contents.Add(c);
    }

    public void ListAdd( List<Card> l)
    {
        foreach( Card c in l)
        {
            contents.Add(c);
        }
    }

    public void RemoveContent(Card c)
    {
        contents.Remove(c);
    }

    public void RemoveContent()
    {
        contents.RemoveAt(0);
    }

    public void ListRemove(List<Card> l)
    {
        foreach (Card c in l)
        {
            contents.Remove(c);
        }
    }

}
