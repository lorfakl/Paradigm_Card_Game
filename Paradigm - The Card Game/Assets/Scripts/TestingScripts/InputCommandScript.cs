using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputCommandScript : MonoBehaviour {

    public InputField commandInputField;
    public GameObject messageDisplayObject;
    private MessageDisplay messageDisplay;

	// Use this for initialization
	void Start ()
    {
        InputField commandField = commandInputField.GetComponent<InputField>();
        messageDisplay = messageDisplayObject.GetComponent<MessageDisplay>();
        commandField.onEndEdit.AddListener(delegate { CallOut(commandField.text); });
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CallOut(string text)
    {
        messageDisplay.RunCommand(text);
    }
    
}
