using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDisplay : MonoBehaviour {

    public GameObject messagePanel;
    public delegate void CommandFunction();
    CommandFunction commandFunction;

	// Use this for initialization
	void Start ()
    {
	    	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowCommands()
    {
        Debug.Log("Commands: '!commands', 'draw', 'show hand', 'end': Ends the turn, 'attack', 'show field', 'show deck' ");
    }
}
