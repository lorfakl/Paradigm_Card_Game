using UnityEngine;
using Utilities;
using System.Collections;

public class Attack 
{
    private Card target;
    private Accessor attacker;
    private int attackDamage;

    public Attack(Accessor attacker, Card target)
    {
        this.attacker = attacker;
        this.target = target;
        this.attackDamage = attacker.Power;
        GameEventsManager.NotifySubsOfEvent += CheckForBlockEvent;
        HelperFunctions.RaiseNewEvent(this, attacker, NonMoveAction.Attack, target);

    }

    public Card Target
    {
        get { return this.target; }
        set { this.target = value; }
    }

    public Accessor Attacker
    {
        get { return this.attacker; }
        set { this.attacker = value; }
    }

    public int AttackDamage
    {
        get { return this.attackDamage; }
        set { this.attackDamage = value; }
    }

    private void CheckForBlockEvent(object sender, GameEventsArgs e)
    {
        if(e.ActionEvent == NonMoveAction.Block)
        {
            this.target = e.EventOriginCard;
            Debug.Log(e.EventOriginCard.Name + " has blocked the attack made by " + this.attacker.Name);
        }
    }



}
