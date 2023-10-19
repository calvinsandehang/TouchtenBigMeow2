using System;
using UnityEngine;

public abstract class BaseState<EState> where EState : Enum
{
    public BaseState(EState key, StateManager<EState> stateManager) 
    {
        StateKey = key;
        StateManager = stateManager;    
    }

    public EState StateKey { get; private set; }
    public StateManager<EState> StateManager { get; private set; }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract EState GetNextState();
}
