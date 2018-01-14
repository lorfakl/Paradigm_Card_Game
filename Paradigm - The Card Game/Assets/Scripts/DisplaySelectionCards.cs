using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySelectionCards :MonoBehaviour
{

    //This script is for use any time the player needs to select Cards from a list
    //It should get the cards from somewhere, currently a global static class this is TEMPORARY 
    //It should display those cards, allow the player to select them and send the selected cards back to the sender
    public GameObject panel;
    public GameObject prefabCard;
    public Sprite selectedSprite;
    
    private List<GameObject> objectsMade = new List<GameObject>();
    private Vector3 PanelTopLeftCorner = new Vector3();
    private List<Card> selectedCards = new List<Card>();
    private Sprite unSelectSprite = null;
    private GameObject content = null;
    public void setSelectedCards(List<Card> l){ selectedCards = l; }
    public List<Card> getSelectedCards() { return selectedCards; }

    public void Awake()
    {
        Rect panelRec = panel.GetComponent<RectTransform>().rect;
        Vector3 temp = new Vector3(panelRec.xMin, panelRec.yMax);
        PanelTopLeftCorner = temp;
        content = panel.transform.GetChild(0).GetChild(0).gameObject;
        ShowCards(GlobalCardTransit.getCards().cardList, GlobalCardTransit.getCards().value);
    }

    private void Start()
    {
        if(selectedCards.Count < 1)
        {
            Debug.Log("Empty");
        }
    }

    void Update()
    { 
        StartCoroutine(selectCard()); 
    }

    public void ShowCards(List<Card> cardsToDisplay, int numToSelect) //TODO send the cards as a argument
    {
        List<Card> cardsToReturn = new List<Card>();
        Debug.Log(cardsToDisplay.Count);
        GameObject cardObject = null;
        Vector3 position = new Vector3(-62.2f, 25.2f);//
        float originalX = position.x;
        int r = 0;
        foreach (Card c in cardsToDisplay)
        {
            cardObject = CreateCardObject(c);
            objectsMade.Add(cardObject);
            Debug.Log("Pre Move: " + cardObject.transform.position);
            cardObject.transform.position = position;//move it
            if (r < 4)
            {
                r++;
                position.x += 24.4f;
                //Debug.Log("make new column");
            }
            else
            {
                position.y += -28.7f;
                position.x = originalX;
                r = 0;
                //Debug.Log("make new row");
            }
            Debug.Log("Post Move: " + cardObject.transform.position);
        }
    }

    GameObject CreateCardObject(Card c)
    {
        GameObject cardObject = GameObject.Instantiate(prefabCard, PanelTopLeftCorner, Quaternion.identity, GameObject.FindGameObjectWithTag("PlayField").transform); // GameObject.FindGameObjectWithTag("canvas").transform);
        cardObject.GetComponentInChildren<Canvas>().overrideSorting = true;
        cardObject.GetComponent<cardDetailsAddOn>().setExpandedCard(c);
        Text[] textEdit = cardObject.GetComponentsInChildren<Text>();
        textEdit[0].text = c.getName();
        textEdit[1].text = "";
        //for (int i = 0; i < c.getEffect().Length; i++)
        //{
        //  textEdit[1].text = textEdit[1].text + c.getEffect()[i];
        //}
        
        return cardObject;
    }

   public IEnumerator selectCard()
   {
        List<Card> cardsToReturn = new List<Card>();
        float scroll = 10f * Input.GetAxis("Mouse ScrollWheel");
        foreach (GameObject g in objectsMade)
        {
            g.transform.Translate(0, scroll, 0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject cardSelected = hit.transform.gameObject;
                Card cardData = cardSelected.GetComponent<cardDetailsAddOn>().getExpandedCard();
                if (cardSelected.GetComponentInChildren<SpriteRenderer>().sprite != selectedSprite)
                {
                    unSelectSprite = cardSelected.GetComponentInChildren<SpriteRenderer>().sprite;
                    cardSelected.GetComponentInChildren<SpriteRenderer>().sprite = selectedSprite;
                    Debug.Log("Change!");
                    //cardsToReturn.Add(cardData);
                    //s/etSelectedCards(cardsToReturn);
                    //Debug.Log("From Coroutine Selected Size: " + cardsToReturn.Count);
                    
                }
                else
                {
                    cardSelected.GetComponentInChildren<SpriteRenderer>().sprite = unSelectSprite;
                    //cardsToReturn.Remove(cardData);
                    //setSelectedCards(cardsToReturn);
                    
                }
            }//end Raycast IF
        }//end Input IF
        yield return new WaitForSeconds(.1f);
    }//end SelectCard*/
}
