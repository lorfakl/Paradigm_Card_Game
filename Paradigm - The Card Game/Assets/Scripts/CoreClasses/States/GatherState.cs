using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

public class GatherState : State, IState
{
    public GatherState(Player p)
    {
        this.SetOwner(p);
    }

    bool IState.IsUniqueToPlayer()
    {
        return false;
    }

    void IState.OnEntry()
    {
        HelperFunctions.Print("Notify the transport layer of Gather start");
    }

    void IState.OnExit()
    {
        HelperFunctions.Print("Notify the transport layer of Gather END");
    }

    async Task IState.Operation()
    {
        await Owner.PerformGather();
    }

    public void SetOwner(Player p)
    {
        this.Owner = p;
    }

}
