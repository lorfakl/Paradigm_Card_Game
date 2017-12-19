﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour {

    public Button backButton;
    // Use this for initialization
    void Start()
    {
        Button back = backButton.GetComponent<Button>();
        back.onClick.AddListener(loadlevel);
    }

    // Update is called once per frame
    void loadlevel()
    {
        SceneManager.LoadScene("mainmenu");
    }
}
