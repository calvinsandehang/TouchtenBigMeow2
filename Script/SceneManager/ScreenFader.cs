using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [SerializeField]
    private Image fadePanel;

    [SerializeField]
    private float fadeDuration = 2.0f;

    public Image FadePanel => fadePanel; // Auto-implemented property for accessing the fadePanel field

    private void Awake()
    {
        if (fadeDuration <= 0f)
        {
            Debug.LogWarning("Fade duration must be greater than zero. Setting it to a default value.");
            fadeDuration = 2.0f; // Set a default value (or choose another suitable value).
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
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, alpha);
            yield return null;
        }
    }

    private void SetAlpha(float alpha)
    {
        Color panelColor = fadePanel.color;
        panelColor.a = alpha;
        fadePanel.color = panelColor;
    }
}
