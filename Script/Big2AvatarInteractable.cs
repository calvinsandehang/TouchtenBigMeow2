using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Import this namespace
using static GlobalDefine;

public class Big2AvatarInteractable : MonoBehaviour, IPointerClickHandler // Implement the IPointerClickHandler interface
{
    [SerializeField]
    private PlayerUserPictureSO userPicture;

    private const string ProfilePictureKey = "ProfilePictureIndex"; // Key used for PlayerPrefs

    public void OnPointerClick(PointerEventData eventData) // This method gets called when a click is detected
    {
        if (userPicture != null)
        {
            // Save the AvatarType as an integer using PlayerPrefs.
            PlayerPrefs.SetInt(ProfilePictureKey, (int)userPicture.AvatarID);
            PlayerPrefs.Save();
            Debug.Log($"Saved AvatarID: {userPicture.AvatarID}");
            Big2CustomEvent.BroadcastOnAvatarIsSet(PlayerType.Human);

            MenuManager.Instance.OpenMenu_Default();
        }
    }
}
