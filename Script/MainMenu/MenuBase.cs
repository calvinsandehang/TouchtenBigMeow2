using UnityEngine;

namespace Big2Meow.MainMenu 
{
    /// <summary>
    /// The base class for all menus in the game. Provides methods to open and close menus.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))] // This ensures a CanvasGroup is attached.
    public abstract class MenuBase : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            // Get the CanvasGroup component to control menu visibility.
            canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Opens the menu by setting alpha, interactable, and blocksRaycasts properties.
        /// </summary>
        public virtual void Open()
        {
            // Set alpha to fully visible.
            canvasGroup.alpha = 1f;
            // Enable user interaction with the menu.
            canvasGroup.interactable = true;
            // Allow the menu to block raycasts, preventing interactions with objects behind it.
            canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Closes the menu by setting alpha, interactable, and blocksRaycasts properties.
        /// </summary>
        public virtual void Close()
        {
            // Set alpha to fully transparent, making the menu invisible.
            canvasGroup.alpha = 0f;
            // Disable user interaction with the menu.
            canvasGroup.interactable = false;
            // Prevent the menu from blocking raycasts, allowing interactions with objects behind it.
            canvasGroup.blocksRaycasts = false;
        }
    }
}

