using Big2Meow.SceneManager;
using UnityEngine;
using UnityEngine.UI;

namespace Big2Meow.UI
{
    /// <summary>
    /// Manages the display and functionality of the post-game UI.
    /// </summary>
    public class UIPostGame : MonoBehaviour
    {
        [Tooltip("Panel_PostGame game object")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Tooltip("Button to continue the game.")]
        [SerializeField] private Button _continueButton;

        [Tooltip("Button to exit to the main menu.")]
        [SerializeField] private Button _exitButton;

        [Tooltip("The name of the main menu scene to load when exiting.")]
        [SerializeField] private string _mainMenuScene;
        

        #region Monobehaviour
        /// <summary>
        /// Initializes the post-game UI by setting up button listeners and hiding the UI.
        /// </summary>
        private void Start()
        {
            // Assign the buttons' click listeners.
            _continueButton.onClick.AddListener(OnContinueButtonPressed);
            _exitButton.onClick.AddListener(OnExitButtonPressed);

            // Initially, the post-game UI should be hidden.
            HideButtons();

            // Listen to game events that dictate when the UI should be shown.
            SubscribeEvents();
        }

        /// <summary>
        /// Removes listeners upon the UI being disabled to prevent memory leaks.
        /// </summary>
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion

        #region Button methods
        /// <summary>
        /// Invoked when the continue button is pressed, broadcasting the restart game event and hiding the buttons.
        /// </summary>
        private void OnContinueButtonPressed()
        {
            Big2GlobalEvent.BroadcastRestartGame();
            HideButtons();
        }

        /// <summary>
        /// Invoked when the exit button is pressed, triggering the scene loader to switch to the main menu scene.
        /// </summary>
        private void OnExitButtonPressed()
        {
            SceneLoader.Instance.LoadNextScene(_mainMenuScene);
        }

        /// <summary>
        /// Makes the post-game UI visible and interactive to the player.
        /// </summary>
        private void ShowButtons()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Hides the post-game UI, making it invisible and non-interactive.
        /// </summary>
        private void HideButtons()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        #endregion

        #region Subscribe Event
        private void SubscribeEvents()
        {
            Big2GlobalEvent.SubscribeAskPlayerInPostGame(ShowButtons);
        }
      
        private void UnsubscribeEvents()
        {
            Big2GlobalEvent.UnsubscribeAskPlayerInPostGame(ShowButtons);
        }
        #endregion
    }
}
