using Big2Meow.FSM;
using Big2Meow.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

namespace Big2Meow.Player
{
    /// <summary>
    /// Manages the UI and interactions for a Big2 player.
    /// </summary>
    public class Big2PlayerUIManager : MonoBehaviour, ISubscriber
    {
        private Big2PlayerStateMachine playerSM;
        public UISkipNotification skipNotification;
        public Big2PlayerProfilePictureManager PlayerProfile;
        private Big2PlayerHand playerHand;
        private Big2PlayerSkipTurnHandler playerSkipHandler;
        private GameObject cardParent;

        [Header("Scale Effect Settings")]
        public Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);
        public float scaleDuration = 1.0f;

        #region Unity Callback
        private void Awake()
        {
            playerSM = GetComponent<Big2PlayerStateMachine>();
            playerHand = GetComponent<Big2PlayerHand>();
            playerSkipHandler = GetComponent<Big2PlayerSkipTurnHandler>();

            SubscribeEvent();
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }
        #endregion

        #region Setup UI components
        /// <summary>
        /// Sets up the UI element for skipping a turn.
        /// </summary>
        /// <param name="skipNotification">The UISkipNotification element to initialize.</param>
        public void SetupSkipNotificationButton(UISkipNotification skipNotification)
        {
            this.skipNotification = skipNotification;
            skipNotification.InitializeUIElement(playerSkipHandler, playerHand);
        }

        /// <summary>
        /// Sets up the player's profile picture.
        /// </summary>
        /// <param name="playerProfile">The Big2PlayerProfilePictureManager to initialize.</param>
        public void SetupProfilePicture(Big2PlayerProfilePictureManager playerProfile)
        {
            this.PlayerProfile = playerProfile;
            playerProfile.InitializePlayerProfile(playerSM);
        }

        /// <summary>
        /// Sets the parent object for the player's cards.
        /// </summary>
        /// <param name="cardParent">The GameObject representing the parent of the player's cards.</param>
        public void SetupCardParent(GameObject cardParent)
        {
            this.cardParent = cardParent;
        }
        #endregion

        #region Visual Effects
        /// <summary>
        /// Activates a scale effect on the player's cards.
        /// </summary>
        private void ActivateScaleEffect()
        {
            if (cardParent != null)
            {
                StartCoroutine(ScaleEffectCoroutine());
            }
            else
            {
                Debug.LogError("Error: cardParent is not set for " + gameObject.name);
            }
        }

        /// <summary>
        /// Coroutine for applying a scale effect to the card parent.
        /// </summary>
        /// <remarks>
        /// This coroutine gradually scales the card parent object from its original scale to the maximum scale defined in maxScale over a specified duration.
        /// </remarks>
        private IEnumerator ScaleEffectCoroutine()
        {
            Vector3 originalScale = cardParent.transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < scaleDuration)
            {
                float progress = elapsedTime / scaleDuration;
                cardParent.transform.localScale = Vector3.Lerp(originalScale, maxScale, progress);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Reset the scale to its original value after the effect is complete
            cardParent.transform.localScale = originalScale;
        }
        #endregion

        #region Subscribe Event
        public void SubscribeEvent()
        {
            playerSM.OnPlayerIsPlaying += ActivateScaleEffect;
        }

        public void UnsubscribeEvent()
        {
            playerSM.OnPlayerIsPlaying -= ActivateScaleEffect;
        }
        #endregion

    }
}

