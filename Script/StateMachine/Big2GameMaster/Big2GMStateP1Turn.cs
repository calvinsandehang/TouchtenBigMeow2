using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.FSM
{
    /// <summary>
    /// Represents the game state when it's Player 1's turn.
    /// </summary>
    public class Big2GMStateP1Turn : BaseState<GMState>
    {
        private Big2GMStateMachine gmStateMachine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Big2GMStateP1Turn"/> class.
        /// </summary>
        /// <param name="key">The state key.</param>
        /// <param name="stateMachine">The game state machine.</param>
        public Big2GMStateP1Turn(GMState key, Big2GMStateMachine stateMachine) : base(key)
        {
            gmStateMachine = stateMachine;
        }

        /// <summary>
        /// Executes when entering this game state.
        /// </summary>
        public override void EnterState()
        {
            Debug.Log("GM in P1 Turn State");
            gmStateMachine.OrderPlayerToPlay(1);
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
            return GMState.P1Turn;
        }

        /// <summary>
        /// Updates the game state.
        /// </summary>
        public override void UpdateState()
        {
        }
    }
}

