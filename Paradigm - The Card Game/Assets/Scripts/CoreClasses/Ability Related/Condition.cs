using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Builder;
using Newtonsoft.Json;
using System;
using Utilities;
using System.Linq;
using System.Reflection;

[Serializable]
public class Condition
{
    private enum EventNames{ Active, Add, Any, Attack, Battle, Damage, DimTwist, Forge, Initiate, Move, Null, Turn};
    private Dictionary<string, System.Action<GameEventsArgs>> CheckConditionDictionary = new Dictionary<string, Action<GameEventsArgs>>();

    private Ability masterAbility;
    private Card masterCard;
    private Player masterPlayer;
    private bool canCheckForEvents; //so abilities dont check for events from the deck
    private string condition;
    private string mod;
    private bool isTriggerOnMove;
    private bool isAnyOwner;
    private MoveAction moveTrigger;
    private NonMoveAction nonMoveTrigger;

    public Condition(Condition condition1)
    {
        Type type = typeof(Condition);
        PropertyInfo[] propertyInfo = type.GetProperties();

        foreach (PropertyInfo pInfo in propertyInfo)
        {
            if(pInfo.GetCustomAttribute<JsonPropertyAttribute>() != null)
            {
                type.GetProperty(pInfo.Name)?.SetValue(this, type.GetProperty(pInfo.Name)?.GetValue(condition1, null));
            }
        }

    }

    public Condition()
    {
        CheckConditionDictionary.Add(EventNames.Active.ToString(), CheckActiveCondition);
        CheckConditionDictionary.Add(EventNames.Add.ToString(), CheckAddCondition);
        CheckConditionDictionary.Add(EventNames.Attack.ToString(), CheckAttackCondition);
        CheckConditionDictionary.Add(EventNames.Battle.ToString(), CheckBattleCondition);
        CheckConditionDictionary.Add(EventNames.Damage.ToString(), CheckDamageCondition);
        CheckConditionDictionary.Add(EventNames.DimTwist.ToString(), CheckDimTwistCondition);
        CheckConditionDictionary.Add(EventNames.Forge.ToString(), CheckForgeCondition);
        CheckConditionDictionary.Add(EventNames.Initiate.ToString(), CheckInitiateCondition);
        CheckConditionDictionary.Add(EventNames.Move.ToString(), CheckMoveCondition);
        CheckConditionDictionary.Add(EventNames.Null.ToString(), CheckNullCondition);
        CheckConditionDictionary.Add(EventNames.Turn.ToString(), CheckTurnCondition);

    }

    #region Properties

    #region JSONProperties

    [JsonProperty]
    public string EventType { get; set; }

    [JsonProperty]
    public string EventName { get; set; }

    [JsonProperty]
    public string Turnphase { get; set; }

    [JsonProperty(PropertyName = "CardType")]
    public string ClassType { get; set; }

    [JsonProperty]
    public string Player { get; set; }

    [JsonProperty]
    public string Target { get; set; }

    [JsonProperty]
    public string Location { get; set; }

    [JsonProperty]
    public SearchCriteria SearchCriteria { get; set; }

    [JsonProperty(PropertyName = "TargetActions")]
    public List<TargetAction> TargetActions { get; set; }

    [JsonProperty]
    public Builder.Source Source { get; set; }

    [JsonProperty]
    public Destination Destination { get; set; }

    [JsonProperty(PropertyName = "IsSatisfiedOnInitiate")]
    public bool IsSatisfiedOnInitiate { get; private set; }

    [JsonProperty(PropertyName = "Inverse")]
    public bool IsInverse { get; private set; }
    #endregion

    public Ability Ability
    {
        get { return masterAbility; }
        set { masterAbility = value; }
    }

    public Card Card
    {
        get { return masterCard; }
        set { masterCard = value; }
    }

    public Player Owner
    {
        get { return masterPlayer; }
        set { masterPlayer = value; }
    }
    public bool CanCheckForEvents
    {
        get;
        set;
    }


    public bool IsConditionMet
    {
        get;
        private set;
    }
    

    public bool IsAnyOwner
    {
        get;
        private set;
    }

    public Card Trigger
    {
        get;
        private set;
    }

    
    #endregion
    
    public bool CheckCondition(GameEventsArgs e)
    {

        if(Ability == null && Card == null)
        {
            HelperFunctions.Print("Ugh so many nullreferences");
            //HelperFunctions.Print()
            //HelperFunctions.Print("Is Card Null?" + this?.Card.Name);
            HelperFunctions.Print("Is Abil Null?" + this?.Ability.Name);
        }
        /*try
        {
            HelperFunctions.Print("Condition of " + Ability.Name + " from " + Card.Name + " is checking the newest Event");
        }
        catch(Exception ex)
        {
            HelperFunctions.Print("What condition Failed?: " + this.Ability.Name + " " + this.Ability.OwningCardName);
        }*/
        
        if (IsSatisfiedOnInitiate)
        {
            IsConditionMet = true;
            return IsConditionMet;
        }

        if (e.MoveActionEvent != MoveAction.None) //if there is a move action call the Move Action Condition Check
        {
            if(e.MoveActionEvent.ToString() == EventName) //if the type of move is relevant to the condition instance
            {
                CheckMoveCondition(e);
            }
            else
            {
                return false;
            }

        }
        else //else call the function for the specific Non Move Action
        {
            if(e.ActionEvent.ToString() == EventName)
            {
                CheckConditionDictionary[e.ActionEvent.ToString()](e);
            }
            else
            {
                return false;
            }
            
        }

        return IsConditionMet;
    }

    #region Private EventCheck Functions
    private void CheckActiveCondition(GameEventsArgs e)
    {
        if(SearchCriteria == null) //No arbitary query
        {
            if (e.EventOriginCard == Card) //this should work because the Card instance's ability should be referring to
            {                              //that same instance
                IsConditionMet = true;
                HelperFunctions.Print("This Card is active it is the same");
                return;
            }
            else
            {
                HelperFunctions.Print("These are two different cards");
                HelperFunctions.Print("Card Owners" + "\n EventOriginOwner: " + e.EventOriginCard?.Owner?.PlayerName + 
                    "\n COndition Card Owner: " + Card?.Owner?.PlayerName + "\n EventOriginName: " + e?.EventOriginCard?.Name
                    + "\n ConditionCard Name: " + Card?.Name);
                IsConditionMet = false;
            }
        }
        else
        {
            IsConditionMet = SearchCriteria.FindCardInfo(SearchCriteria, new List<Card> { e.EventOriginCard });
            return;
        }
    }

    private void CheckAddCondition(GameEventsArgs e)
    {

    }

    private void CheckDamageCondition(GameEventsArgs e)
    {

    }

    private void CheckMoveCondition(GameEventsArgs e)
    {
        if(SearchCriteria == null)
        {
            //HelperFunctions.Print(e.MoveActionEvent.ToString());
            HelperFunctions.Print(/*Card.Name + */"'s Condition Target Value: " + Target.ToString());
            if (Target == "This" && e.TargetCard.InstanceID == Card.InstanceID)
            {
                HelperFunctions.Print(Card.Name + " is the card that was moved");
                IsConditionMet = true;
            }
            else
            {
                //HelperFunctions.Print(Card.Name + " is the card that was not moved " +  e.TargetCard.Name + " was moved instead");
                //HelperFunctions.Print("InstanceID of Conditioner Owner " + Card.InstanceID);
                //HelperFunctions.Print("InstanceID of Card Moved " + e.TargetCard.InstanceID);
            }
        }
        else
        {
            //check using the SearchCriteria
        }
    }

    private void CheckInitiateCondition(GameEventsArgs e)
    {

    }

    private void CheckNullCondition(GameEventsArgs e)
    {

    }

    private void CheckTurnCondition(GameEventsArgs e)
    {

    }

    private void CheckAttackCondition(GameEventsArgs e)
    {

    }

    private void CheckBattleCondition(GameEventsArgs e)
    {

    }

    private void CheckForgeCondition(GameEventsArgs e)
    {

    }

    private void CheckDimTwistCondition(GameEventsArgs e)
    {

    }

    #endregion
}


