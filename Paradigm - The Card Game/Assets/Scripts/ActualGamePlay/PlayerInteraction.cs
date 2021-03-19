using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    public GameObject player;
    public GameObject gm;
    public int attackChance;
    private IPlayable p = null;
    private bool setUp;
    private Coroutine coroutine;
    public delegate bool NotifyDoneChoosing();
    public event NotifyDoneChoosing IsDoneChoosing; 
    /*

    public IPlayable CurrentPlayer
    {
        get { return p; }
    }

    public void GatherPhaseStart()
    {
        GatherPhaseAction();
    }

    public void AwakenPhaseStart()
    {
        AwakenPhaseAction();
    }

    public void CentralPhaseStart()
    {
        CentralPhaseAction();
    }

    public void CrystalPhaseStart()
    {
        CrystalPhaseAction();
    }

    public void EndPhaseStart()
    {
        EndPhaseAction();
    }
    // Use this for initialization
    void Awake()
    {
        
        
    }
    
	
	// Update is called once per frame
	void Update ()
    {
        
      
	}

    bool GrabDestination(object sender, Location d)
    {
        print("Got our card(s)");

        return true;
    }

    private IEnumerator GatherPhaseAction()
    {
        print("Gather Coroutine started?");
        yield return StartCoroutine(p.PerformGather());
    }

    private IEnumerator AwakenPhaseAction()
    {
        print("Awaken Coroutine started?");
        yield return StartCoroutine(p.PerformAwaken());
    }
    private IEnumerator CentralPhaseAction()
    {
        print("Central Coroutine started?");
        yield return StartCoroutine(p.PerformCentral());
    }

    private IEnumerator CrystalPhaseAction()
    {
        print("Crystal Coroutine started?");
        yield return StartCoroutine(p.PerformCrystal());
    }

    private IEnumerator EndPhaseAction()
    {
        print("End Coroutine started?");
        yield return StartCoroutine(p.PerformEnd());
    }*/
}
