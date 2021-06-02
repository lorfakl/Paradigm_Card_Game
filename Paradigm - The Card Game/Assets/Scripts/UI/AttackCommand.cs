using UnityEngine;
using System;
using System.Collections;

public class AttackCommand : ICommand
{
    private UIScriptableObject attackUIEffects;
    private GameObject attacIcon;

    public AttackCommand(UIScriptableObject drawUIEffects, GameObject attackIcon)
    {
        this.attackUIEffects = drawUIEffects;
        attacIcon = attackIcon;
    }

    public IEnumerator Execute(GameEventsArgs g)
    {
        foreach(Card c in g.CardTargets)
        {
           GameObject icon = GameObject.Instantiate(attacIcon, c.GameObj.transform);
        }

        yield return null;
    }
}