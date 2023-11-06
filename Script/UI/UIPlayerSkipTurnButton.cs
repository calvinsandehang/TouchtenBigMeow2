using Big2Meow.DeckNCard;
using Big2Meow.FSM;
using Big2Meow.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

namespace Big2Meow.UI
{
    /// <summary>
    /// Manages the skip turn button for a player in the UI.
    /// </summary>
    [DefaultExecutionOrder(2)]
    public class UIPlayerSkipTurnButton : MonoBehaviour, ISubscriber
    {
        private Button skipTurnButton;
        private Image buttonImage;

        [SerializeField] private Color _allowedColor = Color.white;
        [SerializeField] private Color _notAllowedColor = Color.gray;

        private Big2PlayerStateMachine playerStateMachine;
        private PlayerType playerType;

        private Big2TableManager tableManager;
        private HandType currentTableType;

        #region Monobehaviour
        private void Awake()
        {
            buttonImage = GetComponent<Image>();
            skipTurnButton = GetComponent<Button>();
        }

        private void Start()
        {
            ParameterInitialization();
        }

        private void OnDisable()
        {
            if (playerStateMachine == null)
                return;
            UnsubscribeEvent();
        }
        #endregion

        #region Initialization
        private void ParameterInitialization()
        {
            tableManager = Big2TableManager.Instance;
        }

        /// <summary>
        /// Initializes the skip turn button for a player.
        /// </summary>
        /// <param name="playerStateMachine">The player's state machine.</param>
        public void InitializeButton(Big2PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            playerType = playerStateMachine.GetPlayerType();

            if (playerStateMachine == null || playerType != PlayerType.Human)
                return;

            SubscribeEvent();
            OnNotAllowedToSkipTurn();
        }
        #endregion

        #region Skip turn logic
        private void OnAllowedToSkipTurn()
        {
            currentTableType = tableManager.TableHandType;

            if (currentTableType == HandType.None)
                return;

            SetButtonInteractable();
        }

        private void OnNotAllowedToSkipTurn()
        {
            SetButtonNonInteractable();
        }

        private void OnNotAllowedToSkipTurn(CardInfo cardInfo)
        {
            SetButtonNonInteractable();
        }
        #endregion

        #region Handle Button
        /// <summary>
        // Set the button interactable and update the image color to allowedColor
        /// </summary>
        private void SetButtonInteractable()
        {
            skipTurnButton.interactable = true;
            buttonImage.color = _allowedColor;
        }

        /// <summary>
        // Set the button not interactable and update the image color to notAllowedColor
        /// </summary>
        private void SetButtonNonInteractable()
        {
            skipTurnButton.interactable = false;
            buttonImage.color = _notAllowedColor;
        }
        #endregion

        #region Subscribe Event
        public void SubscribeEvent()
        {
            playerStateMachine.OnPlayerIsPlaying += OnAllowedToSkipTurn;
            playerStateMachine.OnPlayerIsLosing += OnNotAllowedToSkipTurn;
            playerStateMachine.OnPlayerIsWaiting += OnNotAllowedToSkipTurn;
            Big2GlobalEvent.SubscribeSubmitCard(OnNotAllowedToSkipTurn);
        }

        public void UnsubscribeEvent()
        {
            playerStateMachine.OnPlayerIsPlaying -= OnAllowedToSkipTurn;
            playerStateMachine.OnPlayerIsLosing -= OnNotAllowedToSkipTurn;
            playerStateMachine.OnPlayerIsWaiting -= OnNotAllowedToSkipTurn;
            Big2GlobalEvent.UnsubscribeSubmitCard(OnNotAllowedToSkipTurn);
        }
        #endregion
    }
}
   
