using Big2Meow.AI;
using Big2Meow.Gameplay;
using Big2Meow.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

namespace Big2Meow.FSM
{
    /// <summary>
    /// Manages the state and behavior of a Big2 player.
    /// </summary>
    public enum PlayerState
    {
        Pregame,
        Playing,
        Waiting,
        Losing,
        Winning,
        Postgame,
    }

    /// <summary>
    /// Manages the state and behavior of a Big2 player.
    /// </summary>
    public class Big2PlayerStateMachine : StateManager<PlayerState>, ISubscriber
    {
        public Big2PlayerHand PlayerHand { get; private set; }
        private Big2CardSubmissionCheck cardSubmissionCheck;
        private Big2PlayerSkipTurnHandler skipTurnHandler;
        public Big2SimpleAI Big2AI { get; private set; }


        #region Events
        public event Action OnPlayerIsPlaying;
        public event Action OnPlayerIsLosing;
        public event Action OnPlayerIsWaiting;
        public event Action OnPlayerIsWinning;
        #endregion

        #region Monobehaviour

        private void Awake()
        {
            StateInitialization();
            ParameterInitialization();
            SubscribeEvent();
        }

        private void Start()
        {
            CurrentState = States[PlayerState.Pregame];
            CurrentState.EnterState();
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }

        #endregion

        #region State Transition Methods

        public void MakePlayerWait()
        {
            RequestTransitionToState(PlayerState.Waiting);
        }

        public void MakePlayerWait(Big2PlayerHand player)
        {
            if (PlayerHand == player)
                RequestTransitionToState(PlayerState.Waiting);
        }

        public void MakePlayerPlay()
        {
            RequestTransitionToState(PlayerState.Playing);
            // Handle player's turn logic here
        }

        public void MakePlayerLose()
        {
            RequestTransitionToState(PlayerState.Losing);
            // Handle player's losing logic here
        }

        public void MakePlayerWin()
        {
            RequestTransitionToState(PlayerState.Winning);
            // Handle player's winning logic here
        }

        public void MakePlayerInPostGame()
        {
            RequestTransitionToState(PlayerState.Postgame);
            // Handle player's postgame logic here
        }

        #endregion


        #region Event Broadcasting Methods

        public void BroadcastPlayerIsPlaying()
        {
            OnPlayerIsPlaying?.Invoke();
        }

        public void BroadcastPlayerIsWaiting()
        {
            OnPlayerIsWaiting?.Invoke();
        }

        public void BroadcastPlayerIsWinning()
        {
            OnPlayerIsWinning?.Invoke();
        }

        public void BroadcastPlayerIsLosing()
        {
            OnPlayerIsLosing?.Invoke();
        }

        #endregion

        #region Subscribe Event

        /// <summary>
        /// Subscribe to relevant events.
        /// </summary>
        public void SubscribeEvent()
        {
            cardSubmissionCheck.OnPlayerFinishTurnLocal += MakePlayerWait;
            skipTurnHandler.OnPlayerSkipTurnLocal += MakePlayerWait;
            Big2GlobalEvent.SubscribeAISkipTurnGlobal(MakePlayerWait);
            Big2GlobalEvent.SubscribeAIFinishTurnGlobal(MakePlayerWait);
            Big2GlobalEvent.SubscribeHavingQuadrupleTwo(MakePlayerWait);
        }

        /// <summary>
        /// Unsubscribe from relevant events.
        /// </summary>
        public void UnsubscribeEvent()
        {
            cardSubmissionCheck.OnPlayerFinishTurnLocal -= MakePlayerWait;
            skipTurnHandler.OnPlayerSkipTurnLocal -= MakePlayerWait;
            Big2GlobalEvent.UnsubscribeAISkipTurnGlobal(MakePlayerWait);
            Big2GlobalEvent.UnsubscribeAIFinishTurnGlobal(MakePlayerWait);
            Big2GlobalEvent.UnsubscribeHavingQuadrupleTwo(MakePlayerWait);
        }

        #endregion

        #region Helper Methods
        public bool CheckCurrentState(PlayerState state)
        {
            // Check if the provided state is the current state
            return CurrentState == States[state];
        }


        public PlayerType GetPlayerType()
        {
            return PlayerHand.PlayerType;
        }

        private void ParameterInitialization()
        {
            PlayerHand = GetComponent<Big2PlayerHand>();
            cardSubmissionCheck = GetComponent<Big2CardSubmissionCheck>();
            Big2AI = GetComponent<Big2SimpleAI>();
            skipTurnHandler = GetComponent<Big2PlayerSkipTurnHandler>();
        }

        private void StateInitialization()
        {
            // Initialize player states
            States[PlayerState.Pregame] = new Big2PlayerStatePreGame(PlayerState.Pregame, this);
            States[PlayerState.Playing] = new Big2PlayerStatePlaying(PlayerState.Playing, this);
            States[PlayerState.Waiting] = new Big2PlayerStateWaiting(PlayerState.Waiting, this);
            States[PlayerState.Postgame] = new Big2PlayerStatePostGame(PlayerState.Postgame, this);
            States[PlayerState.Winning] = new Big2PlayerStateWinning(PlayerState.Winning, this);
            States[PlayerState.Losing] = new Big2PlayerStateLosing(PlayerState.Losing, this);
        }

        #endregion
    }
}


