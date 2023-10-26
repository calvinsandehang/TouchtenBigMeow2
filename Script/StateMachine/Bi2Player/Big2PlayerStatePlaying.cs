using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2PlayerStatePlaying : BaseState<PlayerState>
{
    private Big2PlayerStateMachine PSM;
    public Big2PlayerStatePlaying(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
    {
        PSM = stateMachine;
    }

    public override void EnterState()
    {
        int playerID = PSM.PlayerHand.PlayerID;
        Debug.Log("Player " + (playerID) + " is in Playing state");

        PSM.BroadcastPlayerIsPlaying();

        if (PSM.PlayerHand.PlayerType != GlobalDefine.PlayerType.Human)
            PSM.Big2AI.InitiateAiDecisionMaking();

    }

    public override void ExitState()
    {
    }

    public override PlayerState GetActiveState()
    {
        return PlayerState.Playing;
    }

    public override void UpdateState()
    {
    }

}
