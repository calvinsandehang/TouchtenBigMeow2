using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Big2Meow.MainMenu
{
    /// <summary>
    /// Represents the default menu in the game.
    /// </summary>
    public class Menu_DefaultMenu : MenuBase
    {
        [SerializeField]
        private Button _userProfile;

        /// <summary>
        /// Called when the menu is initialized.
        /// </summary>
        private void Start()
        {
            // Attach a listener to the UserProfile button's click event.
            _userProfile.onClick.AddListener(OnUserProfilePressed);
        }

        /// <summary>
        /// Handler for the UserProfile button's click event.
        /// </summary>
        private void OnUserProfilePressed()
        {
            // Open the Choose Avatar menu when the UserProfile button is pressed.
            MenuManager.Instance.OpenChooseAvatarMenu();
        }
    }
}


