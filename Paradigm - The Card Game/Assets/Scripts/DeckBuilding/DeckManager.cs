using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;

public class DeckManager : MonoBehaviour {

    // Use this for initialization
    List<Card> AllCards = new List<Card>();
    Deck playDeck;

    void Awake()
    {
        Player p = new Player();
        playDeck = p.PlayerDeck;
    }

    void Start ()
    {
        
    }

    void Update()
    {
        //GlobalPlayerDeck.setPlayerDeck(playDeck);
        //print("Curr Size: " + playDeck.Count);
    }
	
	public Deck getDeck()
    {
        print("Deck get");
        return playDeck;

    }

	public List<Card> getCards ()
    {
        return AllCards;
    }
}
