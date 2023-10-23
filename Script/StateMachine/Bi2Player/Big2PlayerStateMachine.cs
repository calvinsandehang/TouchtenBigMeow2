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

    
    public event Action OnPlayerIsPlaying;
    public event Action OnPlayerIsWinning;
    public event Action OnPlayerIsLosing;
    public event Action OnPlayerIsWaiting;

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
        cardSubmissionCheck.OnPlayerFinishTurnLocal += MakePlayerWait;
        big2AI.OnAIFinishTurnLocal += MakePlayerWait;
    }

    public void UnsubscribeEvent()
    {
        PlayerHand.OnHandLastCardIsDropped -= MakePlayerWin;
        cardSubmissionCheck.OnPlayerFinishTurnLocal += MakePlayerWait;
        big2AI.OnAIFinishTurnLocal -= MakePlayerWait;
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
        States[PlayerState.Waiting] = new Big2PlayerStateWaiting(PlayerState.Waiting, this);
        States[PlayerState.Postgame] = new Big2PlayerStatePostGame(PlayerState.Postgame, this);
        States[PlayerState.Winning] = new Big2PlayerStateWinning(PlayerState.Winning, this);
        States[PlayerState.Losing] = new Big2PlayerStateLosing(PlayerState.Losing, this);
    }

    #region Order
    public void MakePlayerWait() 
    {
        NextState = States[PlayerState.Waiting];
    }

    public void MakePlayerPlay() 
    {
        NextState = States[PlayerState.Playing];

        // Card evaluator can be used
        // can submit
        // can pass
    }
    public void MakePlayerLose() 
    {
        NextState = States[PlayerState.Losing];
        // do player lose stuff
    }

    public void MakePlayerWin() 
    {
        NextState = States[PlayerState.Winning];
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

    #region event
    public void BroadcastPlayerIsPlaying() 
    {
        OnPlayerIsPlaying?.Invoke();
       
        //Debug.Log("BroadcastPlayerIsPlaying() ");
    }

    public void BroadcastPlayerIsWaiting()
    {
        OnPlayerIsWaiting?.Invoke();
        //Debug.Log("BroadcastPlayerIsWaiting() ");
    }

    #endregion



    #region Helper
    private void OnDisable()
    {
        UnsubscribeEvent();
    }
    #endregion
}
