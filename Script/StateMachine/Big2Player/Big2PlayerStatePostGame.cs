using UnityEngine;

namespace Big2Meow.FSM
{
    /// <summary>
    /// Represents the state of a Big2 player during the postgame phase.
    /// </summary>
    public class Big2PlayerStatePostGame : BaseState<PlayerState>
    {
        private Big2PlayerStateMachine playerStateMachine;

        /// <summary>
        /// Initializes a new instance of the Big2PlayerStatePostGame class with the specified state key and state machine.
        /// </summary>
        /// <param name="key">The key that represents the state.</param>
        /// <param name="stateMachine">The state machine managing the player's states.</param>
        public Big2PlayerStatePostGame(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
        {
            playerStateMachine = stateMachine;
        }

        /// <summary>
        /// Called when entering the postgame state.
        /// </summary>
        public override void EnterState()
        {
            // No specific actions needed when entering the postgame state
        }

        /// <summary>
        /// Called when exiting the postgame state.
        /// </summary>
        public override void ExitState()
        {
            // No specific actions needed when exiting the postgame state
        }

        /// <summary>
        /// Gets the currently active state.
        /// </summary>
        /// <returns>The currently active state key (Postgame).</returns>
        public override PlayerState GetActiveState()
        {
            return PlayerState.Postgame;
        }

        /// <summary>
        /// Called to update the postgame state logic (not used in this state).
        /// </summary>
        public override void UpdateState()
        {
            // No specific logic needed for the postgame state
        }
    }
}


