using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages the display of a splash screen with a logo and audio before transitioning to the next scene.
/// </summary>
public class SplashScreenManager : MonoBehaviour
{
    [Header("Logo Display")]
    [SerializeField] private Image _logo;
    [SerializeField] private float _displayTime = 2.0f;
    [SerializeField] private float _fadeDuration = 1.0f;

    [Header("Audio")]
    [SerializeField] private AudioClip _logoClip;
    [SerializeField] private float _volume = 0.5f;

    [Header("Next Scene")]
    [SerializeField] private string _nextSceneName;

    private void Start()
    {
        StartCoroutine(ShowSplash());
    }

    private IEnumerator ShowSplash()
    {
        SetLogoAlpha(0);
        yield return Fade(0, 1);

        // Play the logo audio clip.
        Big2SfxManager.Instance.PlayClip(_logoClip);
        yield return new WaitForSeconds(_displayTime);

        yield return Fade(1, 0);

        // Load the next scene after the splash screen.
        SceneLoader.Instance.LoadNextScene(_nextSceneName);
    }

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
    }

    private void SetLogoAlpha(float alpha)
    {
        _logo.color = new Color(_logo.color.r, _logo.color.g, _logo.color.b, alpha);
    }
}
