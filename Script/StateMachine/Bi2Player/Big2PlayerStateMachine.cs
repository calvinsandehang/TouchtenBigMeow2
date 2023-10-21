using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public enum PlayerState 
{
    Pregame,
    Playing,
    Waiting, 
    Losing,
    Winning,
    Postgame,
}

public class Big2PlayerStateMachine : StateManager<PlayerState>, ISubscriber
{
    public PlayerState PlayerState { get; private set; }
    private Big2PlayerHand playerHand;
    private Big2CardSubmissionCheck cardSubmissionCheck;

    public delegate void Big2PlayerStateMachineDelegate();
    public Big2PlayerStateMachineDelegate onPlayerIsPlaying;
    public Big2PlayerStateMachineDelegate onPlayerIsWinning;
    public Big2PlayerStateMachineDelegate onPlayerIsLosing;
    public Big2PlayerStateMachineDelegate onPlayerIsWaiting;

    public bool Test;    

    private void Awake()
    {       
        StateInitialization();
        ParameterInitialization();
        SubscribeEvent();

       
    }

    public void SubscribeEvent()
    {
        playerHand.OnHandLastCardIsDropped += MakePlayerWin;
    }

    public void UnsubscribeEvent()
    {
        playerHand.OnHandLastCardIsDropped -= MakePlayerWin;
    }

    protected override void ParameterInitialization()
    {
        playerHand = GetComponent<Big2PlayerHand>();
        cardSubmissionCheck = GetComponent<Big2CardSubmissionCheck>();
    }

    protected override void StateInitialization()
    {
        States[PlayerState.Pregame] = new Big2PlayerStatePreGame(PlayerState.Pregame, this);
        States[PlayerState.Playing] = new Big2PlayerStatePlaying(PlayerState.Playing, this);
        States[PlayerState.Waiting] = new Big2PlayerStatePlaying(PlayerState.Waiting, this);
        States[PlayerState.Postgame] = new Big2PlayerStatePostGame(PlayerState.Postgame, this);

        CurrentState = States[PlayerState.Pregame];
    }

    #region Order
    public void MakePlayerWait() 
    {
        TransitionToState(PlayerState.Waiting);
    }

    public void MakePlayerPlay() 
    {
        TransitionToState(PlayerState.Playing);
        
        // Card evaluator can be used
        // can submit
        // can pass
    }
    public void MakePlayerLose() 
    {
        TransitionToState(PlayerState.Losing);
        // do player lose stuff
    }

    public void MakePlayerWin() 
    {
        TransitionToState(PlayerState.Winning);
        // do player win stuff
    }

    

    public PlayerState GetPlayerState() 
    {
        return PlayerState;
    }

    public PlayerType GetPlayerType() 
    {
        Debug.Log("playerHand.PlayerType : " + playerHand.PlayerType);
        return playerHand.PlayerType;
    }


    #endregion

    

    #region Parameter

    #endregion




    #region Helper
    private void OnDisable()
    {
        UnsubscribeEvent();
    }
    #endregion
}
