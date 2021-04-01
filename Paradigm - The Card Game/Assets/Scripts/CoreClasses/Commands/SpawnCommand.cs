using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Utilities;


public class SpawnCommand : ICommand
{

    private UIScriptableObject spawnUIEffects;
    private GameObject handSpace;
    private GameObject fieldSpace;
    private List<GameObject> drawnCards;


    public SpawnCommand(UIScriptableObject uIScriptableObject)
    {
        spawnUIEffects = uIScriptableObject;
    }

    public IEnumerator Execute(GameEventsArgs g)
    {
        HelperFunctions.Print("Spawn Command Execurte function");
        Show((UiEvents)g);
        yield return TweenWaitTime();
        
    }

    private void Show(UiEvents ue)
    {
        if(ue.EventOwner.Type != PlayerType.Human)
        {
            handSpace = GameObject.FindGameObjectWithTag("enemyHand");
            fieldSpace = GameObject.FindGameObjectWithTag("enemyField");
            drawnCards = UIManager.OtherHandCards;
        }
        else
        {
            Debug.LogWarning("This Player type should be change MainHuman");
            handSpace = GameObject.FindGameObjectWithTag("playerHand");
            fieldSpace = GameObject.FindGameObjectWithTag("playerField");
            drawnCards = UIManager.HandCards;
        }

        HelperFunctions.Print(ue.EventOriginCard.Name);
        GameObject c = ue.EventOriginCard.GameObj;
        if(drawnCards.Remove(c))
        {
            c.transform.DOMove(fieldSpace.transform.position, spawnUIEffects.MoveSpeed);
            c.transform.SetParent(fieldSpace.transform);
        }
        else
        {
            HelperFunctions.Error("Card GO not removed you should find our why");
        }
        
    }

    private IEnumerator TweenWaitTime()
    {
        yield return new WaitForSeconds(spawnUIEffects.MoveSpeed);
    }
}
