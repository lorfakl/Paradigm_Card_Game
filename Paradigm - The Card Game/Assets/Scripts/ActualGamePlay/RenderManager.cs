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
    private List<Card> uiHandContent = new List<Card>();
    private List<Card> uiFieldContent = new List<Card>();
    private List<Card> noUiHandContent = new List<Card>();
    private List<Card> noUiFieldContent = new List<Card>();
    private List<GameObject> objectsCreated = new List<GameObject>();
    private Player p1;
    private Player p2;
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

        if(p1 == null)
        {
            throw new Exception("Eyy buddy you didnt make a send mesage call when instaniating the RenderManager, what up with that?");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    private void ChangePlayerInfo(int[] data, List<Card> cardsInHand, List<Card> cardsOnField, List<Card> nonUiCardsInHand, List<Card> nonUiCardsOnField)
    {
        for(int i = 0; i < 7; i++)
        {
            playerInfoObjects[i].text = playerInfoPreambles[i] + data[i];
        }
        List<List<Card>> newData = new List<List<Card>> {cardsInHand, cardsOnField, nonUiCardsInHand, nonUiCardsOnField };
        List<List<Card>> oldData = new List<List<Card>> { uiHandContent, uiFieldContent, noUiHandContent, noUiFieldContent };
        List<String> tags = new List<string> { "playerHand", "playerField", "enemyHand", "enemyField" };

        for(int i = 0; i < 4; i++)
        {
            oldData[i] = newData[i];
            GameObject parent = GameObject.FindWithTag(tags[i]);
            if (parent != null)
            {
                ShowCards(oldData[i], parent.transform);
            }
        }

    }

    private void ShowCards(List<Card> cardsToMake, Transform parent)
    {
        //DestroyCards();
        print("Show Cards was called");
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

    public void SetPlayers(List<Player> players)
    {
        p1 = players[0];
        p2 = players[1];
    }

    private void DestroyCards()
    {
        if (objectsCreated.Count > 0)
        { 
            foreach (GameObject g in objectsCreated)
            {
                Destroy(g);
            }
            objectsCreated.Clear();
        }
    }
}
