using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInDeckButton : MonoBehaviour {

    public Button cardInDeck;
    public Card cardData;

	// Use this for initialization
	void Start ()
    {
        cardInDeck.onClick.AddListener(ExpandCard);
	}

    // Update is called once per frame
    void ExpandCard()
    {
        GameObject removeCardButton = GameObject.Find("removeButton");
        //cardDets.GetComponent<Text>().text = cardData.getName() + "\nKazoku: " + cardData.getFam().getFam() + "\t   Ability: " + cardData.getAbility() +  "\nEffect: " + cardData.getEffect();
        RemoveCardButton removeCardButtonScript = removeCardButton.GetComponent<RemoveCardButton>();
        removeCardButtonScript.SetCardToRemove(cardData);
        print(cardData.getName() + "has been clicked");
    }

    public void SetCardData (Card c)
    {
        cardData= c;
	}

    public Card GetCardData()
    {
        return cardData;
    }
}
