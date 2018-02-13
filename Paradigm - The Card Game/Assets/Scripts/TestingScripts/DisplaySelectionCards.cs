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
    private Vector3 position = new Vector3();
    private static List<GameObject> objectsCreated = new List<GameObject>();
    private static Location source;
    private static Location destination;
    private static int numToMove = 0;

    private void Awake()
    {
        display = this.gameObject;
        parent = display.transform;
    }

    private void Start()
    {
        print("Start!");
        DisplayCards();
    }

    private void Update()
    {
        
    }

    private void DisplayCards()
    {
        print(numToMove);
        if (source != null && destination != null)
        {
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
        Transform cardName = cardObject.transform.Find("cardName");
        cardName.GetComponent<Text>().text = c.Name;
        Transform content = cardObject.transform.FindDeepChild("Content");
        content.GetComponent<Text>().text = c.GetAbilityText();
        position = cardObject.transform.position;
        MoveCard(cardObject);
    }

    private void MoveCard(GameObject c)
    {
        

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

}

public static class TransformDeepChildExtension
{
    //Breadth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }
}
