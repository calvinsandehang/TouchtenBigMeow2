using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2TableStateEnd : BaseState<TableState>
{
    public Big2TableStateEnd(TableState key, StateManager<TableState> stateManager) : base(key, stateManager)
    {
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override TableState GetNextState()
    {
        return TableState.End;
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

   
}
