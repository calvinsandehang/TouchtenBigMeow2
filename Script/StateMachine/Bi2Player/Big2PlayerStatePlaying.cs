using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2PlayerStatePlaying : BaseState<PlayerState>
{
    private Big2PlayerStateMachine playerStateMachine;
    public Big2PlayerStatePlaying(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
    {
        playerStateMachine = stateMachine;
    }

    public override void EnterState()
    {
        int playerID = playerStateMachine.PlayerHand.PlayerID;
        Debug.Log("Player " + (playerID + 1) + " is in Playing state");
        
        if (playerStateMachine.PlayerHand.PlayerType == GlobalDefine.PlayerType.Human) 
        {
            playerStateMachine.onPlayerIsPlaying?.Invoke();
        }
        else
        {
            playerStateMachine.big2AI.InitiateAiDecisionMaking();
        }
       
    }

    public override void ExitState()
    {
    }

    public override PlayerState GetActiveState()
    {
        return PlayerState.Playing;
    }

    public override void UpdateState()
    {
    }

}
