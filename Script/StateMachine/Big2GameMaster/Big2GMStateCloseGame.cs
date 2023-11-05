using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.FSM 
{
    /// <summary>
    /// Represents the game state when the game is closing.
    /// </summary>
    public class Big2GMStateCloseGame : BaseState<GMState>
    {
        private Big2GMStateMachine GMSM;

        /// <summary>
        /// Initializes a new instance of the <see cref="Big2GMStateCloseGame"/> class.
        /// </summary>
        /// <param name="key">The state key.</param>
        /// <param name="stateMachine">The game state machine.</param>
        public Big2GMStateCloseGame(GMState key, Big2GMStateMachine stateMachine) : base(key)
        {
            GMSM = stateMachine;
        }

        /// <summary>
        /// Executes when entering this game state.
        /// </summary>
        public override void EnterState()
        {
            Debug.Log("GM in Close Game State");

            Big2GlobalEvent.BroadcastGameHasEnded();
            SetGameNotInFirstGame();

            GMSM.StartCoroutine(GMSM.DelayedAction(GMSM.AskPlayerInPostGame, GMSM.PostGameDelay));
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
            return GMState.CloseGame;
        }

        /// <summary>
        /// Updates the game state.
        /// </summary>
        public override void UpdateState()
        {
        }

        /// <summary>
        /// Sets the game as not in the first game.
        /// </summary>
        private void SetGameNotInFirstGame()
        {
            if (GMSM.WinnerPlayer != null)
                GMSM.FirstGame = false;
        }
    }
}

