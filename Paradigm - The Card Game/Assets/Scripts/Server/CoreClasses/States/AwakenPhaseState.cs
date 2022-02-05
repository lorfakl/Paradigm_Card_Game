using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AwakenPhaseState : State, IState
{
    public AwakenPhaseState(Player p)
    {
        Owner = p;
    }

    public bool IsUniqueToPlayer()
    {
        Debug.Log("This is something inside AwakenPhaseState");
        return true;
    }

    public void OnEntry()
    {
        Debug.Log("This is something inside AwakenPhaseState");
    }

    public void OnExit()
    {
        Debug.Log("This is something inside AwakenPhaseState");
    }

    public async Task Operation()
    {
        Debug.Log("Operation inside AwakenPhaseState");
        await Owner.PerformAwaken();
    }

    public void SetOwner(Player p)
    {
        Debug.Log("This is something inside AwakenPhaseState");
    }

}