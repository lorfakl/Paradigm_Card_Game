using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;

public class DeckManager : MonoBehaviour {

    // Use this for initialization
    List<Card> AllCards = new List<Card>();
    Deck playDeck;
    List<Card> initialDeck = new List<Card>();

    void Awake()
    {
        Player p = new Player();
        playDeck = p.PlayerDeck;
        initialDeck.AddRange(playDeck.GetContents());
        if(!CardDataBase.IsDataLoaded)
        {
            CardDataBase.GetDataBaseData();
        }
        
    }

    void Start ()
    {
        
    }

    void Update()
    {
        //GlobalPlayerDeck.setPlayerDeck(playDeck);
        //print("Curr Size: " + playDeck.Count);
        print("Inital deck size: " + initialDeck.Count);
    }
	
	public Deck GetDeck()
    {
        print("Deck get");
        return playDeck;

    }

	public List<Card> GetCards ()
    {
        return AllCards;
    }

    public bool IsDeckDifferent
    {
        get { return GetDeckChange(); }
    }

    private bool GetDeckChange()
    {
        if (initialDeck.Count == playDeck.Count)
        {
            for(int i =0; i < playDeck.Count; i++)
            {
                if(initialDeck[i] != playDeck.GetContents()[i])
                {
                    return true;
                }
            }

            return false;
        }
        else
        {
            return true;
        }
    }

    
}
