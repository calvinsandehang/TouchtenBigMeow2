using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public class Big2TableStatePlay : BaseState<TableState>
{
    public Big2TableStatePlay(TableState key, StateManager<TableState> stateManager) : base(key, stateManager)
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
        return TableState.Play;
    }

    public override void UpdateState()
    {
    }   
}
