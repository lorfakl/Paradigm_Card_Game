using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PerformOperationEvent : ScriptableObject
{
    [SerializeField]
    public int numberOfOperations = 0;

    [System.NonSerialized]
    public UnityEvent<int> operationToPerformEvent;

    private void OnEnable()
    {
        if (operationToPerformEvent == null)
        {
            operationToPerformEvent = new UnityEvent<int>();
        }
    }

    public void GetOperationCount()
    {
        operationToPerformEvent.Invoke(numberOfOperations);
    }
}
