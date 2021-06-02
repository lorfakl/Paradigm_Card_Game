using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CentralPhaseState : State, IState
{
    public CentralPhaseState(Player p)
    {
        Owner = p;
    }

    public bool IsUniqueToPlayer()
    {
        Debug.Log("This is something inside CentralPhaseState");
        return true;
    }

    public void OnEntry()
    {
        Debug.Log("This is something inside CentralPhaseState");
    }

    public void OnExit()
    {
        Debug.Log("This is something inside CentralPhaseState");
    }

    public async Task Operation()
    {
        Debug.Log("Operation inside CentralPhaseState");
        await Owner.PerformCentral();
    }

    public void SetOwner(Player p)
    {
        Debug.Log("This is something inside CentralPhaseState");
    }

}