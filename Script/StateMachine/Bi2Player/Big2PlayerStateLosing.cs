using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2PlayerStateLosing : BaseState<PlayerState>
{
    private Big2PlayerStateMachine PSM;
    public Big2PlayerStateLosing(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
    {
        PSM = stateMachine;
    }

    public override void EnterState()
    {
        int playerID = PSM.PlayerHand.PlayerID;
        Debug.Log("Player " + (playerID) + " is in Losing state");

        PSM.BroadcastPlayerIsLosing();
    }

    public override void ExitState()
    {
    }

    public override PlayerState GetActiveState()
    {
        return PlayerState.Losing;
    }

    public override void UpdateState()
    {
    }
}
