using System.Collections;
using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TurnPhaseCommand : ICommand
{
    private UIScriptableObject turnphaseNotification;
    private GameObject turnphaseNotificationObject;
    private GameObject onScreenTurnphase;
    private GameObject turnTextInstance = null;
    private bool isInstantiated;
    private Vector3 startingPosition;

    public TurnPhaseCommand(UIScriptableObject uIScriptableObject, GameObject go, GameObject onScreen)
    {
        turnphaseNotification = uIScriptableObject;
        turnphaseNotificationObject = go;
        onScreenTurnphase = onScreen;
        isInstantiated = false;
        startingPosition = new Vector3(-90, 2, -20);
    }

    public IEnumerator Execute(GameEventsArgs g)
    {
        
        if(turnphaseNotification == null || turnphaseNotificationObject == null)
        {
            throw new Exception("Something is null here");
        }

        
        if (!isInstantiated)
        {
            turnTextInstance = UnityEngine.Object.Instantiate(turnphaseNotificationObject, GameObject.FindGameObjectWithTag("Respawn").transform.position, Quaternion.identity);
            isInstantiated = true;
        }
        onScreenTurnphase.GetComponent<TextMeshPro>().text = GameMaster.CurrentTurnPlayer.Type.ToString() + " " + GameMaster.CurrentTurnPhase.ToString();
        turnTextInstance.GetComponent<TextMeshPro>().text = GameMaster.CurrentTurnPlayer.Type.ToString() + " " + GameMaster.CurrentTurnPhase.ToString();
        turnTextInstance.transform.DOMove(new Vector3(0, 2, -20), .1f);
        
        
        yield return new WaitForSeconds(1.5f);
        turnTextInstance.transform.DOMoveX(90, .1f);
        yield return new WaitForSeconds(1);
        turnTextInstance.transform.position = startingPosition;

    }
}
