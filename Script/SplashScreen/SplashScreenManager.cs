using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Big2Meow.SceneManager;

/// <summary>
/// Manages the display of a splash screen with a logo and accompanying audio
/// before transitioning to the next scene.
/// </summary>
public class SplashScreenManager : MonoBehaviour
{
    #region Fields

    [Header("Logo Display")]
    [SerializeField] private Image _logo;             // The logo image to display.
    [SerializeField] private float _displayTime = 2.0f; // The time to display the logo.
    [SerializeField] private float _fadeDuration = 1.0f; // Duration of the fade effect.

    [Header("Audio")]
    [SerializeField] private AudioClip _logoClip;     // Audio clip that plays with the logo.
    [SerializeField] private float _volume = 0.5f;    // Volume of the audio clip.

    [Header("Next Scene")]
    [SerializeField] private string _nextSceneName;   // Name of the next scene to load.

    #endregion

    #region MonoBehaviour Callbacks

    /// <summary>
    /// Unity's Start method that begins the splash screen sequence.
    /// </summary>
    private void Start()
    {
        StartCoroutine(ShowSplash());
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Coroutine to display the splash screen, play the audio, and transition to the next scene.
    /// </summary>
    private IEnumerator ShowSplash()
    {
        SetLogoAlpha(0);
        yield return Fade(0, 1); // Fade-in effect.

        // Play the logo audio clip with specified volume.
        AudioSource.PlayClipAtPoint(_logoClip, Camera.main.transform.position, _volume);
        yield return new WaitForSeconds(_displayTime);

        yield return Fade(1, 0); // Fade-out effect.

        // Load the next scene after the splash screen has completed.
        SceneLoader.Instance.LoadNextScene(_nextSceneName);
    }

    /// <summary>
    /// Fades the logo's alpha from startAlpha to endAlpha over the duration of _fadeDuration.
    /// </summary>
    /// <param name="startAlpha">The initial alpha value.</param>
    /// <param name="endAlpha">The final alpha value.</param>
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / _fadeDuration);
            SetLogoAlpha(alpha);
            yield return null;
        }

        // Ensure the final alpha is set to avoid any discrepancies caused by frame timing.
        SetLogoAlpha(endAlpha);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Sets the alpha value of the logo image.
    /// </summary>
    /// <param name="alpha">The alpha value to set to the logo's color.</param>
    private void SetLogoAlpha(float alpha)
    {
        _logo.color = new Color(_logo.color.r, _logo.color.g, _logo.color.b, alpha);
    }

    #endregion
}
