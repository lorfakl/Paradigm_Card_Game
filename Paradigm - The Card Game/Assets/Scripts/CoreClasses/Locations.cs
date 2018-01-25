using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Location
{

    public struct LocationChanges
    {
        public Card c;
        public Location origin;
        public Location destination;
    }

    private string name;
    private Player owner;
    private List<Card> contents;
    private static List<Location> locations = new List<Location>();
    private static Dictionary<Location, List<LocationChanges>> changesDict = new Dictionary<Location, List<LocationChanges>>();
    private List<LocationChanges> changes;

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public int Count
    {
        get { return this.contents.Count; }
    }

    public Location(string name, Player p)
    {
        this.name = name;
        this.owner = p;
        contents = new List<Card>() ;
        locations.Add(this);
        this.changes = new List<LocationChanges>();
        changesDict.Add(this, this.changes);
    }

    public List<Card> GetContents() { return this.contents; }

    public void MoveContent(List<Card> l, Location destination)
    {
        ProcessListLocationChanges(l, destination);
    }

    public void MoveContent(Card c, Location destination)
    {
        ProcessLocationChange(c, destination);
    }

    public void MoveContent(Location destination)
    {
        Card c = this.contents[0];
        ProcessLocationChange(c, destination);
    }

    private void ProcessListLocationChanges(List<Card> l, Location destination)
    {
        LocationChanges newChanges = new LocationChanges();
        foreach (Card c in l)
        {
            newChanges.c = c;
            newChanges.destination = destination;
            newChanges.origin = this;
            changes.Add(newChanges);
            destination.contents.Add(c);
            c.setLocation(destination);
            if(!(this.contents.Remove(c)))
            {
                Debug.Log("ERROR!! ERROR!! Card Cant be removed because" + c.getName() + " is not locationed in Location: " + this.name);
            }
        }
        
    }

    private void ProcessLocationChange(Card c, Location destination)
    {
        LocationChanges newChanges = new LocationChanges();
        newChanges.c = c;
        newChanges.destination = destination;
        newChanges.origin = this;
        changes.Add(newChanges);
        destination.contents.Add(c);
        c.setLocation(destination);
        if (!(this.contents.Remove(c)))
        {
            Debug.Log("ERROR!! ERROR!! Card Cant be removed because" + c.getName() + " is not locationed in Location: " + this.name);
        }

    }

}
