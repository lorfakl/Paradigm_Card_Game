using DG.Tweening;
using System;
using UnityEngine;
using Utilities;

public class CardScript : MonoBehaviour {

    public Sprite cardSelected;
    public Vector3 scaleFactor = new Vector3(2, 2, 2);
    

    private Vector3 defaultScale, defaultPosition;
    private Sprite defaultCard;
    private bool selected;
    private DisplaySelectionCards displayScript;
    private Transform parent;
    private Card cardData;
    private static int numToSelect;
    private bool displayMode;
    private bool hasScrolled;
    private bool isSetModeCalled;


    public Card Card { get{ return cardData; } }
    /*Dont know if there will be more than 2 modes
    private string mode;
    private string [] modes = {"display", }
    */

    #region Unity Callbacks

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

        
        
        defaultScale = transform.localScale;
        print("What is the local scale: " + transform.localScale);
        
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
                    print("You hit a card with a click");
                    if (displayMode) //if the script is in displayMode, for selection from the overlay
                    {
                        print("Is display mode enabled: " + displayMode);
                        selected = !selected; //invert selection bool
                        ChangeSprite(selected); //update the selection status sprite
                    }
                    else
                    {
                        this.cardData.PlayCard();
                        print("PlayCard was called. Card type was: " + cardData.GetType().ToString());
                    }
                }
            }
        }
        
    }

    private void OnDestroy()
    {
        print("Its been destroyed");
    }

    private void OnMouseOver()
    {
        if (!displayMode && !hasScrolled)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                print("Mouse Scroll Data: " + Input.mouseScrollDelta.y);
                defaultPosition = transform.position;
                print("Mouse is in");
                Vector3 targetValueDoScale = Vector3.Scale(transform.localScale, scaleFactor);
                transform.DOScale(targetValueDoScale, 0.25f);
                transform.DOMove(transform.position + new Vector3(0.0f, 3f, 0.0f), 0.25f);
                hasScrolled = true;
            }
        }
    }


    private void OnMouseEnter()
    {
        if(!displayMode)
        {
            /*defaultPosition = transform.position;
            print("Mouse is in");
            Vector3 targetValueDoScale = Vector3.Scale(transform.localScale, scaleFactor);
            transform.DOScale(targetValueDoScale, 0.25f);
            transform.DOMove(transform.position + new Vector3(0.0f, 3f, 0.0f), 0.25f);*/

        }


    }

    private void OnMouseExit()
    {
        if (!displayMode && hasScrolled)
        {
            hasScrolled = false;
            print("Mouse is out");
            Vector3 targetValueDoScale = Vector3.Scale(transform.localScale, Vector3.one);
            transform.DOScale(defaultScale, 0.25f);
            transform.DOMove(defaultPosition, 0.25f);
        }
        

    }

    #endregion

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

    public void Print()
    {
        print(this.cardData);
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
