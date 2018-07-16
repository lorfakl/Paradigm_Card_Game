using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;

public struct LocationChanges  //this struct is for containing infomation regarding a location change in one package
{
    public Card c;
    public Location origin;
    public Location destination;
}

public class Location
{
    private string name;
    private Player owner;
    protected List<Card> contents;
    protected static List<Location> locations = new List<Location>(); 
    protected static Dictionary<Location, List<LocationChanges>> changesDict = 
                   new Dictionary<Location, List<LocationChanges>>(); 
    protected List<LocationChanges> changes;

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public int Count
    {
        get { return this.contents.Count; }
    }

    public Player Owner
    {
        get { return this.owner; }
        set { this.owner = value; }
    }

    public List<Card> Content
    {
        get { return this.contents; }
    }

    public Location()
    {
    
    }

    public Location(string name, Player p)
    {
        this.name = name;
        this.owner = p;
        contents = new List<Card>();
        locations.Add(this);
        this.changes = new List<LocationChanges>();
        changesDict.Add(this, this.changes);
    }

    public List<LocationChanges> GetChangesOnLocation()
    {
        return this.changes;
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

    public void MoveRandomContent(Location destination)
    {
        int index = UnityEngine.Random.Range(0, this.Count);
        Card c = this.contents[index];
        ProcessLocationChange(c, destination);
    }

    public Card SelectRandomContent()
    {
        int index = UnityEngine.Random.Range(0, this.Count);
        Card c = this.contents[index];
        return c;
    }

    public void AddContent(Card c)
    {
        if (this.contents == null)
        {
            Debug.Log("The Location contents are Null as fuck!");
            return;
        }

        this.contents.Add(c);    
    }

    protected bool RemoveContent(Card c)
    {
        bool result = this.contents.Remove(c);
        return result;
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
                Debug.Log("ERROR!! ERROR!! Card Cant be removed because" + c.getName() + " is not locationed in Location: " 
                                                                                                            + this.name);
            }
            else
            {
                //Debug.Log(c.getName() + " has been moved from " + this.owner.PlayerID + "'s " + this.Name + " to "
                                                               // + destination.owner.PlayerID + "'s " + destination.Name);
                changesDict[this] = changes;
                Utilities.HelperFunctions.RaiseNewEvent(this, changes, GetMoveAction(this, destination));
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
            Debug.Log("ERROR!! ERROR!! Card Cant be removed because" + c.getName() + " is not locationed in Location: " 
                                                                                                            + this.name);
        }
        else
        {
            Debug.Log(c.getName() + " has been moved from " + this.owner.PlayerID + "'s " + this.Name + " to " 
                                                            + destination.owner.PlayerID + "'s " + destination.Name);
            changesDict[this] = changes;
            Utilities.HelperFunctions.RaiseNewEvent(this, changes, GetMoveAction(this, destination));
        }

    }

    private MoveAction GetMoveAction(Location from, Location to)
    {
        if(from.name == "BZ")
        {
            return MoveAction.Break;
        }
        else if(to.name == "BZ")
        {
            return MoveAction.Build;
        }
        else if(to.name == "SC")
        {
            return MoveAction.Collect;
        }
        else if((from.name == "Hand") && (to.name == "Grave"))
        {
            return MoveAction.Delete;
        }
        else if((from.name == "Field") && (to.name == "Grave"))
        {
            return MoveAction.Despawn;
        }
        else if((from.name == "Deck") && (to.name == "Hand"))
        {
            return MoveAction.Draw;
        }
        else if(to.name == "LZ")
        {
            return MoveAction.Lock;
        }
        else if(to.name == "DZ")
        {
            return MoveAction.Rest;
        }
        else if(to.name == "Deck")
        {
            return MoveAction.Return;
        }
        else if(to.name == "Field")
        {
            return MoveAction.Spawn;
        }
        else if((from.name == "LZ") && (to.name == "Deck"))
        {
            return MoveAction.Unlock;
        }
        else
        {
            return MoveAction.None;
        }
    }
}
