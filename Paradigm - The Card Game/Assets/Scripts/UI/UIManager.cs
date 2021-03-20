using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UITarget
{
    Deck,
    Hand,
    Field,
    Grave
}



public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEventsManager.NotifySubsOfEvent += CheckForUIEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckForUIEvent(object sender, GameEventsArgs e)
    {
        if(e.IsUIEvent)
        {
            print("UI manager Caught a UI event");
            print("UI Event Data: ");
            e.Print();
        }
        
    }
}
