using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2PlayerStateWinning : BaseState<PlayerState>
{
    private Big2PlayerStateMachine PSM;
    public Big2PlayerStateWinning(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
    {
        PSM = stateMachine;
    }

    public override void EnterState()
    {
        int playerID = PSM.PlayerHand.PlayerID;
        Debug.Log("Player " + (playerID) + " is in Winning state");

        PSM.BroadcastPlayerIsWinning();        
    }

    public override void ExitState()
    {
    }

    public override PlayerState GetActiveState()
    {
        return PlayerState.Winning;
    }

    public override void UpdateState()
    {
    }
}
