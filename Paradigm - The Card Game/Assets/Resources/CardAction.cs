using UnityEngine;
using System.Collections;
using Utilities;

public class CardAction 
{

    private Ability ability;
    private Player playerOwner;
    private Card card;
    private string action;

    public CardAction(Ability a, string [] actionComponents)
    {
        card = a.CardOwner;
        playerOwner = a.CardOwner.Owner;
        action = actionComponents[0]; 
    }

    public void PerformAction()
    {

        
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
