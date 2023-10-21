using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2TableStateEnd : BaseState<TableState>
{
    private Big2TableStateMachine tableStateMachine;
    public Big2TableStateEnd(TableState key, Big2TableStateMachine stateMachine ) : base(key)
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
        return TableState.End;
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

   
}
