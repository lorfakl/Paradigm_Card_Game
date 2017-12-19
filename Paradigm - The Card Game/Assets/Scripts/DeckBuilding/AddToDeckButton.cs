using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddToDeckButton : MonoBehaviour {
    public Button DeckAddButton;
    public GameObject panel;
    public GameObject cardButtonPrefab;
    public GameObject manager;
    public float cardButtonoffset = 0f;

    Card cardToAdd;
    Deck playerDeck = new Deck();
	
    // Use this for initialization
	void Start ()
    {
        Button add2deck = DeckAddButton.GetComponent<Button>();
        add2deck.onClick.AddListener(addCardToDeck);
        playerDeck = manager.GetComponent<DeckManager>().getDeck();
        print("Deck get");
    }
	
	
	void addCardToDeck ()
    {
        if (playerDeck.addCard(cardToAdd))
        {
            addCardUI(cardButtonoffset);
        }
            //Debug.Log("Card Added");
        cardButtonoffset -= 20f;
        sendDeck();
        //Debug.Log("Y at next placement(kinda): " + cardButtonoffset);

        // Debug.Log("No Card Selected");
    }
    void addCardUI(float offset)
    {
        Vector3 gameObjectpos = panel.transform.position;
        Vector3 rectPosition = new Vector3(0f, 0f + offset);
        GameObject cardButton = Instantiate(cardButtonPrefab, gameObjectpos, Quaternion.identity);
        cardButton.GetComponent<CardInDeckButton>().setCardData(cardToAdd);
        cardButton.name = cardToAdd.getName();
        RectTransform cardRectLocation = cardButton.GetComponent<RectTransform>();
        cardRectLocation.SetParent(panel.transform, false);
        cardRectLocation.localPosition = rectPosition;
        cardButton.GetComponentInChildren<Text>().text = cardToAdd.getName();
        //Debug.Log("Y at placement: " + rectPosition.y);

    }

    public void sendDeck()
    {
        print("Card has been added deck size: " + playerDeck.getDeck().Count);

        foreach (Card c in playerDeck.getDeck())
        {
            print(c.getName());
        }
    }

    public void setCardToAdd(Card c)
    {
        cardToAdd = c;
    }
}
