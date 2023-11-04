using UnityEngine;
using static GlobalDefine;

namespace Big2Meow.UI 
{
    /// <summary>
    /// Manages the UI for the main menu, including the avatar selection menu.
    /// </summary>
    public class UIMainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _chooseAvatarMenu;

        private void Start()
        {
            HideChooseAvatarMenu();
        }

        /// <summary>
        /// Sets the visibility and interactivity of a CanvasGroup.
        /// </summary>
        /// <param name="canvasGroup">The CanvasGroup to set.</param>
        /// <param name="isVisible">Whether the CanvasGroup should be visible and interactable.</param>
        public void SetCanvasGroupVisibility(CanvasGroup canvasGroup, bool isVisible)
        {
            if (isVisible)
            {
                canvasGroup.alpha = 1f;          // Fully visible
                canvasGroup.interactable = true; // Can interact
                canvasGroup.blocksRaycasts = true; // Can receive input events
            }
            else
            {
                canvasGroup.alpha = 0f;          // Fully transparent
                canvasGroup.interactable = false; // Cannot interact
                canvasGroup.blocksRaycasts = false; // Cannot receive input events
            }
        }

        /// <summary>
        /// Hides the choose avatar menu.
        /// </summary>
        public void HideChooseAvatarMenu()
        {
            SetCanvasGroupVisibility(_chooseAvatarMenu, false);
        }
    }
}

