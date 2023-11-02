using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_DefaultMenu : MenuBase
{
    [SerializeField]
    private Button _userProfile;

    private void Start()
    {
        _userProfile.onClick.AddListener(OnUserProfilePressed);
    }

    private void OnUserProfilePressed() 
    {
        MenuManager.Instance.OpenMenu_ChooseAvatar();
    }
}
