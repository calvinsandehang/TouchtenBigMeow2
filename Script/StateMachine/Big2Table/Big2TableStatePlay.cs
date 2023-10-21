using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public class Big2TableStatePlay : BaseState<TableState>
{
    private Big2TableStateMachine tableStateMachine;
    public Big2TableStatePlay(TableState key, Big2TableStateMachine stateMachine) : base(key)
    {
        tableStateMachine = stateMachine;
    }


    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override TableState GetActiveState()
    {
        return TableState.Play;
    }

    public override void UpdateState()
    {
    }   
}
