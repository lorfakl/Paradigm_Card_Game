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

    private void CheckForBlockEvent(object sender, GameEventsArgs e)
    {
        if(e.ActionEvent == NonMoveAction.Block)
        {

        }
    }



}
