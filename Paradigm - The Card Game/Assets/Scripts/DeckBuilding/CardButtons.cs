using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButtons : MonoBehaviour {

    public GameObject prefab;

    // Use this for initialization
    void Start ()
    {
        Button card = prefab.GetComponent<Button>();
        card.onClick.AddListener(expandView);
      
    }
	
	
	void expandView ()
    {
        
        GameObject cardDets = GameObject.FindGameObjectWithTag("cardDetails");
        //GameObject cardArt = GameObject.FindGameObjectWithTag("cardArt");
        GameObject cardAdder = GameObject.FindGameObjectWithTag("addButton");
        Card buttonCardData = prefab.GetComponent<searchResultAddOns>().cardData;
        Text details = cardDets.GetComponent<Text>();
        //details.text = buttonCardData.getName() + "\nKazoku: " + buttonCardData.getFam().getFam() + "\t   Ability: " + buttonCardData.getAbility() + "\nEffect: " + buttonCardData.getEffect();
        cardAdder.GetComponent<AddToDeckButton>().setCardToAdd(buttonCardData);
        //Debug.Log(buttonCardData.getName());
      
    }

   
    
}
