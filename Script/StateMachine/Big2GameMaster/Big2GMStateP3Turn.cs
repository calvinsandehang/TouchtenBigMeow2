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
        gmStateMachine.OrderPlayerToPlay(3);
    }

    public override void ExitState()
    {
    }

    public override GMState GetActiveState()
    {
        return GMState.P3Turn;
    }

    public override void UpdateState()
    {
    }

  
}
