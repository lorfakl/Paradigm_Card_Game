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
        contents = null;
    }

    public Locations(string name, List<Card> l)
    {
        this.name = name;
        ListAdd(l);
    }
    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public int Count
    {
        get { return this.contents.Count; }
    }

    public void AddList(List<Card> l)
    {
        if(this.contents == null)
        {
            this.contents = l;
        }
    }

    public List<Card> GetContents() { return this.contents; }

    public void AddContent(Card c)
    {
        this.contents.Add(c);
    }

    public void ListAdd( List<Card> l)
    {
        foreach( Card c in l)
        {
            this.contents.Add(c);
        }
    }

    public void RemoveContent(Card c)
    {
        this.contents.Remove(c);
    }

    public void RemoveContent()
    {
        this.contents.RemoveAt(0);
    }

    public void ListRemove(List<Card> l)
    {
        foreach (Card c in l)
        {
            this.contents.Remove(c);
        }
    }

}
