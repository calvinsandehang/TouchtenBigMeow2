using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

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
    public PlayerState CurrentPlayerState { get; private set; }
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

    #region Unity Callback

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
        NextState = States[PlayerState.Waiting];
    }

    public void MakePlayerWait(Big2PlayerHand player)
    {
        if (PlayerHand == player)
            NextState = States[PlayerState.Waiting];
    }

    public void MakePlayerPlay()
    {
        NextState = States[PlayerState.Playing];
        // Handle player's turn logic here
    }

    public void MakePlayerLose()
    {
        NextState = States[PlayerState.Losing];
        // Handle player's losing logic here
    }

    public void MakePlayerWin()
    {
        NextState = States[PlayerState.Winning];
        // Handle player's winning logic here
    }

    public void MakePlayerInPostGame()
    {
        NextState = States[PlayerState.Postgame];
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

    }

    #endregion

    #region Helper Methods

    public PlayerType GetPlayerType()
    {
        return PlayerHand.PlayerType;
    }

    protected override void ParameterInitialization()
    {
        PlayerHand = GetComponent<Big2PlayerHand>();
        cardSubmissionCheck = GetComponent<Big2CardSubmissionCheck>();
        Big2AI = GetComponent<Big2SimpleAI>();
        skipTurnHandler = GetComponent<Big2PlayerSkipTurnHandler>();
    }

    protected override void StateInitialization()
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
