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
    private List<GameObject> objectsCreated = new List<GameObject>();
    private List<Card> selectedCards = new List<Card>();
    private Location source;
    private Location destination;
    private int numToMove = 0;
    private int leftToMove = 0;
    private bool isDoneSelecting = false;
    public delegate void NotifyDoneChoosing(object sender, Location source);
    public static event NotifyDoneChoosing IsDoneChoosing; //for multiplayer this cant be static

    //Public Properties, mainly for CardScript
    public int CardsSelected
    {
        get { return selectedCards.Count; }
    }

    public int TotalCards
    {
        get { return numToMove; }
    }

    public int CardsLeft
    {
        get { return leftToMove; }
    }

    public List<Card> SelectedCards
    {
        get { return this.selectedCards; }
    }


    /// <summary>
    /// This function needs to be called by the script instantiating this display IMMEDIATELY after the instantiation. 
    /// In order to set the "Address" of the cards, setting the From Location and To Location.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="d"></param>
    /// <param name="n"></param>
    public void SetCardPath(Location s, Location d, int n)
    {
        if (s == null || d == null)
        {
            print("well fuck...");
        }
        //print("setsardpathnum of cards in source: " + source.Count);
        source = s;
        destination = d;
        numToMove = n;
        leftToMove = n;
        //print("Called it");
    }

    /// <summary>
    /// Called Explicitly from the CardScript class, all it does is let this display know how many cards the user has selected
    /// </summary>
    /// <param name="c"></param>
    /// <param name="added"></param>
    /// <returns></returns>
    public int UpdateSelectedCards(Card c, bool add)
    {
        if (c == null)
        {
            //throw new Exception("The Card's null dumbass!");
        }

        if (add)
        {
            selectedCards.Add(c);
            leftToMove = numToMove - selectedCards.Count;
        }
        else
        {
            if (selectedCards.Remove(c))
            {
                leftToMove = numToMove - selectedCards.Count;
            }
            else
            {
                return leftToMove;
            }
        }
        return leftToMove;
    }

    public bool SendSelectionEnd()
    {
        PlayerInteraction playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerInteraction>();
        Player p = playerScript.CurrentPlayer;
        p.TimeLeftOnTimer = 0;
        isDoneSelecting = true;
        return true;
    }

    //Private Functions
    private void Awake()
    {
        display = this.gameObject;
        parent = display.transform;
    }

    private void Start()
    {
        //print("Start!");
        canvas = gameObject.transform.parent.parent;
        canvas.Find("Button").GetComponent<Button>().onClick.AddListener(StopSelecting);
        DisplayCards();
    }

    private void Update()
    {
        //print("Total Cards to Select: " + numToMove);
        //print("Cards Selected: " + selectedCards.Count);
        print("Number of Cards left to select: " + leftToMove);

        if (isDoneSelecting)
        {
            source.MoveContent(selectedCards, destination);
            print(destination.Count);
            IsDoneChoosing(this, destination);
            Destroy(canvas.gameObject);
        }
    }

    private void DisplayCards()
    {
        if (source != null && destination != null)
        {                                          
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
        //cardPrefab.GetComponent<CardScript>().SetCard(c);
        GameObject cardObject = Instantiate(cardPrefab, parent) as GameObject;
        cardObject.SendMessage("SetMode", true);
        cardObject.GetComponent<CardScript>().SetCard(c);
        objectsCreated.Add(cardObject);
        Transform cardName = cardObject.transform.FindDeepChild("cardName");
        cardName.GetComponent<Text>().text = c.Name;
        Transform content = cardObject.transform.FindDeepChild("Content");
        content.GetComponent<Text>().text = c.GetAbilityText();
        if (c == null)
        {
            throw new Exception("The Card's null dumbass!(CreateCard)");
        }
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
        SendSelectionEnd();
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
