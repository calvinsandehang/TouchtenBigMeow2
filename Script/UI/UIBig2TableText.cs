using Big2Meow.Player;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Big2Meow.UI
{
    public class UIBig2TableText : MonoBehaviour, ISubscriber
    {
        private const string FIVE_CARDS_LEFT_MESSAGE = "5 CARD LEFT!";
        private const string MUST_INCLUDE_THREE_OF_DIAMONDS_MESSAGE = "MUST INCLUDE THREE OF DIAMONDS";

        [SerializeField] private TextMeshProUGUI _big2TableText;
        [SerializeField] private Color _textColor = Color.red;
        [SerializeField] private Color _initialColor = Color.red;
        [SerializeField] private float _fadeInDuration = 1.0f;
        [SerializeField] private float _stayDuration = 2.0f;
        [SerializeField] private float _fadeOutDuration = 1.0f;

        private bool warningCardLessThanSixTextActive = false;
        private bool warningMustIncludeThreeOfDiamondsTextActive = false;

        void Start()
        {
            _big2TableText.color = _initialColor;
            SubscribeEvent();

            // Optionally display the message at start for testing
            // StartCoroutine(FadeTextInOut(FIVE_CARDS_LEFT_MESSAGE));
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }

        private void ResetState(Big2PlayerHand player) 
        {
            warningCardLessThanSixTextActive = false;
        }

        /// <summary>
        /// Starts the fade in and out animation for the provided message.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void FadeTextInOut(string message)
        {
            
            StartCoroutine(FadeTextInOutCoroutine(message));
        }

        public void FadeWarningTextInOut()
        {
            if (!warningCardLessThanSixTextActive) 
            {
                warningCardLessThanSixTextActive = true;

                StartCoroutine(FadeTextInOutCoroutine(FIVE_CARDS_LEFT_MESSAGE));
            }
        }

        public void FadeWarningMustIncludeThreeDiamondsTextInOut()
        {
            if (!warningMustIncludeThreeOfDiamondsTextActive)
            {
                warningMustIncludeThreeOfDiamondsTextActive = true;

                StartCoroutine(FadeTextInOutCoroutine(MUST_INCLUDE_THREE_OF_DIAMONDS_MESSAGE));
            }
        }

        /// <summary>
        /// Coroutine to fade in and out the specified text message.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <returns>IEnumerator for coroutine.</returns>
        private IEnumerator FadeTextInOutCoroutine(string message)
        {
            _big2TableText.text = message;

            // Fade In
            yield return StartCoroutine(FadeTextToFullAlpha(_fadeInDuration, _big2TableText));
            // Stay
            yield return new WaitForSeconds(_stayDuration);
            // Fade Out
            yield return StartCoroutine(FadeTextToZeroAlpha(_fadeOutDuration, _big2TableText));

            warningMustIncludeThreeOfDiamondsTextActive = false;
        }

        /// <summary>
        /// Coroutine to fade text to full alpha.
        /// </summary>
        /// <param name="t">Duration of the fade.</param>
        /// <param name="text">The TextMeshProUGUI component to fade.</param>
        /// <returns>IEnumerator for coroutine.</returns>
        private IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI text)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            while (text.color.a < 1.0f)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / t));
                yield return null;
            }
        }

        /// <summary>
        /// Coroutine to fade text to zero alpha.
        /// </summary>
        /// <param name="t">Duration of the fade.</param>
        /// <param name="text">The TextMeshProUGUI component to fade.</param>
        /// <returns>IEnumerator for coroutine.</returns>
        private IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI text)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
            while (text.color.a > 0.0f)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / t));
                yield return null;
            }
        }

        public void SubscribeEvent()
        {
            Big2GlobalEvent.SubscribePlayerCardLessThanSix(FadeWarningTextInOut);
            Big2GlobalEvent.SubscribePlayerDropLastCard(ResetState);
            Big2GlobalEvent.SubscribeMustIncludeThreeOfDiamond(FadeWarningMustIncludeThreeDiamondsTextInOut);
        }

        public void UnsubscribeEvent()
        {
            Big2GlobalEvent.UnsubscribePlayerCardLessThanSix(FadeWarningTextInOut);
            Big2GlobalEvent.UnsubscribePlayerDropLastCard(ResetState);
            Big2GlobalEvent.UnsubscribeMustIncludeThreeOfDiamond(FadeWarningMustIncludeThreeDiamondsTextInOut);
        }
    }
}
