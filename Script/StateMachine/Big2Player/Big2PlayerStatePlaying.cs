using UnityEngine;

namespace Big2Meow.FSM
{
    /// <summary>
    /// Represents the state of a Big2 player during the playing phase.
    /// </summary>
    public class Big2PlayerStatePlaying : BaseState<PlayerState>
    {
        private Big2PlayerStateMachine PSM;

        /// <summary>
        /// Initializes a new instance of the Big2PlayerStatePlaying class with the specified state key and state machine.
        /// </summary>
        /// <param name="key">The key that represents the state.</param>
        /// <param name="stateMachine">The state machine managing the player's states.</param>
        public Big2PlayerStatePlaying(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
        {
            PSM = stateMachine;
        }

        /// <summary>
        /// Called when entering the playing state.
        /// </summary>
        public override void EnterState()
        {
            int playerID = PSM.PlayerHand.PlayerID;
            Debug.Log("Player " + (playerID) + " is in Playing state");

            PSM.BroadcastPlayerIsPlaying();

            if (PSM.PlayerHand.PlayerType != GlobalDefine.PlayerType.Human)
                PSM.Big2AI.InitiateAiDecisionMaking();
        }

        /// <summary>
        /// Called when exiting the playing state.
        /// </summary>
        public override void ExitState()
        {
            // No specific actions needed when exiting the playing state
        }

        /// <summary>
        /// Gets the currently active state.
        /// </summary>
        /// <returns>The currently active state key (Playing).</returns>
        public override PlayerState GetActiveState()
        {
            return PlayerState.Playing;
        }

        /// <summary>
        /// Called to update the playing state logic (not used in this state).
        /// </summary>
        public override void UpdateState()
        {
            // No specific logic needed for the playing state
        }
    }

}

