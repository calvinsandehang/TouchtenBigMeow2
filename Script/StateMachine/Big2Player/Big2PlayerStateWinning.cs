using UnityEngine;

namespace Big2Meow.FSM
{
    /// <summary>
    /// Represents the state of a Big2 player when they are winning the game.
    /// </summary>
    public class Big2PlayerStateWinning : BaseState<PlayerState>
    {
        private Big2PlayerStateMachine PSM;

        /// <summary>
        /// Initializes a new instance of the Big2PlayerStateWinning class with the specified state key and state machine.
        /// </summary>
        /// <param name="key">The key that represents the state.</param>
        /// <param name="stateMachine">The state machine managing the player's states.</param>
        public Big2PlayerStateWinning(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
        {
            PSM = stateMachine;
        }

        /// <summary>
        /// Called when entering the winning state.
        /// </summary>
        public override void EnterState()
        {
            int playerID = PSM.PlayerHand.PlayerID;
            Debug.Log("Player " + playerID + " is in Winning state");

            // Broadcast the event to indicate that the player is winning
            PSM.BroadcastPlayerIsWinning();
        }

        /// <summary>
        /// Called when exiting the winning state.
        /// </summary>
        public override void ExitState()
        {
            // No specific actions needed when exiting the winning state
        }

        /// <summary>
        /// Gets the currently active state.
        /// </summary>
        /// <returns>The currently active state key (Winning).</returns>
        public override PlayerState GetActiveState()
        {
            return PlayerState.Winning;
        }

        /// <summary>
        /// Called to update the winning state logic (not used in this state).
        /// </summary>
        public override void UpdateState()
        {
            // No specific logic needed for the winning state
        }
    }
}


