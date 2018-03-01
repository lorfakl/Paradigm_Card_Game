using UnityEngine;
using Utilities;
using System.Collections;

public class Block 
{
    private Attack attackBlocked;
    private Accessor blocker;

    public Block(Attack attackBlocked, Accessor blocker)
    {
        this.attackBlocked = attackBlocked;
        this.blocker = blocker;
        HelperFunctions.RaiseNewEvent(this, blocker, NonMoveAction.Block, attackBlocked.Attacker);
    }




}
