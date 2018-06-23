using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Builder;

public class Condition
{
    private Ability masterAbility;
    private Card masterCard;
    private Player masterPlayer;
    private bool canCheckForEvents; //so abilities dont check for events from the deck
    private string condition;
    private string mod;
    
    public Condition(Ability abilityMaster, string condition)
    {
        this.masterAbility = abilityMaster;
        this.masterCard = abilityMaster.CardOwner;
        this.masterPlayer = abilityMaster.Owner;
        this.condition = condition;
    }

    public bool CheckCondition(GameEventsArgs e)
    {
        AbilityBuilder.ConditionCheck function = AbilityBuilder.GetConditionCheck(condition);
        return function(e, this.masterCard, mod);
    }
}
