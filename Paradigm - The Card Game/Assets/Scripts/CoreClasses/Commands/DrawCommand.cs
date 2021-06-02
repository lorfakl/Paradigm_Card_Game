using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using DG.Tweening;

public class DrawCommand : ICommand
{
    UIScriptableObject drawUIEffects;
    private System.Collections.ObjectModel.ObservableCollection<GameObject> cardsCreated;
    private GameObject mainHandspace;
    private GameObject otherHandSpace;


    public DrawCommand(UIScriptableObject uIScriptableObject)
    {
        drawUIEffects = uIScriptableObject;
        mainHandspace = GameObject.FindGameObjectWithTag("playerHand");
        otherHandSpace = GameObject.FindGameObjectWithTag("enemyHand");

    }

    public IEnumerator Execute(GameEventsArgs g)
    {
        if(g.EventOwner.Type == PlayerType.MainHuman)
        {
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
        Player owner = e.EventOwner;
        GameObject startPoint = drawUIEffects.UiEntryPoint;
        bool isFaceDown = false;
        GameObject handspace;
        if (owner.Type == PlayerType.MainHuman)
        {
            handspace = mainHandspace;
        }
        else
        {
            handspace = otherHandSpace;
            isFaceDown = true;
        }

        Tween LeftMovingTween = null;

        if (cardsCreated.Count > 0)
        {
            foreach (GameObject c in cardsCreated)
            {
                //c.GetComponent<CardScript>().Print();
                LeftMovingTween = c.transform.DOMoveX(c.transform.position.x + UIManager.CardWidth, 0.1f);
                //HelperFunctions.Print("Moving to: " + c.transform.position.x + UIManager.CardWidth);
                //HelperFunctions.Print("Should move " + cardsCreated.Count + " to the left");
            }
        }


        GameObject cardObject = HelperFunctions.CreateCard(e.EventOriginCard, false, handspace.transform, isFaceDown);

        cardsCreated.Add(cardObject);

        if (cardsCreated.Count > 2)
        {
            //HelperFunctions.Print("I dont know whats happening");
            cardObject.transform.DOMoveX(cardObject.transform.position.x - ((UIManager.CardWidth / 2) * (cardsCreated.Count - 2)), 0.1f);
        }
        //HelperFunctions.Print("What is UIManager.CardWidth: " + UIManager.CardWidth);
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
        
        if (cardsCreated.Count != 1)
        {
            foreach (GameObject c in cardsCreated)
            {
                //c.GetComponent<CardScript>().Print();
                c.transform.DOMoveX(c.transform.position.x - (UIManager.CardWidth / 2f), drawUIEffects.MoveSpeed);
                //HelperFunctions.Print("Should move " + cardsCreated.Count + " to the right");
            }
        }
    }

}
