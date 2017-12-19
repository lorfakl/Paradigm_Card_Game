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
        cardInDeck.onClick.AddListener(expandCard);
	}

    // Update is called once per frame
    void expandCard()
    {
        GameObject cardDets = GameObject.FindGameObjectWithTag("cardDetails");
        cardDets.GetComponent<Text>().text = cardData.getName() + "\nKazoku: " + cardData.getFam().getFam() + "\t   Ability: " + cardData.getAbility() +  "\nEffect: " + cardData.getEffect();
        cardDets.GetComponent<cardDetailsAddOn>().setExpandedCard(cardData);
        print("From in the Deck: "+cardDets.GetComponent<cardDetailsAddOn>().getExpandedCard().getName());
    }

    public void setCardData (Card c)
    {
        cardData= c;
	}

    public Card getCardData()
    {
        return cardData;
    }
}
