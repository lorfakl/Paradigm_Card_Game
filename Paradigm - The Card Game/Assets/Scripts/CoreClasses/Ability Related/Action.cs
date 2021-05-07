using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Builder;
using System.Reflection;

[Serializable]
public class Action
{
    private static Dictionary<string, Action<Action>> ActionToExecuteDict = new Dictionary<string, Action<Action>>();
    private static bool IsActionDictLoaded = false;
    public Action(Action c)
    {
        IsNegated = false;
        Type type = typeof(Action);
        PropertyInfo[] propertyInfo = type.GetProperties();

        foreach (PropertyInfo pInfo in propertyInfo)
        {
            if (pInfo.GetCustomAttribute<JsonPropertyAttribute>() != null)
            {
                type.GetProperty(pInfo.Name)?.SetValue(this, type.GetProperty(pInfo.Name)?.GetValue(c, null));
            }
        }
    }

    public Action()
    {
        IsNegated = false;
        
    }
    #region Properties

    #region JSONProperties

    [JsonProperty]
    public string EventType
    {
        get;
        set;
    }

    [JsonProperty]
    public string EventName
    {
        get;
        set;
    }

    [JsonProperty]
    public string Turnphase
    {
        get;
        set;
    }

    [JsonProperty(PropertyName = "CardType")]
    public string ClassType
    {
        get;
        set;
    }

    [JsonProperty]
    public string Target
    {
        get;
        set;
    }

    [JsonProperty(PropertyName = "Search Criteria")]
    public SearchCriteria SearchCriteria
    {
        get;
        set;
    }

    [JsonProperty]
    public Builder.Source Source
    {
        get;
        set;
    }
    [JsonProperty]
    public Destination Destination
    {
        get;
        set;
    }
    [JsonProperty]
    public string Trigger
    {
        get;
        private set;
    }

    #endregion

    /// <summary>
    /// Whether or not the Action is negated, this private because it will only be accessed
    /// by other objects of type Action
    /// </summary>
    private bool IsNegated 
    {
        get;
        set;
    }

    public Ability Ability
    {
        get;
        set;
    }

    public Card Card
    {
        get;
        set;
    }

    public Player Owner
    {
        get;
        set;
    }

    public bool IsAnyOwner
    {
        get;
        private set;
    }

    public Card CardTrigger
    {
        get;
        private set;
    }

    private List<Card> Targets
    {
        get;
        set;
    }

    /*public Func<bool> Resolve
    {
        get;
        private set;
    }*/

    #endregion

    public static void AddDictionaryData()
    {
        Action act = new Action();
        if(!IsActionDictLoaded)
        {
            ActionToExecuteDict.Add(NonMoveAction.Active.ToString(), ActivateCard);
            IsActionDictLoaded = true;
        }
    }

    public void Resolve()
    {
        ActionToExecuteDict[this.EventName](this);
    }

    public void GenerateEvent(GameEventsArgs ge)
    {
        GameEventsArgs gameEvent = new GameEventsArgs();
        if(EventType == "Move")
        {
            MoveAction ma = Utilities.HelperFunctions.ParseEnum<MoveAction>(EventName);
            gameEvent.MoveActionEvent = ma;
        }
        else
        {
            NonMoveAction nma = Utilities.HelperFunctions.ParseEnum<NonMoveAction>(EventName);
            gameEvent.ActionEvent = nma;
        }
        
        if(this.Source != null) //To DO handle If Source is Any or Both
        {
            if(this.Source.Player == "Other")
            {
                Player playerRef = Utilities.HelperFunctions.GetReferenceToOtherPlayer(Owner, ge);
                gameEvent.CardTargets = SearchCriteria.FindandReturnCards(SearchCriteria, playerRef.GetLocation(
            /*This will break if there is no Source specified in the JSON*/Utilities.HelperFunctions.ParseEnum<ValidLocations>(Source.Location)));
            }
               

            //Currently this line assumes the cards we need to find belong to the Owner of the Action but that is not always the case
            //There needs to be a way for Actions and Conditions to get a reference to the other player
            gameEvent.CardTargets = SearchCriteria.FindandReturnCards(SearchCriteria, Owner.GetLocation(
            /*This will break if there is no Source specified in the JSON*/Utilities.HelperFunctions.ParseEnum<ValidLocations>(Source.Location)));
        }
        else
        {
            Utilities.HelperFunctions.Print("Target: " + Target);
            if(Target != null)
            {
                Utilities.HelperFunctions.Print("Using the Target Variable");
                if(Target == "This")
                {
                    gameEvent.CardTargets = new List<Card> { Card };
                }
                else if(Target == "Target")
                {
                    if(ge.CardTargets != null)
                    {
                        gameEvent.CardTargets = ge.CardTargets;
                    }
                    else
                    {
                        gameEvent.CardTargets = new List<Card> { ge.TargetCard };
                    }
                }
                else
                {
                    Utilities.HelperFunctions.Print("Target must be equal to Trigger: " + Target);
                    gameEvent.CardTargets = new List<Card> { ge.EventOriginCard };
                }
            }
        }

        gameEvent.EventOriginCard = Card;
        gameEvent.EventOwner = Card.Owner; //had to use Card.Owner because the Owner of Action isnt getting set for some reason
        gameEvent.PlayerTarget = gameEvent.CardTargets[0].Owner;
        //gameEvent.TurnPhase = Utilities.HelperFunctions.ParseEnum<TurnPhase>(this.Turnphase);
        gameEvent.TriggeringAbility = Ability;
        gameEvent.Type = global::EventType.Gameplay;
        gameEvent.IsUIEvent = false;

        Utilities.HelperFunctions.RaiseNewEvent(this, gameEvent, Card);
        gameEvent.Print();
        this.Targets = gameEvent.CardTargets;
    }

    private static void ActivateCard(Action act)
    {
        if(!act.IsNegated)
        {
            if(act.Target == "This")
            {
                act.Card.IsActive = true;
                Utilities.HelperFunctions.RaiseNewEvent(act, act.Card, NonMoveAction.Active, act.Card);
                //return true;
            }
            else
            {
                List<Card> results = SearchCriteria.FindandReturnCards(act.SearchCriteria, act.Owner.Field);
                foreach(var c in results)
                {
                    c.IsActive = true;
                    Utilities.HelperFunctions.RaiseNewEvent(act, act.Card, NonMoveAction.Active, act.Card);
                }
            }
        }
        else
        {
            //I believe this Generates a UI event and a Non UI Event
            act.Card.getLocation().MoveContent(act.Card.Owner.Grave);
        }

        //return false;
    }
}
