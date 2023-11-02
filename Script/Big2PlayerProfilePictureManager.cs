using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GlobalDefine;

public class Big2PlayerProfilePictureManager : MonoBehaviour, ISubscriber
{
    [SerializeField]
    private List<PlayerUserPictureSO> _userPictures;
    [SerializeField]
    private Image _profilePictureHolder;

    private const string ProfilePictureKey = "ProfilePictureIndex"; // Key used for PlayerPrefs

    private PlayerUserPictureSO currentProfilePicture;
    private Big2PlayerStateMachine playerSM;
    private PlayerType playerType;

    private void Awake()
    {
        InitializePlayerProfile();
    }

    public void InitializePlayerProfile()
    {
        LoadProfilePicture();
        SubscribeEvent();
    }

    public void InitializePlayerProfile(Big2PlayerStateMachine playerSM)
    {
        this.playerSM = playerSM;
        PlayerType playerType = this.playerSM.GetPlayerType();
        LoadProfilePicture(playerType);
        SubscribeEvent();
    }

    private void LoadProfilePicture(PlayerType playerType)
    {
        if (playerType == PlayerType.Human)
        {
            Debug.Log("Load Profile Picture");
            AvatarType savedAvatar = (AvatarType)PlayerPrefs.GetInt(ProfilePictureKey, 0); // Default to the first avatar if not found
            int savedAvatarID = PlayerPrefs.GetInt(ProfilePictureKey);
            Debug.Log($"Retrieved saved AvatarID from PlayerPrefs: {savedAvatarID}");

            currentProfilePicture = FindUserPictureByAvatarType(savedAvatar);
            SetProfilePicture();
        }
        else
        {
            AvatarType randomAvatar = (AvatarType)Random.Range(0, System.Enum.GetValues(typeof(AvatarType)).Length);

            currentProfilePicture = FindUserPictureByAvatarType(randomAvatar);
            SetProfilePicture();
        }
    }

    private void LoadProfilePicture()
    {
        Debug.Log("Load Profile Picture");
        AvatarType savedAvatar = (AvatarType)PlayerPrefs.GetInt(ProfilePictureKey, 0); // Default to the first avatar if not found

        currentProfilePicture = FindUserPictureByAvatarType(savedAvatar);
        SetProfilePicture();
    }

    private PlayerUserPictureSO FindUserPictureByAvatarType(AvatarType avatarType)
    {
        foreach (var userPicture in _userPictures)
        {
            if (userPicture.AvatarID == avatarType)
            {
                return userPicture;
            }
        }

        // Return null or a default picture if not found
        return null;
    }

    private void SetProfilePicture() 
    {
        _profilePictureHolder.sprite = currentProfilePicture.Normal;
    }

    private void SetNormalProfilePicture()
    {
        _profilePictureHolder.sprite = currentProfilePicture.Normal;
    }

    private void SetSadProfilePicture()
    {
        int rand = Random.Range(0, 2); // Generates either 0 or 1

        if (rand == 0)
        {
            _profilePictureHolder.sprite = currentProfilePicture.Sad;
        }
        else
        {
            _profilePictureHolder.sprite = currentProfilePicture.Angry;
        }
    }

    private void SetHappyProfilePicture()
    {
        _profilePictureHolder.sprite = currentProfilePicture.Happy;
    }   

    private void SetExcitedProfilePicture()
    {
        _profilePictureHolder.sprite = currentProfilePicture.Excited;
    }

    public void SubscribeEvent()
    {
        Big2CustomEvent.OnAvatarIsSet += LoadProfilePicture;

        if (playerSM != null)
        {
            playerSM.OnPlayerIsLosing += SetSadProfilePicture;
            playerSM.OnPlayerIsPlaying += SetExcitedProfilePicture;
            playerSM.OnPlayerIsWaiting += SetNormalProfilePicture;
            playerSM.OnPlayerIsWinning += SetHappyProfilePicture;
        }
           
    }

    public void UnsubscribeEvent()
    {
        Big2CustomEvent.OnAvatarIsSet -= LoadProfilePicture;

        if (playerSM != null)
        {
            playerSM.OnPlayerIsLosing -= SetSadProfilePicture;
            playerSM.OnPlayerIsPlaying -= SetExcitedProfilePicture;
            playerSM.OnPlayerIsWaiting -= SetNormalProfilePicture;
            playerSM.OnPlayerIsWinning -= SetHappyProfilePicture;
        }
        
    }

    private void OnDisable()
    {
       UnsubscribeEvent();
    }

   
}
