using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;


public class JustTesting : MonoBehaviour {

    /*public Transform display;
    List<Card> cards = null;
    int numToSelect = 0;
    int count = 0;
	void Awake ()
    {
        List<Card> AllCards = new List<Card>();
        TextAsset cardCollection = Resources.Load("carddatabase") as TextAsset;
        string[] lines = cardCollection.text.Split("\n"[0]);
        AllCards = CardDataBase.LoadData(lines);

        int size = Random.Range(1, AllCards.Count);
        List<Card> randomCards = new List<Card>();

        for(int i = 0; i < size; i++)
        {
            randomCards.Add(AllCards[Random.Range(0, AllCards.Count)]);
            string name = randomCards[i].GetType().Name;
            Debug.Log(randomCards[i].GetType());
            Debug.Log("Type in a string " + name);
        }
        numToSelect = Random.Range(1, 10);
        Debug.Log("Num To Select: " + numToSelect);
        cards = randomCards;
        GlobalCardTransit.sendCards(randomCards, numToSelect);
        Debug.Log(size);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log("Num To Select: " + numToSelect);
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Debug.Log("Made the thing");
            GameObject overlay = Instantiate(Resources.Load("SelectionOverlay", typeof(GameObject))) as GameObject;

            //DisplaySelectionCards.ShowCards(cards, numToSelect);
            
            //DisplaySelectionCards.selectCard(ref numToSelect);
            ///DisplaySelectionCards displayScript = display.GetComponent<DisplaySelectionCards>();
            //List<Card> returnedCards = displayScript.ShowCards(cards);
            Debug.Log("Pressed the enter key, good job");
        }
     
        //StartCoroutine(DisplaySelectionCards.selectCard());
        
        if (count < numToSelect)
        {
            //count = count + DisplaySelectionCards.getSelectedCards().Count;
            Debug.Log("Cards selected over time: " + count);
        }
	}*/
}
