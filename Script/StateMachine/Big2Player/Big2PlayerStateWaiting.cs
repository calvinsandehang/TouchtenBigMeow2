using UnityEngine;

namespace Big2Meow.FSM
{
    /// <summary>
    /// Represents the state of a Big2 player when they are waiting for their turn.
    /// </summary>
    public class Big2PlayerStateWaiting : BaseState<PlayerState>
    {
        private Big2PlayerStateMachine playerStateMachine;

        /// <summary>
        /// Initializes a new instance of the Big2PlayerStateWaiting class with the specified state key and state machine.
        /// </summary>
        /// <param name="key">The key that represents the state.</param>
        /// <param name="stateMachine">The state machine managing the player's states.</param>
        public Big2PlayerStateWaiting(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
        {
            playerStateMachine = stateMachine;
        }

        /// <summary>
        /// Called when entering the waiting state.
        /// </summary>
        public override void EnterState()
        {
            int playerID = playerStateMachine.PlayerHand.PlayerID;
            Debug.Log("Player " + playerID + " is in Waiting state");

            // Broadcast the event to indicate that the player is waiting
            playerStateMachine.BroadcastPlayerIsWaiting();
        }

        /// <summary>
        /// Called when exiting the waiting state.
        /// </summary>
        public override void ExitState()
        {
            // No specific actions needed when exiting the waiting state
        }

        /// <summary>
        /// Gets the currently active state.
        /// </summary>
        /// <returns>The currently active state key (Waiting).</returns>
        public override PlayerState GetActiveState()
        {
            return PlayerState.Waiting;
        }

        /// <summary>
        /// Called to update the waiting state logic (not used in this state).
        /// </summary>
        public override void UpdateState()
        {
            // No specific logic needed for the waiting state
        }
    }
}


