using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RemoveCardButton : MonoBehaviour {

    public Button removebutton;
    public GameObject cardDets;
    public GameObject manager;
    Deck playerDeck;
    Card cardMarkedForRemoval;
    
    // Use this for initialization
	void Start ()
    {
        removebutton.onClick.AddListener(RemoveCard);
        playerDeck = manager.GetComponent<DeckManager>().GetDeck();
	}
	
	// Update is called once per frame
	void RemoveCard()
    {
        print("To be deleted "+ cardMarkedForRemoval.getName());
        GameObject buttonToDelete = GameObject.Find(cardMarkedForRemoval.getName());
        playerDeck.RemoveCard(cardMarkedForRemoval);
        GameObject.Destroy(buttonToDelete);
    }

    public void SetCardToRemove(Card c)
    {
        cardMarkedForRemoval = c;
    }
}
