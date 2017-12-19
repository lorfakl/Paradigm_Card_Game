using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RemoveCardButton : MonoBehaviour {

    public Button removebutton;
    public GameObject cardDets;
    public GameObject manager;
    Deck playerDeck;
    
    // Use this for initialization
	void Start ()
    {
        removebutton.onClick.AddListener(removeCard);
        playerDeck = manager.GetComponent<DeckManager>().getDeck();
	}
	
	// Update is called once per frame
	void removeCard()
    {
        Card cardToRemove = cardDets.GetComponent<cardDetailsAddOn>().getExpandedCard();
        print("To be deleted "+ cardToRemove.getName());
        GameObject buttonToDelete = GameObject.Find(cardToRemove.getName());
        playerDeck.removeCard(cardToRemove);
        GameObject.Destroy(buttonToDelete);
        
    }
}
