using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2GMStateP3Turn : BaseState<GMState>
{
    private Big2GMStateMachine gmStateMachine;
    public Big2GMStateP3Turn(GMState key, Big2GMStateMachine stateMachine) : base(key)
    {
        gmStateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("GM in P3 Turn State");
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
