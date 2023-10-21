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

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        skipTurnButton = GetComponent<Button>();
        
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

        SubscribeEvent();
        OnNotAllowedToSkipTurn();
    }
    private void OnAllowedToSkipTurn()
    {
        if (playerStateMachine == null || playerType != PlayerType.Human) return;
        // Set the button interactable and update the image color to allowedColor
        skipTurnButton.interactable = true;
        buttonImage.color = allowedColor;

        Debug.Log("OnAllowedToSkipTurn()");
    }

    private void OnNotAllowedToSkipTurn()
    {
        Debug.Log("OnNotAllowedToSkipTurn()_1");
        if (playerStateMachine == null || playerType != PlayerType.Human) return;

        // Set the button not interactable and update the image color to notAllowedColor
        skipTurnButton.interactable = false;
        buttonImage.color = notAllowedColor;

        Debug.Log("OnNotAllowedToSkipTurn()_2");
    }

    public void SubscribeEvent()
    {
        playerStateMachine.onPlayerIsPlaying += OnAllowedToSkipTurn;
        playerStateMachine.onPlayerIsLosing += OnNotAllowedToSkipTurn;
        playerStateMachine.onPlayerIsWinning += OnNotAllowedToSkipTurn;
        playerStateMachine.onPlayerIsWaiting += OnNotAllowedToSkipTurn;
    }

    public void UnsubscribeEvent()
    {
        playerStateMachine.onPlayerIsPlaying -= OnAllowedToSkipTurn;
        playerStateMachine.onPlayerIsLosing -= OnNotAllowedToSkipTurn;
        playerStateMachine.onPlayerIsWinning -= OnNotAllowedToSkipTurn;
        playerStateMachine.onPlayerIsWaiting -= OnNotAllowedToSkipTurn;
    }
}
