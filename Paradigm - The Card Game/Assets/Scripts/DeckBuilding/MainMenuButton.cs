using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DataBase;

public class MainMenuButton : MonoBehaviour {

    public Button backButton;
    DeckManager deckManagerScript;
    // Use this for initialization
    void Start()
    {
        Button back = backButton.GetComponent<Button>();
        back.onClick.AddListener(LoadLevel);
        GameObject deckManager = GameObject.Find("DeckManager");
        deckManagerScript = deckManager.GetComponent<DeckManager>();

    }

    // Update is called once per frame
    void LoadLevel()
    {
        Debug.Log("Are they different? " + deckManagerScript.IsDeckDifferent);
        if (deckManagerScript.IsDeckDifferent)
        {
            Debug.Log("Do you want to save your Deck changes?");
            CardDataBase.SavePlayerDeck(deckManagerScript.GetDeck());
        }
        SceneManager.LoadScene("mainmenu");
    }
}
