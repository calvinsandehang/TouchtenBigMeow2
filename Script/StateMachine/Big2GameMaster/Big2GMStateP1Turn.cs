using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2GMStateP1Turn : BaseState<GMState>
{
    private Big2GMStateMachine gmStateMachine;
    public Big2GMStateP1Turn(GMState key, Big2GMStateMachine stateMachine) : base(key)
    {
        gmStateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("GM in P1 Turn State");
        gmStateMachine.OrderPlayerToPlay(0);
    }

    public override void ExitState()
    {
    }

    public override GMState GetActiveState()
    {
        return GMState.P1Turn;
    }

    public override void UpdateState()
    {
    }

   

}
