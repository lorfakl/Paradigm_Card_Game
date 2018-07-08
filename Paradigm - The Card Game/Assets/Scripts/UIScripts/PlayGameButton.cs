using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayGameButton : MonoBehaviour
{
    public Button playgamebutton;
    // Use this for initialization
    void Start()
    {
        Button play = playgamebutton.GetComponent<Button>();
        play.onClick.AddListener(loadlevel);
    }

    // Update is called once per frame
    void loadlevel()
    {
        Debug.Log("Game not available, that being retooled");
        //SceneManager.LoadScene("playgame");

    }
}
