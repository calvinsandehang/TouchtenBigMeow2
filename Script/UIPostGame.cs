using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the post-game UI, including continue and exit buttons.
/// </summary>
public class UIPostGame : MonoBehaviour
{
    [SerializeField] private Button _continueButton, _exitButton;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        _continueButton.onClick.AddListener(OnContinueButtonPressed);
        HideButton();
        SubscribeEvent();
    }

    /// <summary>
    /// Handles the action when the continue button is pressed.
    /// </summary>
    private void OnContinueButtonPressed()
    {
        Big2GlobalEvent.BroadcastRestartGame();
        HideButton();
    }

    /// <summary>
    /// Shows the post-game UI buttons.
    /// </summary>
    private void ShowButton()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Hides the post-game UI buttons.
    /// </summary>
    private void HideButton()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Subscribes to the relevant events.
    /// </summary>
    private void SubscribeEvent()
    {
        Big2GlobalEvent.SubscribeAskPlayerInPostGame(ShowButton);
    }

    /// <summary>
    /// Unsubscribes from the relevant events.
    /// </summary>
    private void UnsubscribeEvent()
    {
        Big2GlobalEvent.UnsubscribeAskPlayerInPostGame(ShowButton);
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }
}
