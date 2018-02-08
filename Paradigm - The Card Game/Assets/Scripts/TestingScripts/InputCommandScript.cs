using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputCommandScript : MonoBehaviour {

    public InputField commandInputField;
    public GameObject messageDisplayObject;
    public delegate void CommandListener();
    CommandListener commandListener;
    private Dictionary<string, MessageDisplay.CommandFunction> commandsDictionary = new Dictionary<string, MessageDisplay.CommandFunction>();
    private bool isPrepared = false;
    private MessageDisplay messageDisplay;

	// Use this for initialization
	void Start ()
    {
        InputField commandField = commandInputField.GetComponent<InputField>();
        messageDisplay = messageDisplayObject.GetComponent<MessageDisplay>();
        //commandField.onEndEdit.AddListener(delegate { CallOut(); });
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PrepareDict()
    {
        if(!isPrepared)
        {
            //commandsDictionary.Add("", messageDisplay.ShowCommands());
        }
    }
    
}
