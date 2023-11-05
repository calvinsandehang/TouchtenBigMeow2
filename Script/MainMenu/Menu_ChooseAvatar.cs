using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.MainMenu
{
    /// <summary>
    /// Represents the "Choose Avatar" menu in the game.
    /// </summary>
    public class Menu_ChooseAvatar : MenuBase
    {
        /// <summary>
        /// Called when the menu is initialized.
        /// </summary>
        private void Start()
        {
            // Close the "Choose Avatar" menu by default when it's initialized.
            Close();
        }
    }
}


