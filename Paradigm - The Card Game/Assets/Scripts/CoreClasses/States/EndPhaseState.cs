using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EndPhaseState : IState
{
    bool IState.IsUniqueToPlayer()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnEntry()
    {
        throw new System.NotImplementedException();
    }

    void IState.OnExit()
    {
        throw new System.NotImplementedException();
    }

    Task IState.Operation()
    {
        throw new System.NotImplementedException();
    }

    void IState.SetOwner(Player p)
    {
        throw new System.NotImplementedException();
    }
}
