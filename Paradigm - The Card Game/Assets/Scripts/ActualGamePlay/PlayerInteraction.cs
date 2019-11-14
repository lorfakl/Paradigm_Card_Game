using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperFunctions;

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
        print("Temp ID: " + temp.Owner.PlayerID);
        // p.LoadDeckFromDataBase();
        yield return StartCoroutine(p.ChooseTerritoryChallengeCard(temp));
        print("Temp Size: " + temp.Count);
        
        yield return StartCoroutine(p.ChooseBarriers(12));
        
        /*if(p.GetPlayerUIStatus())//need to find a better way of doing this, doesnt work with networked play
        {
            gm.GetComponent<EventManager>().NoUiPlayerReturnedLocation = temp;
            gm.GetComponent<EventManager>().NonUIPlayer.IsPreparedToStart = true;
        }
        else
        {
            gm.GetComponent<EventManager>().UiPlayerReturnedLocation = temp;
            gm.GetComponent<EventManager>().UIPlayer.IsPreparedToStart = true;
        }*/

        Player testP = (Player)p;
        print("Card count after all things Type:" + testP.PlayerID + " " + testP.PlayerDeck.Count + " Cards in hand: " + testP.GetLocationCount("Hand"));

        //print("Now other one");
        //p2.ListLocationSizes();
        DisplayFirstHand(testP);
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

    private void DisplayFirstHand(Player p)
    {
        p.PlayerDeck.Draw(5);
        if(p.GetPlayerUIStatus())
        {
            GameObject hand = GameObject.FindGameObjectWithTag("playerHand");
            Vector3 pos = new Vector3(-52f, -25, 0);
            float offset = 30f;
            foreach (Card c in p.GetLocation(ValidLocations.Hand))
            {
                GameObject go = Utilities.CreateCard(c, false, hand.transform);
                go.GetComponent<RectTransform>().position = pos;
                pos.x += offset;
            }
        }
        else
        {
            return;
        }
    }
}
