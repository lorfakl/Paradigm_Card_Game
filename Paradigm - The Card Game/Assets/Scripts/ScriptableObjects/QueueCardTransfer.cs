using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewQueueCardTransfer", menuName = "CardTransfer/QueueCardTransfer")]
public class QueueCardTransferSO : ScriptableObject
{
    [System.NonSerialized]
    public UnityEvent<CardSO> dataTransferReadyEvent;

    private Queue<CardSO> cardQueue = new Queue<CardSO>();

    private void OnEnable()
    {
        if (dataTransferReadyEvent == null)
        {
            dataTransferReadyEvent = new UnityEvent<CardSO>();
        }
    }

    public void SetQueueDataToListener(List<CardSO> cardSOs)
    {
        foreach(CardSO c in cardSOs)
        {
            cardQueue.Enqueue(c);
        }
    }

    public CardSO GetQueueCard()
    {
        return cardQueue.Dequeue();
    }


}
