using UnityEngine;
using UnityEngine.UI;

namespace Big2Meow.UI 
{
    public class UIApplicationQuitButton : MonoBehaviour
    {
        [SerializeField] private Button _quitButton; // Reference to the UI button that will be used to quit the application

        // Start is called before the first frame update
        void Start()
        {
            // Ensure the quitButton has been assigned in the inspector to avoid null reference errors
            if (_quitButton != null)
            {
                // Add a listener to the button that calls the QuitApplication method when clicked
                _quitButton.onClick.AddListener(QuitApplication);
            }
            else
            {
                Debug.LogError("Quit button not assigned in the inspector", this);
            }
        }

        /// <summary>
        /// Quits the application.
        /// </summary>
        private void QuitApplication()
        {
            // Logs the quit action - useful for confirming the quit in the editor
            Debug.Log("Quitting application");

            // Quit the application
            Application.Quit();

            // If we're running in the editor, stop play mode
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

}
