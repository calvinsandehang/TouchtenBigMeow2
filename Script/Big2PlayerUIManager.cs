using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public class Big2PlayerUIManager : MonoBehaviour, ISubscriber
{
    private Big2PlayerStateMachine playerSM;
    private Big2SimpleAI simpleAI;
    public UISkipNotification skipNotification;
    public Big2PlayerProfilePictureManager PlayerProfile;
    private Big2PlayerHand playerHand;
    private Big2PlayerSkipTurnHandler playerSkipHandler;
    private GameObject cardParent;

    [Header("Scale Effect Settings")]
    public Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);
    public float scaleDuration = 1.0f;
    public AnimationCurve scaleCurve;

    private void Awake()
    {
        simpleAI = GetComponent<Big2SimpleAI>();
        playerSM = GetComponent<Big2PlayerStateMachine>();
        playerHand = GetComponent<Big2PlayerHand>();
        playerSkipHandler = GetComponent<Big2PlayerSkipTurnHandler>();

        SubscribeEvent();
    }

    public void SetupSkipNotificationButton(UISkipNotification skipNotification)
    {
        this.skipNotification = skipNotification;
        skipNotification.InitializeUIElement(playerSkipHandler, playerHand);
    }

    public void SetupProfilePicture(Big2PlayerProfilePictureManager playerProfile)
    {
        this.PlayerProfile = playerProfile;
        playerProfile.InitializePlayerProfile(playerSM);
    }

    public void SetupCardParent(GameObject cardParent)
    {
        this.cardParent = cardParent;
    }

    public void SubscribeEvent()
    {
        playerSM.OnPlayerIsPlaying += ActivateScaleEffect;
    }    

    public void UnsubscribeEvent()
    {
        playerSM.OnPlayerIsPlaying -= ActivateScaleEffect;
    }

    private void ActivateScaleEffect()
    {
        if (cardParent != null) // Check if cardParent has been set
        {
            StartCoroutine(ScaleEffectCoroutine());
        }
        else
        {
            Debug.LogError("cardParent is not set.");
        }
    }

    private IEnumerator ScaleEffectCoroutine()
    {
        Vector3 originalScale = cardParent.transform.localScale;  // Get the original scale
        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            float progress = elapsedTime / scaleDuration;
            cardParent.transform.localScale = Vector3.Lerp(originalScale, maxScale, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset to the original scale after reaching max scale
        cardParent.transform.localScale = originalScale;
    }

}
