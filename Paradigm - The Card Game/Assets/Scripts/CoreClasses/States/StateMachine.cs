using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

public enum Results
{
    Success,
    Failure
}
public class StateMachine
{
    public string Name { get; private set; }
    public IState EntryState { get; private set; }
    public IState CurrentState { get; private set; }
    public IState EndState { get; private set; }
    public List<IState> States { get; private set; }

    private StateProcessingComplete ProcessingCompleteCallback { get; set; }

    private int CurrentIndex { get; set; }

    public StateMachine(string name, List<IState> states)
    {
        Name = name;
        CurrentState = EntryState = states[0];
        EndState = states[states.Count - 1];
        CurrentIndex = 0;
        States = states;
        HelperFunctions.Print("Total States in machine" + states.Count);

    }

    public StateMachine(string name, List<IState> states, StateProcessingComplete turnCompleteCallback)
    {
        Name = name;
        CurrentState = EntryState = states[0];
        EndState = states[states.Count - 1];
        CurrentIndex = 0;
        States = states;
        HelperFunctions.Print("Total States in machine" + states.Count);
        ProcessingCompleteCallback = turnCompleteCallback;

    }

    public IEnumerator ProcessStates()
    {
        GameMaster.IsTurnActive = true;
        do
        {
            //Debug.Log("There's an error some where");
            CurrentState.OnEntry();
            Debug.Log("State Entry no error");
            Task operation = CurrentState.Operation();
            Debug.Log("Operation started");
            //await operation;

            yield return new WaitUntil(() => operation.IsCompleted);
            if(operation.IsCompleted)
            {
                Debug.Log("Operation ENDED OnExit start");
                CurrentState.OnExit();
                
                CurrentIndex++;
                Debug.Log("Moving to next state after On Exit");
                CurrentState = States[CurrentIndex];
                if(States == null)
                {
                    HelperFunctions.Error("Somehow this is null");
                }
            }
        }
        while (CurrentState != EndState);
        Debug.Log("I dont think we ever get to the end");
        ProcessingCompleteCallback();
    }
    

    public void UpdateCurrentState(IState nextState)
    {
        CurrentState.OnExit();
        CurrentState = nextState;
        CurrentState.OnEntry();
    }
}
