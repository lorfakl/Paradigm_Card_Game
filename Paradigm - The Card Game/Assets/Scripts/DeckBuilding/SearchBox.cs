using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataBase;
using System;

public class SearchBox : MonoBehaviour {
    public ToggleGroup searchMods;
    public InputField searchBox;
    public Transform manager;
    public Transform searchResults;
    List<Card> results = new List<Card>();
    
    // Use this for initialization
    void Start ()
    {
        InputField search = searchBox.GetComponent<InputField>();
        search.onValueChanged.AddListener(delegate { callDataBaseSearch(search.text, getSearchMod()); });
        
	}
    
    string getSearchMod()
    {
        string name = "";
        foreach (Toggle t in searchMods.ActiveToggles())
        {
            name = t.name;
            
        }
        return name;
    }
	
	// Update is called once per frame
	void callDataBaseSearch (string searchVal, string searchMod)
    {
        DeckManager getCardsScript = manager.GetComponent<DeckManager>();
        List<Card> AllCards = getCardsScript.getCards();
        SearchResults printResultsScript = searchResults.GetComponent<SearchResults>();
        if (!(String.IsNullOrEmpty(searchVal)))
        {
            results = CardDataBase.search(searchVal, searchMod, AllCards);
            printResultsScript.printSearchResults();
        }
            else
            {
                printResultsScript.clearResults();
            }
        
    }

    public List<Card> getResults()
    {
        return results;
    }
}
