using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CrystalPhaseState : State, IState
{
    public CrystalPhaseState(Player p)
    {
        Owner = p;
    }
    public bool IsUniqueToPlayer()
    {
        Debug.Log("This is something inside CrystalPhaseState");
        return true;
    }

    public void OnEntry()
    {
        Debug.Log("This is something inside CrystalPhaseState");
    }

    public void OnExit()
    {
        Debug.Log("This is something inside CrystalPhaseState");
    }

    public async Task Operation()
    {
        Debug.Log("Operation inside CrystalPhaseState");
        await Owner.PerformCrystal();
    }

    public void SetOwner(Player p)
    {
        Debug.Log("This is something inside CrystalPhaseState");
    }

}