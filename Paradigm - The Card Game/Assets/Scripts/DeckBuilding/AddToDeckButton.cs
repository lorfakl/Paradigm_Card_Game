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
    Deck playerDeck;
	
    // Use this for initialization
	void Start ()
    {
        Button add2deck = DeckAddButton.GetComponent<Button>();
        add2deck.onClick.AddListener(AddCardToDeck);
        playerDeck = manager.GetComponent<DeckManager>().GetDeck();
        
        if(manager.GetComponent<DeckManager>().GetDeck().Count > 0)
        {
            Debug.Log("Something has been loaded");
            foreach(Card c in playerDeck.GetContents())
            {
                Debug.Log("Card UI to Add: " + c.getName());
                cardToAdd = c;
                AddCardUI(cardButtonoffset);
                cardButtonoffset -= 20f;
            }

        }
    }
	
	
	void AddCardToDeck ()
    {
        if (playerDeck.AddCard(cardToAdd))
        {
            AddCardUI(cardButtonoffset);
        }
            //Debug.Log("Card Added");
        cardButtonoffset -= 20f;
        UpdateDeck();
        //Debug.Log("Y at next placement(kinda): " + cardButtonoffset);

        // Debug.Log("No Card Selected");
    }
    void AddCardUI(float offset)
    {
        Vector3 gameObjectpos = panel.transform.position;
        Vector3 rectPosition = new Vector3(0f, 0f + offset);
        GameObject cardButton = Instantiate(cardButtonPrefab, gameObjectpos, Quaternion.identity);
        cardButton.GetComponent<CardInDeckButton>().SetCardData(cardToAdd);
        cardButton.name = cardToAdd.getName();
        RectTransform cardRectLocation = cardButton.GetComponent<RectTransform>();
        cardRectLocation.SetParent(panel.transform, false);
        cardRectLocation.localPosition = rectPosition;
        cardButton.GetComponentInChildren<Text>().text = cardToAdd.getName();
        //Debug.Log("Y at placement: " + rectPosition.y);

    }

    public void UpdateDeck()
    {
        print("Card has been added deck size: " + playerDeck.Count);

        foreach (Card c in playerDeck.GetContents())
        {
            print(c.getName());
        }
    }

    public void SetCardToAdd(Card c)
    {
        cardToAdd = c;
    }
}
