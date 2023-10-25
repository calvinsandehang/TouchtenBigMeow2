using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            int savedIndex = PlayerPrefs.GetInt(ProfilePictureKey, 0); // Default to 0 if not found

            currentProfilePicture = _userPictures[savedIndex];
            SetProfilePicture();
        }
        else 
        {
            int rand = Random.Range(0, 4);

            currentProfilePicture = _userPictures[rand];
            SetProfilePicture();
        }
       
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
        playerSM.OnPlayerIsLosing += SetSadProfilePicture;
        playerSM.OnPlayerIsPlaying += SetExcitedProfilePicture;
        playerSM.OnPlayerIsWaiting += SetNormalProfilePicture;
        playerSM.OnPlayerIsWinning += SetHappyProfilePicture;
    }

    public void UnsubscribeEvent()
    {
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
