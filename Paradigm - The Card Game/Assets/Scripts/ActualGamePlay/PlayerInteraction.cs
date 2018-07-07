using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    public GameObject player;
    public GameObject gm;
    private Player p = null;
    private bool setUp;
    private Coroutine coroutine;
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
        if(p.IsAI)//need to find a better way of doing this, doesnt work with networked play
        {
            gm.GetComponent<GameEventsManager>().NoUiPlayerReturnedLocation = temp;
            print("Got a TC from AI");
            print("ai Temp Size: " + temp.Count);

        }
        else
        {
            gm.GetComponent<GameEventsManager>().UiPlayerReturnedLocation = temp;
            print("Got a TC from Human");
            print("human Temp Size: " + temp.Count);

        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        print("We in update?");
        
      
	}
}
