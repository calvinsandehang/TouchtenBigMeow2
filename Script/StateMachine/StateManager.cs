using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.FSM
{
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

        private bool waitingForTransitionToEndOfFrame = false;

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            // Process current state update if not transitioning
            if (!IsTransitioningState && CurrentState != null)
            {
                CurrentState.UpdateState();
            }

            // Check at the end of the frame if we need to transition states
            if (NextState != null && !IsTransitioningState && !waitingForTransitionToEndOfFrame)
            {
                waitingForTransitionToEndOfFrame = true;
                StartCoroutine(DelayedStateTransition());
            }
        }

        private IEnumerator DelayedStateTransition()
        {
            // Wait until the end of the frame to process state transition
            // This ensures all Update() calls for this frame are done
            yield return new WaitForEndOfFrame();

            // Now perform the state transition
            if (NextState != null) // Double-checking in case something changed during the frame
            {
                TransitionToState(NextState.StateKey);
                NextState = null;
                waitingForTransitionToEndOfFrame = false;
            }
        }

        public void RequestTransitionToState(Estate stateKey)
        {
            if (IsTransitioningState)
            {
                Debug.LogWarning($"Transition to {stateKey} is requested while another transition is in progress.");
                return;
            }

            // Set the NextState without immediately transitioning
            NextState = States[stateKey];
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
    }
}

