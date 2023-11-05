using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.FSM 
{
    /// <summary>
    /// Represents the state where the game manager asks players for input during post-game.
    /// </summary>
    public class Big2GMStateAskPlayer : BaseState<GMState>
    {
        private Big2GMStateMachine GMSM;

        /// <summary>
        /// Initializes a new instance of the <see cref="Big2GMStateAskPlayer"/> class.
        /// </summary>
        /// <param name="key">The state key.</param>
        /// <param name="stateMachine">The game manager state machine.</param>
        public Big2GMStateAskPlayer(GMState key, Big2GMStateMachine stateMachine) : base(key)
        {
            GMSM = stateMachine;
        }

        /// <summary>
        /// Called when entering the AskPlayer state.
        /// </summary>
        public override void EnterState()
        {
            Debug.Log("GameManager is in the AskPlayer State");
            Big2GlobalEvent.BroadcastAskPlayerInPostGame();

            GMSM.OrderPlayerToWaitInPostGame();
        }

        /// <summary>
        /// Called when exiting the AskPlayer state.
        /// </summary>
        public override void ExitState()
        {
            // No specific exit actions for this state.
        }

        /// <summary>
        /// Gets the active state, which is GMState.AskPlayer.
        /// </summary>
        /// <returns>The active state.</returns>
        public override GMState GetActiveState()
        {
            return GMState.AskPlayer;
        }

        /// <summary>
        /// Called to update the AskPlayer state.
        /// </summary>
        public override void UpdateState()
        {
            // No specific update actions for this state.
        }
    }
}

