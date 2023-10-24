using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPostGame : MonoBehaviour
{
    [SerializeField]
    private Button _continueButton, _exitButton;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
     
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        _continueButton.onClick.AddListener(OnContinueButtonPressed);

        HideButton();
        SubscribeEvent();
    }

    private void OnContinueButtonPressed()
    {
        Big2GMStateMachine.Instance.OnOpenGame();
        HideButton();
    }

    private void ShowButton()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void HideButton()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void SubscribeEvent()
    {
        Big2GMStateMachine.OnGameHasEnded += ShowButton;
    }

    public void UnsubscribeEvent()
    {
        Big2GMStateMachine.OnGameHasEnded -= ShowButton;
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }
}

