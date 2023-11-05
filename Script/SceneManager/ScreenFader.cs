using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Big2Meow.SceneManager 
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField]
        private Image _fadePanel;

        [SerializeField]
        private float _fadeDuration = 2.0f;


        private void Awake()
        {
            if (_fadeDuration <= 0f)
            {
                Debug.LogWarning("Fade duration must be greater than zero. Setting it to a default value.");
                _fadeDuration = 2.0f; // Set a default value (or choose another suitable value).
            }

            SetAlpha(0); // Initially set the alpha to transparent
        }

        /// <summary>
        /// Fades the screen to black over a specified duration.
        /// </summary>
        /// <param name="duration">The duration of the fade effect.</param>
        /// <returns>An IEnumerator for the fading process.</returns>
        public IEnumerator FadeToBlack(float duration)
        {
            yield return Fade(0, 1, duration);
        }

        /// <summary>
        /// Fades the screen from black to transparent over a specified duration.
        /// </summary>
        /// <param name="duration">The duration of the fade effect.</param>
        /// <returns>An IEnumerator for the fading process.</returns>
        public IEnumerator FadeFromBlack(float duration)
        {
            yield return Fade(1, 0, duration);
        }

        private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                _fadePanel.color = new Color(_fadePanel.color.r, _fadePanel.color.g, _fadePanel.color.b, alpha);
                yield return null;
            }
        }

        private void SetAlpha(float alpha)
        {
            Color panelColor = _fadePanel.color;
            panelColor.a = alpha;
            _fadePanel.color = panelColor;
        }
    }
}

