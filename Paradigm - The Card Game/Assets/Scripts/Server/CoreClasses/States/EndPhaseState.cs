using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EndPhaseState : State, IState
{
    public EndPhaseState(Player p)
    {
        Owner = p;
    }

    public EndPhaseState()
    {

    }
    bool IState.IsUniqueToPlayer()
    {
        Debug.Log("IsUniqueToPlayer inside EndPhaseState");
        return true;
    }

    void IState.OnEntry()
    {
        Debug.Log("This is something inside EndPhaseState");
    }

    void IState.OnExit()
    {
        Debug.Log("This is something inside EndPhaseState");
    }

    public async Task Operation()
    {
        Debug.Log("Operation inside EndPhaseState");
        await Owner.PerformEnd();

    }

    void IState.SetOwner(Player p)
    {
        Debug.Log("This is something inside EndPhaseState");
    }
}
