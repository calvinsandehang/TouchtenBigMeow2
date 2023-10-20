using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2PlayerStateLosing : BaseState<PlayerState>
{
    private Big2PlayerStateMachine playerStateMachine;
    public Big2PlayerStateLosing(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
    {
        playerStateMachine = stateMachine;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override PlayerState GetNextState()
    {
        return PlayerState.Waiting;
    }

    public override void UpdateState()
    {
    }
}
