    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2GMStateCloseGame : BaseState<GMState>
{
    private Big2GMStateMachine GMSM;
    public Big2GMStateCloseGame(GMState key, Big2GMStateMachine stateMachine) : base(key)
    {
        GMSM = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("GM in Close Game State");
        GMSM.BroadcastGameHasEnded();
        SetGameNotInFirstGame();
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

    private void SetGameNotInFirstGame() 
    {
        GMSM.FirstGame = false;
    }
}
