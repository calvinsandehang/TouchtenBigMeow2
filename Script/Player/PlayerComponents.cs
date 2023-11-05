using Big2Meow.Gameplay;
using Big2Meow.UI;
using System;
using UnityEngine;

namespace Big2Meow.Player
{
    /// <summary>
    /// Represents the components associated with a player.
    /// </summary>
    [Serializable]
    public class PlayerComponents
    {
        /// <summary>
        /// The UI component for skipping notifications.
        /// </summary>
        public UISkipNotification SkipNotification;

        /// <summary>
        /// The manager for the player's profile picture.
        /// </summary>
        public Big2PlayerProfilePictureManager ProfilePicture;

        /// <summary>
        /// The parent GameObject for the player's cards.
        /// </summary>
        public GameObject CardParent;
    }
}

