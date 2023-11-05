using Big2Meow.Gameplay;
using Big2Meow.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Big2Meow.UI
{
    /// <summary>
    /// Represents a UI element that displays a skip notification and fades it out after a delay.
    /// </summary>
    public class UISkipNotification : MonoBehaviour, ISubscriber
    {
        public float fadeInDuration = 0.5f;
        public float fadeOutDuration = 0.5f;
        public float displayDuration = 1.0f;

        private CanvasGroup canvasGroup;
        private Big2PlayerSkipTurnHandler playerSkipHandler;
        private Big2PlayerHand playerHand;


        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            // Hide the element initially
            canvasGroup.alpha = 0f;
            SubscribeEvent();
        }

        /// <summary>
        /// Initializes the UI element with the specified player skip handler and player hand.
        /// </summary>
        /// <param name="playerSkipHandler">The player skip turn handler associated with the element.</param>
        /// <param name="playerHand">The player hand associated with the element.</param>
        public void InitializeUIElement(Big2PlayerSkipTurnHandler playerSkipHandler, Big2PlayerHand playerHand)
        {
            this.playerSkipHandler = playerSkipHandler;
            this.playerHand = playerHand;
        }

        // Method to instantly show the element and then fade out after a delay
        private void ShowAndFadeOut(Big2PlayerHand playerHand)
        {
            if (this.playerHand != playerHand) return;
            StartCoroutine(ShowAndFadeOutCoroutine());
        }

        private IEnumerator ShowAndFadeOutCoroutine()
        {
            // Fade in
            float elapsedTime = 0f;
            canvasGroup.alpha = 1f;

            // Display for a specified duration
            yield return new WaitForSeconds(displayDuration);

            // Fade out
            elapsedTime = 0f;
            while (elapsedTime < fadeOutDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
                canvasGroup.alpha = alpha;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }

        public void SubscribeEvent()
        {
            Big2GlobalEvent.SubscribeAISkipTurnGlobal(ShowAndFadeOut);
            Big2GlobalEvent.SubscribePlayerSkipTurnGlobal(ShowAndFadeOut);
        }

        public void UnsubscribeEvent()
        {
            Big2GlobalEvent.UnsubscribeAISkipTurnGlobal(ShowAndFadeOut);
            Big2GlobalEvent.UnsubscribePlayerSkipTurnGlobal(ShowAndFadeOut);
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }
    }
}

