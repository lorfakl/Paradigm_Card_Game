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
    
    public void printSearchResults()
    {
        
        clearResults(createdButtons);
        SearchBox script = searchBox.GetComponent<SearchBox>();
        List<Card> results = script.getResults();
        float deltaY = 0f;
        foreach (Card card in results)
        {
            GameObject made = makeButtonsForResults(deltaY);
            made.GetComponentInChildren<Text>().text = card.getName();
            made.GetComponent<searchResultAddOns>().setCardData(card);  
            createdButtons.Add(made);
            deltaY -= 40f;
            
        }
        
    }

    GameObject makeButtonsForResults(float offset)
    {
        
        Vector3 pos = this.gameObject.transform.position;
        Vector3 loc = new Vector3(-2.3842e-06f, 0f + offset);
        GameObject newButton = Instantiate(result, pos, Quaternion.identity); 
        RectTransform newrec = newButton.GetComponent<RectTransform>();
        newrec.SetParent(panel.transform, false);
        newrec.localPosition = loc;
        
        return newButton;
    }

    public void clearResults()
    {
        clearResults(createdButtons);
    }


    void clearResults(List<GameObject> createdButtons)
    {
        foreach(GameObject button in createdButtons)
        {
            GameObject.Destroy(button.gameObject);
            //Debug.Log("gone");
        }
        createdButtons.Clear();
    }

    
}
