using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2PlayerStateWaiting :BaseState<PlayerState>
{
    private Big2PlayerStateMachine playerStateMachine;
    public Big2PlayerStateWaiting(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
    {
        playerStateMachine = stateMachine;
    }

    public override void EnterState()
    {
        playerStateMachine.onPlayerIsWaiting?.Invoke();
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
