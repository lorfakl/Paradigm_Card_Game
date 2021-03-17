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

    //CREATE AN EVENT THAT INFORMS THE GAMEMASTER ALL STATES HAVE BEEN PROCESSED
    public delegate void NotifyDoneProcessingStates();
    public static event NotifyDoneProcessingStates IsDoneProcessing; //for multiplayer this cant be static

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

    public async void ProcessStates()
    {
        do
        {
            CurrentState.OnEntry();
            Task operation = CurrentState.Operation();
            await operation;
            if(operation.IsCompleted)
            {
                CurrentState.OnExit();
                HelperFunctions.Print("Current State Index" + CurrentIndex);
                CurrentIndex++;
                HelperFunctions.Print("Current State Index" + CurrentIndex);
                CurrentState = States[CurrentIndex];
                if(States == null)
                {
                    HelperFunctions.Print("Somehow this is null");
                }
            }
        }
        while (CurrentState != EndState);

        
    }
    
    

    public void UpdateCurrentState(IState nextState)
    {
        CurrentState.OnExit();
        CurrentState = nextState;
        CurrentState.OnEntry();
    }
}
