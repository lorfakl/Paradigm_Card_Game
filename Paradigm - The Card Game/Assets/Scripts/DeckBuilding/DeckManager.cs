using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;

public class DeckManager : MonoBehaviour {

    // Use this for initialization
    List<Card> AllCards = new List<Card>();
    Deck playDeck = GlobalPlayerDeck.getPlayerDeck();
    void Start ()
    {
        TextAsset cardCollection = Resources.Load("carddatabase") as TextAsset;
        string[] lines = cardCollection.text.Split("\n"[0]);
        AllCards = CardDataBase.LoadData(lines);
    }

    void Update()
    {
        GlobalPlayerDeck.setPlayerDeck(playDeck);
        //print("Curr Size: " + playDeck.getDeck().Count);
    }
	
	public Deck getDeck()
    {
        //print("Deck get");
        return playDeck;

    }

	public List<Card> getCards ()
    {
        return AllCards;
    }
}
