using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public class Big2TableStatePreparation: BaseState<TableState>
{
    private Big2TableStateMachine tableStateMachine;
    public Big2TableStatePreparation(TableState key, Big2TableStateMachine stateMachine) : base(key)
    {
        tableStateMachine = stateMachine;
    }


    public override void EnterState()
    {
        // tell the UI
        // Dont limit the submission

    }

    public override void ExitState()
    {

    }

    public override TableState GetNextState()
    {
        return TableState.Preparation;
    }

    public override void UpdateState()
    {
    }    
}
