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
        Debug.Log("Tell thje transport layer avout this");
    }

    public void OnExit()
    {
        Debug.Log("Tell the transport layer avout this");
    }

    async public Task Operation()
    {
        Debug.Log("Start Player 1 Operation");
        await this.Player1.ChooseBarriers(GlobalGameConfiguration.barriers);
        Debug.Log("Awaiting? Player 1 Operation");
        Debug.Log("Start Player 2 Operation");
        await this.Player2.ChooseBarriers(GlobalGameConfiguration.barriers);
        Debug.Log("Awaiting? Player 2 Operation");
    }

    
}
