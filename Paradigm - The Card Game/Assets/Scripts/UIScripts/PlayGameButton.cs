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
        play.onClick.AddListener(Loadlevel);
    }

    // Update is called once per frame
    void Loadlevel()
    {
        Debug.Log("Game not available, that being retooled");
        //SceneManager.LoadScene("playgame");

    }
}
