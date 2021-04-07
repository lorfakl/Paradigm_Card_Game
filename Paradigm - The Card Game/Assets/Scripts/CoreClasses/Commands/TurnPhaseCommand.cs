using System.Collections;
using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TurnPhaseCommand : ICommand
{
    private UIScriptableObject turnphaseNotification;
    private GameObject turnphaseNotificationObject;

    public TurnPhaseCommand(UIScriptableObject uIScriptableObject, GameObject go)
    {
        turnphaseNotification = uIScriptableObject;
        turnphaseNotificationObject = go;
    }

    public IEnumerator Execute(GameEventsArgs g)
    {
        
        if(turnphaseNotification == null || turnphaseNotificationObject == null)
        {
            throw new Exception("Something is null here");
        }

        GameObject turnTextInstance = UnityEngine.Object.Instantiate(turnphaseNotificationObject, GameObject.FindGameObjectWithTag("Respawn").transform.position, Quaternion.identity);
        turnTextInstance.GetComponent<TextMeshPro>().text = GameMaster.CurrentTurnPlayer.Type.ToString() + " " + GameMaster.CurrentTurnPhase.ToString();
        turnTextInstance.transform.DOMove(new Vector3(0, 2, -20), .1f);
        
        
        yield return new WaitForSeconds(1.5f);
        turnTextInstance.transform.DOMoveX(90, .1f);
        yield return new WaitForSeconds(1);
        GameObject.Destroy(turnTextInstance);

    }
}
