using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.FSM
{
    /// <summary>
    /// Represents the game state when it's Player 0's turn.
    /// </summary>
    public class Big2GMStateP0Turn : BaseState<GMState>
    {
        private Big2GMStateMachine GMSM;

        /// <summary>
        /// Initializes a new instance of the <see cref="Big2GMStateP0Turn"/> class.
        /// </summary>
        /// <param name="key">The state key.</param>
        /// <param name="stateMachine">The game state machine.</param>
        public Big2GMStateP0Turn(GMState key, Big2GMStateMachine stateMachine) : base(key)
        {
            GMSM = stateMachine;
        }

        /// <summary>
        /// Executes when entering this game state.
        /// </summary>
        public override void EnterState()
        {
            Debug.Log("GM in P0 Turn State");
            GMSM.OrderPlayerToPlay(0);
        }

        /// <summary>
        /// Executes when exiting this game state.
        /// </summary>
        public override void ExitState()
        {
        }

        /// <summary>
        /// Gets the active game state.
        /// </summary>
        /// <returns>The active game state.</returns>
        public override GMState GetActiveState()
        {
            return GMState.P0Turn;
        }

        /// <summary>
        /// Updates the game state.
        /// </summary>
        public override void UpdateState()
        {
        }
    }
}

