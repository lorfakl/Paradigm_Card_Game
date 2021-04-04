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
    private System.Collections.ObjectModel.ObservableCollection<GameObject> drawnCards;
    private System.Collections.ObjectModel.ObservableCollection<GameObject> spawnCards;


    public SpawnCommand(UIScriptableObject uIScriptableObject)
    {
        spawnUIEffects = uIScriptableObject;
    }

    public IEnumerator Execute(GameEventsArgs g)
    {
        //HelperFunctions.Print("Spawn Command Execurte function");
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
            spawnCards = UIManager.OtherFieldCards;
        }
        else
        {
            Debug.LogWarning("This Player type should be change MainHuman");
            handSpace = GameObject.FindGameObjectWithTag("playerHand");
            fieldSpace = GameObject.FindGameObjectWithTag("playerField");
            drawnCards = UIManager.HandCards;
            spawnCards = UIManager.FieldCards;
        }

        HelperFunctions.Print(ue.EventOriginCard.Name);
        GameObject c = ue.EventOriginCard.GameObj;
        if(drawnCards.Remove(c))
        {
            c.transform.SetParent(fieldSpace.transform);
            Vector3 offsetFromOrigin = fieldSpace.transform.position;
            offsetFromOrigin.x -= UIManager.CardWidth / 2;
            c.transform.DOMove(offsetFromOrigin, spawnUIEffects.MoveSpeed);
            spawnCards.Add(c);
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
