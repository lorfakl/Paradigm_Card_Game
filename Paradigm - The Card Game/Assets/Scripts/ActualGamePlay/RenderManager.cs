using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using UnityEngine.UI;

public class RenderManager : MonoBehaviour
{
    private int[] playerInfo = { 0, 0, 0, 0, 0, 0, 0 };
    private string[] playerInfoTags = { "yourDeckCount","yourGraveCount","yourBarrierCount","enemyDeckCount","enemyGraveCount","enemyHandCount","enemyBarrierCount" };
    private string[] playerInfoPreambles = { "Deck: ", "Grave: ", "Barriers: ", "EDeck: ", "EGrave: ", "EHand: ", "EBarriers: " };
    private Text[] playerInfoObjects = new Text[7];
    private List<Card> displayHandContent = new List<Card>();
    private List<GameObject> objectsCreated = new List<GameObject>();
    private Transform parent;
    // Use this for initialization

    private void Awake()
    {
        GameEventsManager.UpdateUI += ChangePlayerInfo;
    }
    void Start ()
    {
        for(int i = 0; i < 7; i++)
        {
            playerInfoObjects[i] = GameObject.FindWithTag(playerInfoTags[i]).GetComponent<Text>();
        }

        parent = GameObject.FindWithTag("PlayField").transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    private void ChangePlayerInfo(int[] data, List<Card> cardsInHand)
    {
        for(int i = 0; i < 7; i++)
        {
            playerInfoObjects[i].text = playerInfoPreambles[i] + data[i];
        }

        displayHandContent = cardsInHand;
        //if (!displayHandContent.SequenceEqual(data))
        ShowCardsInHand(cardsInHand);
        //PositionCards();
    }

    private void ShowCardsInHand(List<Card> cardsToMake)
    {
        DestroyCards();
        int cardsMade = 0;
        Vector3 position = new Vector3();
        foreach (Card c in cardsToMake)
        {
            GameObject g = HelperFunctions.CreateCard(c, false, parent);
            objectsCreated.Add(g);
            cardsMade++;

            if(cardsMade > 1)
            {
                if(cardsMade == 2)
                {
                    position = g.transform.position;
                }
                position.x += 25;
                g.transform.position = position;
            }
        }
        
        
    }
    private void PositionCards()
    {
        Vector3 position = objectsCreated[0].transform.position;

        for(int i = 1; i < objectsCreated.Count; i++)
        {
            position.x += 20;
        }
    }
    private void DestroyCards()
    {
        foreach(GameObject g in objectsCreated)
        {
            Destroy(g);
        }
    }
}
