using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-9998)]
public abstract class StateManager<Estate> : MonoBehaviour where Estate : Enum
{
    protected Dictionary<Estate, BaseState<Estate>> States = new Dictionary<Estate, BaseState<Estate>>();
    protected BaseState<Estate> CurrentState;
    protected BaseState<Estate> NextState;
    protected bool IsTransitioningState = false;

    private void Awake()
    {
        StateInitialization();
        ParameterInitialization();
    }

    private void Update()
    {
        if (NextState == null && CurrentState != null)
        {
            CurrentState.UpdateState();
        }

        if (NextState == null) 
        {
            return;
        }
        Estate currentStateKey = CurrentState.GetActiveState();

        if (!IsTransitioningState && currentStateKey.Equals(NextState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!IsTransitioningState)
        {
            TransitionToState(NextState.StateKey);
        }
    }

    public void TransitionToState(Estate stateKey) 
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    protected virtual void ParameterInitialization()
    {

    }

    protected virtual void StateInitialization()
    {

    }
}
