using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2PlayerStatePreGame : BaseState<PlayerState>
{
    private Big2PlayerStateMachine playerStateMachine;
    public Big2PlayerStatePreGame(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
    {
        playerStateMachine = stateMachine;
    }

    public override void EnterState()
    {
        int playerID = playerStateMachine.PlayerHand.PlayerID;
        Debug.Log("Player " + (playerID+1) + " is in PreGame state");
    }

    public override void ExitState()
    {
    }

    public override PlayerState GetActiveState()
    {
        return PlayerState.Waiting;
    }

    public override void UpdateState()
    {
    }
}
