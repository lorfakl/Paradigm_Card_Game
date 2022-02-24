using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewMultiCardTransfer", menuName = "CardTransfer/MultiCardTransfer")]
public class MultiCardTransferSO : ScriptableObject
{
    [System.NonSerialized]
    public UnityEvent<List<CardSO>> dataTransferReadyEvent;

    private void OnEnable()
    {
        if (dataTransferReadyEvent == null)
        {
            dataTransferReadyEvent = new UnityEvent<List<CardSO>>();
        }
    }

    public void SendListDataToListener(List<CardSO> cardSOs)
    {
        dataTransferReadyEvent.Invoke(cardSOs);
    }
}
