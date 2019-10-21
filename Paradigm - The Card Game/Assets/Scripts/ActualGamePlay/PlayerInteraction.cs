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


    public IPlayable CurrentPlayer
    {
        get { return p; }
    }

    public IEnumerator GatherPhaseStart()
    {
        yield return GatherPhaseAction();
    }

    public IEnumerator AwakenPhaseStart()
    {
        yield return AwakenPhaseAction();
    }

    public IEnumerator CentralPhaseStart()
    {
        yield return CentralPhaseAction();
    }

    public IEnumerator CrystalPhaseStart()
    {
        yield return CrystalPhaseAction();
    }

    public IEnumerator EndPhaseStart()
    {
        yield return EndPhaseAction();
    }
    // Use this for initialization
    void Awake()
    {
        if(gm == null)
        {
            gm = GameObject.FindWithTag("GameManager");
        }

        p = gm.GetComponent<EventManager>().GrabPlayer();
        
    }
    
    public void StatusCheck()
    {
        print("Called from playerInteraction");
    }

	IEnumerator Start ()
    {
        Location temp = new Location("temp", (Player)p);
        //p.LoadDeckFromDataBase();
        yield return StartCoroutine(p.ChooseTerritoryChallengeCard(temp));
        print("Temp Size: " + temp.Count);
        yield return StartCoroutine(p.ChooseBarriers(12));
        
        if(p.GetPlayerUIStatus())//need to find a better way of doing this, doesnt work with networked play
        {
            gm.GetComponent<EventManager>().NoUiPlayerReturnedLocation = temp;
            gm.GetComponent<EventManager>().NonUIPlayer.IsPreparedToStart = true;
        }
        else
        {
            gm.GetComponent<EventManager>().UiPlayerReturnedLocation = temp;
            gm.GetComponent<EventManager>().UIPlayer.IsPreparedToStart = true;
        }

        Player testP = (Player)p;
        print("Card count after all things Type:" + testP.GetPlayerUIStatus() + testP.PlayerDeck.Count);
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
    }
}
