using Big2Meow.FSM;
using Big2Meow.Injection;
using Big2Meow.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

namespace Big2Meow.Player 
{
    /// <summary>
    /// Handles the skip turn functionality for a Big2 player.
    /// </summary>
    [DefaultExecutionOrder(1)]
    public class Big2PlayerSkipTurnHandler : MonoBehaviour
    {
        private Big2PlayerStateMachine playerStateMachine;
        private Big2PlayerHand playerHand;
        private Button skipTurnButton;

        /// <summary>
        /// Event that is invoked when the local player decides to skip their turn.
        /// </summary>
        public event Action OnPlayerSkipTurnLocal;

        private void Awake()
        {
            playerStateMachine = GetComponent<Big2PlayerStateMachine>();
            playerHand = GetComponent<Big2PlayerHand>();
        }

        private void Start()
        {
            if (playerHand.PlayerType == PlayerType.Human)
            {
                SetupSkipTurnButton();
            }
        }

        private void SetupSkipTurnButton()
        {
            skipTurnButton = UIButtonInjector.Instance.GetButton(ButtonType.SkipTurn);
            skipTurnButton.onClick.AddListener(TellGMToGoNextTurn);

            UIPlayerSkipTurnButton skipTurnButtonBehaviour = skipTurnButton.GetComponent<UIPlayerSkipTurnButton>();
            skipTurnButtonBehaviour.InitializeButton(playerStateMachine);
        }

        private void TellGMToGoNextTurn()
        {
            StartCoroutine(DelayedAction());
        }

        private IEnumerator DelayedAction()
        {
            yield return new WaitForSeconds(0.1f);
            Big2GlobalEvent.BroadcastPlayerSkipTurnGlobal(playerHand);
            OnPlayerSkipTurnLocal?.Invoke();
        }
    }
}

