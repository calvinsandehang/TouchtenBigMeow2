using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2GMStateP0Turn : BaseState<GMState>
{
    private Big2GMStateMachine gmStateMachine;
    public Big2GMStateP0Turn(GMState key, Big2GMStateMachine stateMachine) : base(key)
    {
        gmStateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("GM in P0 Turn State");
        gmStateMachine.OrderPlayerToPlay(0);
    }

    public override void ExitState()
    {
    }

    public override GMState GetActiveState()
    {
        return GMState.P0Turn;
    }

    public override void UpdateState()
    {
    }

   

}
