using UnityEngine;
using System.Collections;

namespace Big2Meow.SceneManager
{
    /// <summary>
    /// Manages scene loading and transitions using a screen fader.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// Gets the singleton instance of the SceneLoader.
        /// </summary>
        public static SceneLoader Instance { get; private set; }

        [SerializeField]
        private ScreenFader _screenFader;

        [SerializeField]
        private float _fadeDuration = 2.0f;

        private void Awake()
        {
            // Singleton pattern: Ensure there is only one instance of SceneLoader.
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                // If an instance already exists, destroy this GameObject.
                Destroy(gameObject);
                return;
            }
        }

        /// <summary>
        /// Loads the next scene with a transition effect.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void LoadNextScene(string sceneName)
        {
            StartCoroutine(TransitionToScene(sceneName));
        }

        private IEnumerator TransitionToScene(string sceneName)
        {
            // Fade the screen to black before loading the new scene.
            yield return _screenFader.FadeToBlack(_fadeDuration);

            // Load the new scene.
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

            // Wait for one frame for the scene to fully load.
            yield return null;

            // Fade the screen from black after the new scene is loaded.
            yield return _screenFader.FadeFromBlack(_fadeDuration);
        }
    }
}


