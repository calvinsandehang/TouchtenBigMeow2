using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2GMStateAskPlayer : BaseState<GMState>
{
    private Big2GMStateMachine gmStateMachine;
    public Big2GMStateAskPlayer(GMState key, Big2GMStateMachine stateMachine) : base(key)
    {
        gmStateMachine = stateMachine;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override GMState GetNextState()
    {
        return GMState.CloseGame;
    }

    public override void UpdateState()
    {
    }
}
