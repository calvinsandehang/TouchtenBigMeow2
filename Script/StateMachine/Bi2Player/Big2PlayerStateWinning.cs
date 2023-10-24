using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2PlayerStateWinning : BaseState<PlayerState>
{
    private Big2PlayerStateMachine playerStateMachine;
    public Big2PlayerStateWinning(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
    {
        playerStateMachine = stateMachine;
    }

    public override void EnterState()
    {
        int playerID = playerStateMachine.PlayerHand.PlayerID;
        Debug.Log("Player " + (playerID) + " is in Winning state");
    }

    public override void ExitState()
    {
    }

    public override PlayerState GetActiveState()
    {
        return PlayerState.Winning;
    }

    public override void UpdateState()
    {
    }
}
