using System;
using UnityEngine;

public class CardScript : MonoBehaviour {

    public Sprite cardSelected;

    private Sprite defaultCard;
    private bool selected;
    private DisplaySelectionCards displayScript;
    private Transform parent;
    private Card cardData;
    private static int numToSelect;
    private bool displayMode;
    private bool isSetModeCalled;
    /*Dont know if there will be more than 2 modes
    private string mode;
    private string [] modes = {"display", }
    */

    private void Awake()
    {
        parent = gameObject.transform.parent;
        defaultCard = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
        selected = false;
        
    }

    // Use this for initialization
    void Start ()
    {
        if (cardData == null) //this should never be true, if it is ya done goofed kid
        {
            throw new Exception("The Card's null dumbass!(CardScript Start)");
        }

        if(isSetModeCalled == false)
        {
            throw new Exception("You didnt do a SendMessage call to SetMode");
        }

        if(displayMode)
        {
            displayScript = gameObject.transform.parent.GetComponent<DisplaySelectionCards>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
            Ray rayLine = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit objectHit;

            if(Physics.Raycast(rayLine, out objectHit))
            {
                if(objectHit.transform == gameObject.transform)
                {
                    selected = !selected;
                    if(displayMode)
                    {
                        ChangeSprite(selected);
                    }
                }
            }
        }

    }

    private void ChangeSprite(bool s)
    {
        if (s)
        {
            if(displayScript.CardsSelected < displayScript.TotalCards)
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = cardSelected;
                numToSelect = displayScript.UpdateSelectedCards(cardData, true);
                return;
            }

            selected = !selected; //set the selected bool to it's pre-clicked value, because the user is no longer able
                                  //to select cards. Only de-select
        }
        else
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = defaultCard;
            numToSelect = displayScript.UpdateSelectedCards(cardData, false);
        }
    }

    public void SetCard(Card c)
    {
        if (c == null)
        {
            throw new Exception("The Card's null dumbass!(SetCard)");  
        }
        else
        {
            this.cardData = c;
            //print(c.Name);
        }
    }

    private void SetMode(bool mode)
    {
        this.displayMode = mode;
        isSetModeCalled = true;
    }
}
