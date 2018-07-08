using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditDeckButton : MonoBehaviour
{
    public Button editdeckbutton;

	// Use this for initialization
	void Start ()
    {
        Button edit = editdeckbutton.GetComponent<Button>();
        edit.onClick.AddListener(loadlevel);
	}
	
	
	void loadlevel ()
    {
        SceneManager.LoadScene("editdeck");
	}
}
