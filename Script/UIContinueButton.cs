using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContinueButton : MonoBehaviour, ISubscriber
{
    private Button continueButton;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        continueButton = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (continueButton == null) 
        {
            continueButton = gameObject.AddComponent<Button>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        continueButton.onClick.AddListener(OnContinueButtonPressed);

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
        Big2GlobalEvent.SubscribeGameHasEnded(ShowButton);
    }
   
    public void UnsubscribeEvent()
    {
        Big2GlobalEvent.UnsubscribeGameHasEnded(ShowButton);
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }
}
