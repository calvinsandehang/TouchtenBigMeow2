using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2GMStateP2Turn : BaseState<GMState>
{
    private Big2GMStateMachine gmStateMachine;
    public Big2GMStateP2Turn(GMState key, Big2GMStateMachine stateMachine) : base(key)
    {
        gmStateMachine = stateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("GM in P2 Turn State");
        gmStateMachine.OrderPlayerToPlay(1);
    }

    public override void ExitState()
    {
    }

    public override GMState GetActiveState()
    {
        return GMState.P2Turn;
    }

    public override void UpdateState()
    {
    }

    public void OrderPlayerToPlay(int playerID)
    {
        Debug.Log("BOM");
        Debug.Log("BOM");
        Debug.Log("BOM");
        Debug.Log("BOM");
        Debug.Log("gmStateMachine");
        Big2PlayerStateMachine playerStateMachine = gmStateMachine.playerHands[playerID].GetComponent<Big2PlayerStateMachine>();
        playerStateMachine.MakePlayerPlay();
        Debug.Log("OrderPlayerToPlay");
    }
}
