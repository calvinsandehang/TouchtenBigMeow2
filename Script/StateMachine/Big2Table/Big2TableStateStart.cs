using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public class Big2TableStateStart : BaseState<TableState>
{
    public Big2TableStateStart(TableState key, StateManager<TableState> stateManager) : base(key, stateManager)
    {

    }

    public override void EnterState()
    {
        Debug.Log("Table State : Enter Single State");
    }

    public override void ExitState()
    {
        Debug.Log("Table State : Exit Single State");
    }

    public override TableState GetNextState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
    }
}
