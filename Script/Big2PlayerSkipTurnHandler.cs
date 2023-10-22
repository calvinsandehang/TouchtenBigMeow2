using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

[DefaultExecutionOrder(1)]
public class Big2PlayerSkipTurnHandler : MonoBehaviour
{
    private Big2GMStateMachine gMStateMachine;
    private Big2PlayerStateMachine playerStateMachine;
    private Big2PlayerHand playerHand;
    private Button skipTurnButton;

    public static event Action<Big2PlayerHand> OnPlayerSkipTurn;

    private void Awake()
    {
        gMStateMachine = GetComponent<Big2GMStateMachine>();   
        playerStateMachine = GetComponent<Big2PlayerStateMachine>();
        playerHand = GetComponent<Big2PlayerHand>();            
    }

    private void Start()
    {
        if (playerHand.PlayerType == PlayerType.Human)
        {
            SetupSkipTurnButton();
        }
        
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
        StartCoroutine(DelayedAction()); 
    }

    private void MakePlayerInWaitingState()
    {
        // Call the MakePlayerWait method on the player state machine
        playerStateMachine.MakePlayerWait();
    }

    private IEnumerator DelayedAction() 
    {
        yield return new WaitForSeconds(2f);
        OnPlayerSkipTurn?.Invoke(playerHand);
    }

}
