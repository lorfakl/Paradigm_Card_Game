using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButtons : MonoBehaviour {

    public GameObject prefab;

    Card cardData;
    // Use this for initialization
    void Awake()
    {
        GameObject searchResultPanel = GameObject.FindGameObjectWithTag("SearchResultPanel");
        SearchResults searchResultScript = searchResultPanel.GetComponent<SearchResults>();
        cardData = searchResultScript.GetCardData();
    }

    void Start ()
    {
        Button card = prefab.GetComponent<Button>();
        card.onClick.AddListener(ExpandView);
      
    }
	
	
	void ExpandView ()
    {
        
        GameObject cardDets = GameObject.FindGameObjectWithTag("cardDetails");
        //GameObject cardArt = GameObject.FindGameObjectWithTag("cardArt"); NOTHING IN THE SCENE IS TAGGED WITH CARDART
        GameObject cardAdder = GameObject.FindGameObjectWithTag("addButton");
        //Card buttonCardData = prefab.GetComponent<searchResultAddOns>().cardData;
        Text details = cardDets.GetComponent<Text>();
        details.text = cardData.getName() + "\nKazoku: " + cardData.getFam().Name + "\t   Abilities: " + cardData.GetAbilityText();
        cardAdder.GetComponent<AddToDeckButton>().SetCardToAdd(cardData);
        Debug.Log(cardData.getName());
      
    }

   
    
}
