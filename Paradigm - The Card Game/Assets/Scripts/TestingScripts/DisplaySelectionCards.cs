using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplaySelectionCards :MonoBehaviour
{

    //This script is for use any time the player needs to select Cards from a list
    //It should get the cards from somewhere, Likely a location object
    //It should display those cards, allow the player to select them and send the selected cards back to the sender
    public GameObject cardPrefab;
    private GameObject display;
    private Transform parent;
    private Transform canvas;
    private Vector3 position = new Vector3();
    private static List<GameObject> objectsCreated = new List<GameObject>();
    private static List<Card> selectedCards = new List<Card>();
    private static Location source;
    private static Location destination;
    private static int numToMove = 0;
    private static bool isDoneSelecting = false;

    public bool IsDoneSelecting
    {
        get { return isDoneSelecting; }
    }

    private void Awake()
    {
        display = this.gameObject;
        parent = display.transform;
        
    }

    private void Start()
    {
        print("Start!");
        canvas = gameObject.transform.parent.parent;
        canvas.Find("Button").GetComponent<Button>().onClick.AddListener(StopSelecting);
        DisplayCards();
    }

    private void Update()
    {
        print("Cards Selected: " + selectedCards.Count);
        print("Number of Cards left to select: " + numToMove);
        foreach (Card c in selectedCards)
        {
            print(c.Name);
        }

        if (isDoneSelecting)
        {
            source.MoveContent(selectedCards, destination);
            Destroy(canvas.gameObject);

        }
    }

    private void DisplayCards()
    {
        if (source != null && destination != null) //this is where the error is, it says they're both null even if 
        {                                          //they're not
            //display cards
            foreach(Card c in source.GetContents())
            {
                CreateCard(c);
            }
        }
        else
        {
            throw new Exception("The Source and/or Destination Location is null, did you forget to call " +
                                                                                    "DisplaySelectionCards.SetCardPath?");
        }
    }

    private void CreateCard(Card c)
    {
        GameObject cardObject = Instantiate(cardPrefab, parent) as GameObject;
        objectsCreated.Add(cardObject);
        Transform cardName = cardObject.transform.FindDeepChild("cardName");
        cardName.GetComponent<Text>().text = c.Name;
        Transform content = cardObject.transform.FindDeepChild("Content");
        content.GetComponent<Text>().text = c.GetAbilityText();
        if (c == null)
        {
            throw new Exception("The Card's null dumbass!(CreateCard)");
        }
        cardPrefab.GetComponent<CardScript>().SetCard(c);
        position = cardObject.transform.position;
        ScaleCard(cardObject);
    }

    private void ScaleCard(GameObject c)
    {
        RectTransform rectTrans = c.GetComponent<RectTransform>();
        rectTrans.localScale += new Vector3(30, 30, 0);
    }

    private void StopSelecting()
    {
        isDoneSelecting = true;
    }

    public void SetCardPath(Location s, Location d, int n)
    {
        if(s == null || d == null)
        {
            print("well fuck...");
        }

        source = s;
        destination = d;
        print(source.Name);
        print(destination.Name);
        numToMove = n;
        print("Called it");
    }

    public int UpdateSelectedCards(Card c, bool added)
    {
        if(c == null)
        {
            //throw new Exception("The Card's null dumbass!");
        }

        if(added)
        {
            selectedCards.Add(c);
            numToMove--;
        }
        else
        {
            if(selectedCards.Remove(c))
            {
                numToMove++;
            }
            else
            {
                return numToMove;
            }
        }
        return numToMove;
    }

}

public static class TransformDeepChildExtension
{
    public static Transform FindDeepChild(this Transform parent, string name)
    {
        Transform child = parent.Find(name);
        if(child == null)
        {
            foreach(Transform c in parent)
            {
                child = c.FindDeepChild(name);
                if(child != null)
                {
                    return child;
                }
            }

            return null;
        }
        else
        {
            return child;
        }
    }
}
