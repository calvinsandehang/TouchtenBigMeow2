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
    public Big2PlayerHand PlayerHand { get; private set; }
    private Big2CardSubmissionCheck cardSubmissionCheck;
    public Big2SimpleAI big2AI { get; private set; }

    public delegate void Big2PlayerStateMachineDelegate();
    public Big2PlayerStateMachineDelegate onPlayerIsPlaying { get; set; }
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

    private void Start()
    {
        CurrentState = States[PlayerState.Pregame];
        CurrentState.EnterState();
    }

    public void SubscribeEvent()
    {
        PlayerHand.OnHandLastCardIsDropped += MakePlayerWin;
    }

    public void UnsubscribeEvent()
    {
        PlayerHand.OnHandLastCardIsDropped -= MakePlayerWin;
    }

    protected override void ParameterInitialization()
    {
        PlayerHand = GetComponent<Big2PlayerHand>();
        cardSubmissionCheck = GetComponent<Big2CardSubmissionCheck>();
        big2AI = GetComponent<Big2SimpleAI>();
    }

    protected override void StateInitialization()
    {
        States[PlayerState.Pregame] = new Big2PlayerStatePreGame(PlayerState.Pregame, this);
        States[PlayerState.Playing] = new Big2PlayerStatePlaying(PlayerState.Playing, this);
        States[PlayerState.Waiting] = new Big2PlayerStatePlaying(PlayerState.Waiting, this);
        States[PlayerState.Postgame] = new Big2PlayerStatePostGame(PlayerState.Postgame, this);
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
        //Debug.Log("playerHand.PlayerType : " + PlayerHand.PlayerType);
        return PlayerHand.PlayerType;
    }


    #endregion

    #region AI
    public bool CheckIfAI() 
    {
        if (PlayerHand.PlayerType == PlayerType.AI)        
            return true;       

        return false;
    }

    public void InitiateAiDecisionMaking() 
    {
        // sort the hand by best hand
        // make a list of possible hand rank and the corresponding card
        // check if it is higher/suittable for the table 
            // if not, skip turn
            // if yes, submit card
        // remove the submitted card
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
