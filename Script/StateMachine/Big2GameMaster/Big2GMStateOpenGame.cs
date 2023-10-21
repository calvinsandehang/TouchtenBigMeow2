using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2GMStateOpenGame : BaseState<GMState>
{
    private Big2GMStateMachine gmStateMachine;
    public Big2GMStateOpenGame(GMState key, Big2GMStateMachine stateMachine) : base(key)
    {
        gmStateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("GM in Open Game State");
       gmStateMachine.InitializeGame();
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
