using System;
using UnityEngine;
using Utilities;

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
        //print("THis is the Awake function");
        selected = false;
        
    }

    // Use this for initialization
    void Start ()
    {
        if (cardData == null) //this should never be true, if it is ya done goofed kid
        {
            HelperFunctions.Error("The Card's null dumbass!(CardScript Start)");
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

        if (Input.GetMouseButtonDown(0)) //If the mouse button is clicked
        {
            Ray rayLine = Camera.main.ScreenPointToRay(Input.mousePosition); //cast a ray from the camera to the mouse position
            RaycastHit objectHit; //gameobject the ray hit

            if (Physics.Raycast(rayLine, out objectHit)) //if the ray hits a collider
            {
                if (objectHit.transform == gameObject.transform) //check the object hit if it contains this script
                {
                    if (displayMode) //if the script is in displayMode, for selection from the overlay
                    {
                        selected = !selected; //invert selection bool
                        ChangeSprite(selected); //update the selection status sprite
                    }
                    else
                    {
                        this.cardData.PlayCard();
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
    private void OnDestroy()
    {
        print("Its been destroyed");
    }

    private void SetMode(bool mode)
    {
        this.displayMode = mode;
        isSetModeCalled = true;
    }
}
