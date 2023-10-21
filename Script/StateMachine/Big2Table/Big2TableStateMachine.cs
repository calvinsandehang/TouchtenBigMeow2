using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;



public enum TableState 
{
    Preparation,
    Start,
    Play,
    End,
}

[DefaultExecutionOrder(-9999)]
public class Big2TableStateMachine : StateManager<TableState>
{
    public static Big2TableStateMachine Instance;
    public TableState TableState { get; private set; }

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        StateInitialization();

        CurrentState = States[TableState.Preparation];
    }

    private void StateInitialization()
    {
        States[TableState.Preparation] = new Big2TableStatePreparation(TableState.Preparation, this);
        States[TableState.Start] = new Big2TableStateStart(TableState.Start, this);
        States[TableState.Play] = new Big2TableStatePlay(TableState.Play, this);
        States[TableState.End] = new Big2TableStateEnd(TableState.End, this);
    }

    

    #region Helper
    
    #endregion
}
