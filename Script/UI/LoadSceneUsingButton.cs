using Big2Meow.SceneManager;
using UnityEngine;
using UnityEngine.UI;

namespace Big2Meow.UI
{
    /// <summary>
    /// A MonoBehaviour class for loading scenes using a button click.
    /// </summary>
    public class LoadSceneUsingButton : MonoBehaviour
    {
        [SerializeField]
        private Button _loadSceneButton;

        [SerializeField]
        private string _sceneNameToLoad;

        /// <summary>
        /// Initializes the button click event.
        /// </summary>
        private void Start()
        {
            // Add a listener to the button's click event
            _loadSceneButton.onClick.AddListener(LoadScene);
        }

        /// <summary>
        /// Loads the specified scene using the SceneLoader instance.
        /// </summary>
        private void LoadScene()
        {
            // Check if a SceneLoader instance exists
            if (SceneLoader.Instance != null)
            {
                // Load the specified scene using the SceneLoader
                SceneLoader.Instance.LoadNextScene(_sceneNameToLoad);
            }
            else
            {
                // Log an error message if SceneLoader instance is not found
                Debug.LogError("SceneLoader instance not found!");
            }
        }
    }
}
  
