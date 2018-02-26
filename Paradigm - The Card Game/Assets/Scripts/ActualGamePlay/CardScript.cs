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



    private void Awake()
    {
        parent = gameObject.transform.parent;
        displayScript = gameObject.transform.parent.GetComponent<DisplaySelectionCards>();
        defaultCard = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
        selected = false;
        
    }

    // Use this for initialization
    void Start ()
    {
        print("CardScript Start");
        if (cardData == null)
        {
            throw new Exception("The Card's null dumbass!(CardScript Start)");
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
                    ChangeSprite(selected);
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
                print("Where?");
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
}
