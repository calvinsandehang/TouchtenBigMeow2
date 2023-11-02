using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public static class Big2CustomEvent
{
    public static event Action<PlayerType> OnAvatarIsSet;
    public static event Action<PlayerType> OnUserProfileIsClick;

    public static void BroadcastOnAvatarIsSet(PlayerType playerType) 
    {
        OnAvatarIsSet?.Invoke(playerType);
    }
}
