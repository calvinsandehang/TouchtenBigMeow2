using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public class Big2TableStatePreparation: BaseState<TableState>
{
    public Big2TableStatePreparation(TableState key, StateManager<TableState> stateManager) : base(key, stateManager)
    {
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
