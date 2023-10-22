using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

[DefaultExecutionOrder(2)]
public class UIPlayerSkipTurnButton : MonoBehaviour, ISubscriber
{
    private Button skipTurnButton;
    private Image buttonImage;    

    [SerializeField]
    private Color allowedColor = Color.white;

    [SerializeField]
    private Color notAllowedColor = Color.gray;

    private Big2PlayerStateMachine playerStateMachine;
    private PlayerType playerType;

    private Big2TableManager tableManager;
    private HandType currentTableType;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        skipTurnButton = GetComponent<Button>();
        
    }

    private void Start()
    {
        ParameterInitialization();
    }

    private void ParameterInitialization()
    {
        tableManager = Big2TableManager.Instance;
    }

    private void OnDisable()
    {
        if (playerStateMachine == null)
            return;
        UnsubscribeEvent();
    }
    public void InitializeButton(Big2PlayerStateMachine playerStateMachine)
    {
        
        this.playerStateMachine = playerStateMachine;
        playerType = playerStateMachine.GetPlayerType();
        
        if (playerStateMachine == null || playerType != PlayerType.Human) return;
        Debug.Log("playerStateMachine : " + playerStateMachine);
        Debug.Log("playerType  : " + playerType);
        SubscribeEvent();
        OnNotAllowedToSkipTurn();
    }
    private void OnAllowedToSkipTurn()
    {
        currentTableType = tableManager.TableHandType;

        if (currentTableType == HandType.None)
            return;

        //if (playerStateMachine == null || playerType != PlayerType.Human) return;
        // Set the button interactable and update the image color to allowedColor
        skipTurnButton.interactable = true;
        buttonImage.color = allowedColor;

        Debug.Log("Allowed to skip");

    }

    private void OnNotAllowedToSkipTurn()
    {
        //Debug.Log("OnNotAllowedToSkipTurn()_1");
        //if (playerStateMachine == null || playerType != PlayerType.Human) return;

        // Set the button not interactable and update the image color to notAllowedColor
        skipTurnButton.interactable = false;
        buttonImage.color = notAllowedColor;

        Debug.Log("Not Allowed to skip");
    }

    public void SubscribeEvent()
    {
        playerStateMachine.OnPlayerIsPlaying += OnAllowedToSkipTurn;
        playerStateMachine.OnPlayerIsLosing += OnNotAllowedToSkipTurn;
        playerStateMachine.OnPlayerIsWinning += OnNotAllowedToSkipTurn;
        playerStateMachine.OnPlayerIsWaiting += OnNotAllowedToSkipTurn;
    }

    public void UnsubscribeEvent()
    {
        playerStateMachine.OnPlayerIsPlaying -= OnAllowedToSkipTurn;
        playerStateMachine.OnPlayerIsLosing -= OnNotAllowedToSkipTurn;
        playerStateMachine.OnPlayerIsWinning -= OnNotAllowedToSkipTurn;
        playerStateMachine.OnPlayerIsWaiting -= OnNotAllowedToSkipTurn;
    }
}
