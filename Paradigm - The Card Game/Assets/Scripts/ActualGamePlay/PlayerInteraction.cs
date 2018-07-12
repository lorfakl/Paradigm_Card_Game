using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    public GameObject player;
    public GameObject gm;
    private Player p = null;
    private bool setUp;
    private Coroutine coroutine;
    public delegate bool NotifyDoneChoosing();
    public event NotifyDoneChoosing IsDoneChoosing; 


    public Player CurrentPlayer
    {
        get { return p; }
    }
    // Use this for initialization
    void Awake()
    {
        if(gm == null)
        {
            gm = GameObject.FindWithTag("GameManager");
        }

        p = gm.GetComponent<GameEventsManager>().GrabPlayer();

    }
    

	IEnumerator Start ()
    {
        Location temp = new Location("temp", p);
        //p.LoadDeckFromDataBase();
        yield return StartCoroutine(p.ChooseTerritoryChallengeCard(temp));
        print("Temp Size: " + temp.Count);
        yield return StartCoroutine(p.ChooseBarriers());
        if(p.IsAI)//need to find a better way of doing this, doesnt work with networked play
        {
            gm.GetComponent<GameEventsManager>().NoUiPlayerReturnedLocation = temp;
            gm.GetComponent<GameEventsManager>().NonUIPlayer.IsPreparedToStart = true;
        }
        else
        {
            gm.GetComponent<GameEventsManager>().UiPlayerReturnedLocation = temp;
            gm.GetComponent<GameEventsManager>().UIPlayer.IsPreparedToStart = true;
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(p.IsAI)
        {
            print("Other stuff");
        }
        else
        {
            print("Human Stuff");
        }
      
	}

    bool GrabDestination(object sender, Location d)
    {
        print("Got our card(s)");

        return true;
    }
}
