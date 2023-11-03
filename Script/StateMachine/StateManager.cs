using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract base class for managing states in a MonoBehaviour-based Unity component.
/// </summary>
/// <typeparam name="Estate">The enumeration type representing states.</typeparam>
[DefaultExecutionOrder(-9998)]
public abstract class StateManager<Estate> : MonoBehaviour where Estate : Enum
{
    /// <summary>
    /// Dictionary that stores available states and their associated state objects.
    /// </summary>
    protected Dictionary<Estate, BaseState<Estate>> States = new Dictionary<Estate, BaseState<Estate>>();

    /// <summary>
    /// The current state of the state manager.
    /// </summary>
    protected BaseState<Estate> CurrentState;

    /// <summary>
    /// The next state that the state manager will transition to.
    /// </summary>
    protected BaseState<Estate> NextState;

    /// <summary>
    /// Indicates whether the state manager is in the process of transitioning to a new state.
    /// </summary>
    protected bool IsTransitioningState = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        StateInitialization();
        ParameterInitialization();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
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

    /// <summary>
    /// Transitions the state manager to a new state.
    /// </summary>
    /// <param name="stateKey">The key of the state to transition to.</param>
    public void TransitionToState(Estate stateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    /// <summary>
    /// Initializes parameters for the state manager (override in derived classes if needed).
    /// </summary>
    protected virtual void ParameterInitialization()
    {
    }

    /// <summary>
    /// Initializes state-related settings and objects (override in derived classes if needed).
    /// </summary>
    protected virtual void StateInitialization()
    {
    }
}
