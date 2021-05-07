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
        search.onValueChanged.AddListener(delegate { CallDataBaseSearch(search.text, GetSearchMod()); }); 
        //adds a delagate to the onValue changed event so that when the text in the InputField changes a new 
        //search begins
                                                                                    
        
	}
    
    string GetSearchMod()
    {
        string name = "";
        foreach (Toggle t in searchMods.ActiveToggles())
        {
            name = t.name;
            
        }
        return name;
    }
	
	
	void CallDataBaseSearch (string searchVal, string searchMod)
    {
        SearchResults printResultsScript = searchResults.GetComponent<SearchResults>();
        if (!(String.IsNullOrEmpty(searchVal)))
        {
            results = CardDataBase.Search(searchVal, searchMod);
            printResultsScript.PrintSearchResults(results);
        }
        else
        {
            printResultsScript.ClearResults();
        }
    }

}
