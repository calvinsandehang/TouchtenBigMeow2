using System;
using UnityEngine;

namespace Big2Meow.FSM
{
    /// <summary>
    /// Base class for defining states in a state machine.
    /// </summary>
    /// <typeparam name="EState">The enumeration type representing states.</typeparam>
    public abstract class BaseState<EState> where EState : Enum
    {
        /// <summary>
        /// Initializes a new instance of the BaseState class with the specified state key.
        /// </summary>
        /// <param name="key">The key that represents the state.</param>
        public BaseState(EState key)
        {
            StateKey = key;
        }

        /// <summary>
        /// Gets the key that represents the state.
        /// </summary>
        public EState StateKey { get; private set; }

        /// <summary>
        /// Called when entering the state.
        /// </summary>
        public abstract void EnterState();

        /// <summary>
        /// Called when exiting the state.
        /// </summary>
        public abstract void ExitState();

        /// <summary>
        /// Called to update the state logic.
        /// </summary>
        public abstract void UpdateState();

        /// <summary>
        /// Gets the currently active state.
        /// </summary>
        /// <returns>The currently active state key.</returns>
        public abstract EState GetActiveState();
    }
}

