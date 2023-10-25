using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2PlayerUIManager : MonoBehaviour
{
    private Big2PlayerStateMachine playerSM;
    private Big2SimpleAI simpleAI;
    public  UISkipNotification skipNotification;
    public Big2PlayerProfilePictureManager PlayerProfile;

    private void Awake()
    {
        simpleAI = GetComponent<Big2SimpleAI>();
        playerSM = GetComponent<Big2PlayerStateMachine>();
    }
    public void SetupSkipNotificationButton(UISkipNotification skipNotification) 
    {
        this.skipNotification = skipNotification;
        skipNotification.InitializeUIElement(simpleAI);
    }

    public void SetupProfilePicture(Big2PlayerProfilePictureManager playerProfile) 
    {
        this.PlayerProfile = playerProfile;
        playerProfile.InitializePlayerProfile(playerSM);
    }

}
