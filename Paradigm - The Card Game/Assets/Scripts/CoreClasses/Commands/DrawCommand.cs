using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using DG.Tweening;

public class DrawCommand : ICommand
{
    UIScriptableObject drawUIEffects;
    private List<GameObject> cardsCreated;
    private GameObject handspace;


    public DrawCommand(UIScriptableObject uIScriptableObject)
    {
        drawUIEffects = uIScriptableObject;
        handspace = GameObject.FindGameObjectWithTag("playerHand");
   
    }

    public IEnumerator Execute(GameEventsArgs g)
    {
        if(g.EventOwner.Type == PlayerType.Human)
        {
            Debug.LogWarning("This Player type should be change MainHuman");
            cardsCreated = UIManager.HandCards;
        }
        else
        {
            cardsCreated = UIManager.OtherHandCards;
        }

        //UIManager.ShowCardMovement((UiEvents)g);
        ShowDraw((UiEvents)g);
        yield return TweenWaitTime();
        Rearrange();
    }

    private void ShowDraw(UiEvents e)
    {

        float xMove = -.16f * 18;
        //if(nextCardSlot  0)
        HelperFunctions.Print("Cards created: " + cardsCreated.Count);
        Player owner = e.EventOwner;
        GameObject startPoint = drawUIEffects.UiEntryPoint;
        if (owner.Type == PlayerType.AI)
        {
            Debug.LogWarning("Please fix the Card bug, this if statement is just while that bug exists");
            Debug.LogWarning("Grabbing UI Player start point instead of NonUI");
        }

        Tween LeftMovingTween = null;

        if (cardsCreated.Count > 0)
        {
            foreach (GameObject c in cardsCreated)
            {
                c.GetComponent<CardScript>().Print();
                LeftMovingTween = c.transform.DOMoveX(c.transform.position.x + xMove, 0.1f);
                HelperFunctions.Print("Moving to: " + c.transform.position.x + xMove);
                HelperFunctions.Print("Should move " + cardsCreated.Count + " to the left");
            }
        }


        GameObject cardObject = HelperFunctions.CreateCard(e.EventOriginCard, false, handspace.transform);

        cardsCreated.Add(cardObject);

        if (cardsCreated.Count > 2)
        {
            HelperFunctions.Print("I dont know whats happening");
            cardObject.transform.DOMoveX(cardObject.transform.position.x - ((xMove / 2) * (cardsCreated.Count - 2)), 0.1f);
        }
        HelperFunctions.Print("What is xMove: " + xMove);






        //cardObject.transform.DOMove(newPosition, 1.5f);
        //cardObject.transform.parent = handspace.transform;


        if (drawUIEffects.Scale == null)
        {
            HelperFunctions.ScaleCard(cardObject, new Vector3(30, 30, 0));
        }
        else
        {
            HelperFunctions.ScaleCard(cardObject, drawUIEffects.Scale);
        }

    }



    private IEnumerator TweenWaitTime()
    {
        yield return new WaitForSeconds(drawUIEffects.MoveSpeed);
    }

    public void Rearrange()
    {
        float xMove = -.16f * 18;

        if (cardsCreated.Count != 1)
        {
            foreach (GameObject c in cardsCreated)
            {
                c.GetComponent<CardScript>().Print();
                c.transform.DOMoveX(c.transform.position.x - (xMove / 2f), drawUIEffects.MoveSpeed);
                HelperFunctions.Print("Should move " + cardsCreated.Count + " to the right");
            }
        }
    }

}
