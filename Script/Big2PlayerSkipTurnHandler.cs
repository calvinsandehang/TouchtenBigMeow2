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

    public static event Action<Big2PlayerHand> OnPlayerSkipTurnGlobal; // subs : Big2GMStateMachine
    public static event Action OnPlayerSkipTurnLocal;

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

        UIPlayerSkipTurnButton skipTurnButtonBehaviour = skipTurnButton.GetComponent<UIPlayerSkipTurnButton>();
        skipTurnButtonBehaviour.InitializeButton(playerStateMachine);
    }

    private void InitializeUIElement() 
    {
        
    }

    // Assuming skipTurnButton ad waitButton are your buttons    

    private void TellGMToGoNextTurn()
    {
        StartCoroutine(DelayedAction()); 
    }

    private IEnumerator DelayedAction() 
    {
        yield return new WaitForSeconds(0.1f);
        OnPlayerSkipTurnGlobal?.Invoke(playerHand);
    }

}
