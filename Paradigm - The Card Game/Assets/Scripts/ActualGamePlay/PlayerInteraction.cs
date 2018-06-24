using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    public GameObject player;
    public GameObject gm;
    private Player p = null;
    private bool setUp;

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
        yield return StartCoroutine(p.ChooseTerritoryChallengeCard(temp));
        //gm.GetComponent<GameEventsManager>().UiPlayerReturnedLocation = temp;
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        
	}
}
