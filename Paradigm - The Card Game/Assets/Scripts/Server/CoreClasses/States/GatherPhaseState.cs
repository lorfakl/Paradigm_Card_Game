using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GatherPhaseState : State, IState
{
    public GatherPhaseState(Player p)
    {
        Owner = p;
    }

    public bool IsUniqueToPlayer()
    {
        Debug.Log("IsUniqueToPlayer inside GatherPhaseState");
        return true;
    }

    public void OnEntry()
    {
        Debug.Log("This is something inside GatherPhaseState");
    }

    public void OnExit()
    {
        Debug.Log("This is something inside GatherPhaseState");
    }

    public async Task Operation()
    {
        Debug.Log("Anything here in gather phase?");
        //await Owner.PerformGather();
        
    }

    public void SetOwner(Player p)
    {
        Debug.Log("This is something inside GatherPhaseState");
    }

    
}
