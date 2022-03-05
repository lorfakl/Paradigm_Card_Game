﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;

public struct LocationChanges  //this struct is for containing infomation regarding a location change in one package
{
    public List<Card> c;
    public Location origin;
    public Location destination;
    public string changeId;
}

public enum ContainsCriteria { Name, Type, Reference, ID}

public class Location: IEnumerable
{
    private string name;
    private Player owner;
    protected List<Card> contents;
    protected static List<Location> locations = new List<Location>(); 
    protected static Dictionary<Location, List<LocationChanges>> changesDict = 
                   new Dictionary<Location, List<LocationChanges>>(); 
    protected List<LocationChanges> changes;
    private Dictionary<string, Card> instanceIdDictionary = new Dictionary<string, Card>();

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

    public Dictionary<string, Card> InstanceIdLookUp
    {
        get { return instanceIdDictionary; }
    }

    public List<Card> Content
    {
        get { return this.contents; }
    }

    public ValidLocations ValidName
    {
        get { return (ValidLocations)ConvertFromLocation(this); }
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

    public static Location CreateTempLocation(Player p)
    {
        return new Location("temp", p);
    }

    public List<LocationChanges> GetChangesOnLocation()
    {
        return this.changes;
    }

    public List<Card> GetContents() {
        Debug.Log("What is happening?");  return this.contents; }

    public List<Card> GetContents(Type t)
    {
        List<Card> cardsOfTypet = new List<Card>();
        foreach(Card c in contents)
        {
            if (c.GetType() == t)
            {
                cardsOfTypet.Add(c);
            }
        }

        if(cardsOfTypet.Count < 1)
        {
            return null;
        }

        return cardsOfTypet;
    }

    public static int ConvertFromLocation(Location l)
    {
        ValidLocations validLocation;
        foreach(string name in Enum.GetNames(typeof(ValidLocations)))
        {
            if(name == l.Name)
            {
                if(Enum.TryParse<ValidLocations>(name, out validLocation))
                {
                    HelperFunctions.Print("Is the location enum actually Valid?: " + validLocation);
                    return (int)validLocation;
                }
            }
        }

        return -1;
    }
    public void MoveContent(List<Card> l, Location destination, bool overrideSamePlayer = false)
    {
        ProcessListLocationChanges(l, destination);
    }

    public void MoveContent(Card c, Location destination, bool overrideSamePlayer = false)
    {
        ProcessLocationChange(c, destination);
    }

    public void MoveContent(int size, Location destination, bool overrideSamePlayer = false)
    {
        List<Card> cards = new List<Card>();
        for(int i = 0; i < size; i++)
        {
            cards.Add(SelectContent(i));
        }
        ProcessListLocationChanges(cards, destination);
    }

    public void MoveContent(Location destination, bool overrideSamePlayer = false)
    {
        Card c = this.contents[0];
        ProcessLocationChange(c, destination);
        
    }

    public void MoveRandomContent(Location destination, bool overrideSamePlayer = false)
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

    public Card SelectContent(int index = 0)
    {
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
        this.AddToLookUp(c);
        
        
    }

    public Card Contains(Card target, ContainsCriteria criteria)
    {
        foreach (Card c in contents)
        {
            switch (criteria)
            {
                case ContainsCriteria.ID:
                    if (target.ID == c.ID)
                    {
                        return c;
                    } 
                    break;

                case ContainsCriteria.Name:
                    if (target.Name == c.Name)
                    {
                        return c;
                    }
                    break;

                case ContainsCriteria.Type:
                    if (target.GetType() == c.GetType())
                    {
                        return c;
                    }
                    break;

                default:
                    Debug.Log("Done goofed");
                    break;
            }
        }
        foreach (Card c in contents)
        {
            if(target.ClassType ==  c.ClassType || target.Name == c.Name)
            {
                return c;
            }
        }
        return null;
    }

    public bool Contains(Type cardType)
    {
        foreach(Card c in this.contents)
        {
            if(c.GetType() == cardType)
            {
                return true;
            }
        }

        return false;
    }


    public bool ContainsValidContent()
    {
        if(contents.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Fisher yates shuffle algorithm using System Random so its not related to any other randoms
    /// </summary>
    public void Shuffle()
    {
        System.Random rand = new System.Random();
        for(int i = 0; i < contents.Count; i++)
        {
            int j = rand.Next(0, contents.Count);
            Card temp = contents[j];
            contents[j] = contents[i];
            contents[i] = temp;
        }
    }

    public IEnumerator<Card> GetEnumerator()
    {
        foreach(Card c in contents)
        {
            yield return c;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }


    protected bool RemoveContent(Card c)
    {
        bool result = this.contents.Remove(c);
        instanceIdDictionary.Remove(c?.InstanceID.ToString());
        return result;
    }

    private void ProcessListLocationChanges(List<Card> l, Location destination, bool overrideSamePlayer = false)
    {
        try
        {
            CheckSamePlayerMoveOverride(overrideSamePlayer, this, destination);
        }
        catch (Exception)
        {
            string newDestinationName = destination.Name;
            //destination = this.Owner.GetLocation(newDestinationName);
            Debug.Log("The Error for cross player card movement with no override triggered");
        }

        LocationChanges newChanges = new LocationChanges();
        newChanges.c = l;
        newChanges.destination = destination;
        newChanges.origin = this;
        newChanges.changeId = Guid.NewGuid().ToString();

        changes.Add(newChanges);
        foreach (Card c in l)
        {
            
            
            if(!(RemoveContent(c)))
            {
                Debug.Log("ERROR!! ERROR!! Card Cant be removed because" + c.getName() + " is not locationed in Location: " 
                                                                                                            + this.name);
            }
            else
            {
                //Debug.Log(c.getName() + " has been moved from " + this.owner.PlayerID + "'s " + this.Name + " to "
                                                                //+ destination.owner.PlayerID + "'s " + destination.Name);
                changesDict[this] = changes;
                if (c.CurrentLocation == ValidLocations.Deck && destination.ValidName != ValidLocations.BZ)
                {
                    foreach (var abl in c.Abilities)
                    {
                        abl.CanCheckEvent = true;
                        HelperFunctions.Print("Ability is now looking for events: " + abl.OwningCardName);
                        HelperFunctions.Print(abl.Name);
                    }
                }
                destination.contents.Add(c);
                destination.AddToLookUp(c);
                c.setLocation(destination);
                HelperFunctions.RaiseNewEvent(this, changes, GetMoveAction(this, destination));
            }
        }
        
    }

    private void ProcessLocationChange(Card c, Location destination, bool overrideSamePlayer = false)
    {
        try
        {
            CheckSamePlayerMoveOverride(overrideSamePlayer, this, destination);
        }
        catch(Exception)
        {
            string newDestinationName = destination.Name;
            //destination = this.Owner.GetLocation(newDestinationName);
            Debug.Log("The Error for cross player card movement with no override triggered");
        }

        LocationChanges newChanges = new LocationChanges();
        newChanges.c = new List<Card>();
        newChanges.c.Add(c);
        newChanges.destination = destination;
        newChanges.origin = this;
        changes.Add(newChanges);
        
        if (!(RemoveContent(c)))
        {
            Debug.Log("ERROR!! ERROR!! Card Cant be removed because" + c.getName() + " is not locationed in Location: " 
                                                                                                            + this.name);
        }
        else
        {
            //Debug.Log(c.getName() + " has been moved from " + this.owner.PlayerID + "'s " + this.Name + " to " 
                                                            //+ destination.owner.PlayerID + "'s " + destination.Name);
            changesDict[this] = changes;
            MoveAction ma = GetMoveAction(this, destination);
            //Utilities.HelperFunctions.RaiseNewEvent(this, changes, ma);
            int returnSorcIndex = ConvertFromLocation(this);
            int returnDestIndex = ConvertFromLocation(destination);

            if(returnDestIndex > -1 && returnSorcIndex > -1)
            {
                //HelperFunctions.Print("It this true?: " + (c.CurrentLocation == ValidLocations.Deck && destination.ValidName != ValidLocations.BZ));
                //HelperFunctions.Print("Current Location: " + c.CurrentLocation.ToString());
                //HelperFunctions.Print("Current Destination: " + destination.ValidName.ToString());
                if (c.CurrentLocation == ValidLocations.Deck && destination.ValidName != ValidLocations.BZ)
                {
                    foreach (var abl in c.Abilities)
                    {
                        abl.CanCheckEvent = true;
                        //HelperFunctions.Print("Ability is now looking for events: " + abl.OwningCardName);
                        HelperFunctions.Print(abl.Name);
                        if(abl.Conditions == null)
                        {
                            HelperFunctions.Print("Conditions for " + abl.Name + " on Card " + abl.OwningCardName + " is null for some reason");
                        }

                        if (abl.Actions == null)
                        {
                            HelperFunctions.Print("Actions for " + abl.Name + " on Card " + abl.OwningCardName + " is null for some reason");
                        }
                    }
                }
                destination.contents.Add(c);
                destination.AddToLookUp(c);
                c.setLocation(destination);
                HelperFunctions.RaiseNewUIEvent(this, (ValidLocations)returnSorcIndex, (ValidLocations)returnDestIndex, ma, c);
                
            }
            else
            {
                HelperFunctions.Print("This is a non normal location move. Maybe territory challenge. Skip the UI event");
            }
            
        }

    }

    private void CheckSamePlayerMoveOverride(bool overrideSamePlayer, Location source, Location destination)
    {
        if(!overrideSamePlayer)
        {
            if(source.Owner.PlayerID != destination.Owner.PlayerID)
            {
                Debug.Log("Content is being moved from " + source.Owner.PlayerID + "'s " + source.Name + " to " + destination.Owner.PlayerID + "'s " + destination.Name);
                throw new Exception("Content is being moved between Owners without an Override");
            }
        }
    }

    private void AddToLookUp(Card c)
    {
        if (this.instanceIdDictionary.ContainsKey(c.InstanceID.ToString()))
        {
            this.instanceIdDictionary[c.InstanceID.ToString()] = c;
        }
        else
        {
            this.instanceIdDictionary.Add(c.InstanceID.ToString(), c);
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