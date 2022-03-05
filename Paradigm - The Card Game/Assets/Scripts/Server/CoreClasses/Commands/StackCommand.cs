using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackCommand : ICommand
{
    private UIScriptableObject stackUIEffects;
    private GameObject stackUI;

    public StackCommand(UIScriptableObject drawUIEffects, GameObject attackIcon)
    {
        this.stackUIEffects = drawUIEffects;
        stackUI = attackIcon;
    }

    public IEnumerator Execute(GameEventsArgs g)
    {
        GameObject icon = GameObject.Instantiate(stackUI, new Vector3(0,0,0), Quaternion.identity);
        icon.transform.localScale = new Vector3(0, 0, 0);
        icon.transform.DOScale(1, .2f);

        yield return new WaitForSeconds(stackUIEffects.MoveSpeed);
    }
}
