using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public class Big2TableStateStart : BaseState<TableState>
{
    private Big2TableStateMachine tableStateMachine;
    public Big2TableStateStart(TableState key, Big2TableStateMachine stateMachine) : base(key)
    {
        tableStateMachine = stateMachine;
    }


    public override void EnterState()
    {
        Debug.Log("Table State : Enter Single State");
    }

    public override void ExitState()
    {
        Debug.Log("Table State : Exit Single State");
    }

    public override TableState GetActiveState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
    }
}
