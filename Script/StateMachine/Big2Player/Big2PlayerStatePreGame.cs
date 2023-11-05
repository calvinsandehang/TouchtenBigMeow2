using UnityEngine;

namespace Big2Meow.FSM
{
    /// <summary>
    /// Represents the state of a Big2 player during the pregame phase.
    /// </summary>
    public class Big2PlayerStatePreGame : BaseState<PlayerState>
    {
        private Big2PlayerStateMachine playerStateMachine;

        /// <summary>
        /// Initializes a new instance of the Big2PlayerStatePreGame class with the specified state key and state machine.
        /// </summary>
        /// <param name="key">The key that represents the state.</param>
        /// <param name="stateMachine">The state machine managing the player's states.</param>
        public Big2PlayerStatePreGame(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
        {
            playerStateMachine = stateMachine;
        }

        /// <summary>
        /// Called when entering the pregame state.
        /// </summary>
        public override void EnterState()
        {
            int playerID = playerStateMachine.PlayerHand.PlayerID;
            // Uncomment the line below if you want to log a message when entering this state
            // Debug.Log("Player " + playerID + " is in PreGame state");
        }

        /// <summary>
        /// Called when exiting the pregame state.
        /// </summary>
        public override void ExitState()
        {
            // No specific actions needed when exiting the pregame state
        }

        /// <summary>
        /// Gets the currently active state.
        /// </summary>
        /// <returns>The currently active state key (Pregame).</returns>
        public override PlayerState GetActiveState()
        {
            return PlayerState.Pregame;
        }

        /// <summary>
        /// Called to update the pregame state logic (not used in this state).
        /// </summary>
        public override void UpdateState()
        {
            // No specific logic needed for the pregame state
        }
    }
}

