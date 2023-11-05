using Big2Meow.MainMenu;
using Big2Meow.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static GlobalDefine;

namespace Big2Meow.UI
{
    /// <summary>
    /// Represents an avatar interactable object that responds to clicks.
    /// </summary>
    public class Big2AvatarInteractable : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private PlayerUserPictureSO userPicture;

        private const string ProfilePictureKey = "ProfilePictureIndex"; // Key used for PlayerPrefs

        /// <summary>
        /// Called when a click event is detected on this avatar interactable.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (userPicture != null)
            {
                // Save the AvatarType as an integer using PlayerPrefs.
                PlayerPrefs.SetInt(ProfilePictureKey, (int)userPicture.AvatarID);
                PlayerPrefs.Save();

                // Log the saved AvatarID for debugging.
                Debug.Log($"Saved AvatarID: {userPicture.AvatarID}");

                // Broadcast an event indicating that an avatar has been set for a human player.
                Big2GlobalEvent.BroadcastAvatarIsSet(PlayerType.Human);

                // Open the default menu.
                MenuManager.Instance.OpenDefaultMenu();
            }
        }
    }
}

