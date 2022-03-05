using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSelectState : State, IState
{
    public BarrierSelectState()
    {
        this.Player1 = GameMaster.PlayerOne;
        this.Player2 = GameMaster.PlayerTwo;
        Debug.Log("Barrier State initialized");
    }

    public void SetOwner(Player p)
    {

    }

    public bool IsUniqueToPlayer()
    {
        return false;
    }

    public void OnEntry()
    {
        Debug.Log("Tell the transport layer we have entered the barrier select state");
    }

    public void OnExit()
    {
        Debug.Log("Tell the transport layer  we have exited the barrier select state");
    }

    async public Task Operation()
    {
        await this.Player1.ChooseBarriers(GlobalGameConfiguration.barriers);
        await this.Player2.ChooseBarriers(GlobalGameConfiguration.barriers);
    }

    
}