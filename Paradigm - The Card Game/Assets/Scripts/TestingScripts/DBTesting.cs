using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBase;
using Utilities;

public class DBTesting : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        CardDataBase.GetDataBaseData();
        List<Card> createdCards = CardDataBase.GetAllCards();
        foreach(Card c in createdCards)
        {
            Debug.Log("Name: " + c.getName());
            Debug.Log("Fam: " + c.getFam().FamString);
            foreach(Trait t in c.getTraits())
            {
                Debug.Log("Trait: " + t.TraitText);
            }

            foreach (Ability t in c.getAbilities())
            {
                Debug.Log("Ability: " + t.AbilityText);
            }
            
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
