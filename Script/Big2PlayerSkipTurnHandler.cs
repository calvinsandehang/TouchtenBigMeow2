using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

public class Big2PlayerSkipTurnHandler : MonoBehaviour
{
    private Big2GMStateMachine gMStateMachine;
    private Big2PlayerStateMachine playerStateMachine;
    private Button skipTurnButton;

    private void Awake()
    {
        gMStateMachine = GetComponent<Big2GMStateMachine>();   
        playerStateMachine = GetComponent<Big2PlayerStateMachine>();
        
    }

    private void Start()
    {
        SetupSkipTurnButton();
    }
    private void SetupSkipTurnButton()
    {
        skipTurnButton = UIButtonInjector.Instance.GetButton(ButtonType.SkipTurn);
        skipTurnButton.onClick.AddListener(TellGMToGoNextTurn);
        skipTurnButton.onClick.AddListener(MakePlayerInWaitingState);

        UIPlayerSkipTurnButton skipTurnButtonBehaviour = skipTurnButton.GetComponent<UIPlayerSkipTurnButton>();
        skipTurnButtonBehaviour.InitializeButton(playerStateMachine);
    }

    // Assuming skipTurnButton ad waitButton are your buttons
    

    private void TellGMToGoNextTurn()
    {
        // Call the NextTurn method on the game state machine
        gMStateMachine.NextTurn();
    }

    private void MakePlayerInWaitingState()
    {
        // Call the MakePlayerWait method on the player state machine
        playerStateMachine.MakePlayerWait();
    }

}
