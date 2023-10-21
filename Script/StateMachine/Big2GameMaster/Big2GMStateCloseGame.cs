    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2GMStateCloseGame : BaseState<GMState>
{
    private Big2GMStateMachine gmStateMachine;
    public Big2GMStateCloseGame(GMState key, Big2GMStateMachine stateMachine) : base(key)
    {
        gmStateMachine = stateMachine;
    }

    public override void EnterState()
    {
        gmStateMachine.SetGameNotInFirstRound();
    }

    public override void ExitState()
    {
    }

    public override GMState GetActiveState()
    {
        return GMState.CloseGame;
    }

    public override void UpdateState()
    {
    }
}
