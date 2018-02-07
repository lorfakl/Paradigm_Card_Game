using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchResults : MonoBehaviour {

    // Use this for initialization
    public GameObject result;
    public Transform searchBox;
    public GameObject panel;
    List<GameObject> createdButtons = new List<GameObject>();
    private static Card searchResultCardData;
    
    public void PrintSearchResults(List<Card> results)
    {
        ClearResults(createdButtons);
        float deltaY = 0f;
        foreach (Card card in results)
        {
            searchResultCardData = card;
            GameObject made = MakeButtonsForResults(deltaY);
            made.GetComponentInChildren<Text>().text = card.getName();
            //made.GetComponent<searchResultAddOns>().setCardData(card);
            Debug.Log(card.GetAbilityText());
            createdButtons.Add(made);
            deltaY -= 40f;
            
        }
        
    }

    GameObject MakeButtonsForResults(float offset)
    {
        
        Vector3 pos = this.gameObject.transform.position;
        Vector3 loc = new Vector3(-2.3842e-06f, 0f + offset);
        GameObject newButton = Instantiate(result, pos, Quaternion.identity); 
        RectTransform newrec = newButton.GetComponent<RectTransform>();
        newrec.SetParent(panel.transform, false);
        newrec.localPosition = loc;
        
        return newButton;
    }

    public void ClearResults()
    {
        ClearResults(createdButtons);
    }

    public Card GetCardData()
    {
        return searchResultCardData;
    }

    void ClearResults(List<GameObject> createdButtons)
    {
        foreach(GameObject button in createdButtons)
        {
            GameObject.Destroy(button.gameObject);
            //Debug.Log("gone");
        }
        createdButtons.Clear();
    }

    
}
