using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;


public class JustTesting : MonoBehaviour
{
    public GameObject display;
   
    List<Card> cards = null;
    int numToSelect = 0;
    int count = 0;
	void Awake ()
    {
        MakeTestPlayer.MakePlayer();
        CardDataBase.GetDataBaseData();
        List<Card> AllCards = CardDataBase.GetAllCards();

        int size = Random.Range(1, AllCards.Count - 1);
        List<Card> randomCards = new List<Card>();

        for(int i = 0; i < size; i++)
        {
            MakeTestPlayer.P1.PlayerDeck.AddCard(AllCards[Random.Range(0, AllCards.Count - 1)]);
        }

        numToSelect = Random.Range(1, 10);
        Debug.Log("Num To Select: " + numToSelect);
        cards = randomCards;
        Debug.Log(size);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log("Num To Select: " + numToSelect);
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Debug.Log("Made the thing");
            GameObject overlay = Instantiate(display) as GameObject;
            display.GetComponentInChildren<DisplaySelectionCards>().SetCardPath(MakeTestPlayer.P1.GetLocation("Deck"),
                                                            MakeTestPlayer.P1.GetLocation("Hand"), numToSelect);

            Debug.Log("Pressed the enter key, good job");
        }

        print("Hand size" + MakeTestPlayer.P1.GetLocation("Hand").Count);
     
	}
}
