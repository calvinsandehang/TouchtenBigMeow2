using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public static class Big2CustomEvent
{
    public static event Action<PlayerType> OnAvatarIsSet;

    public static event Action OnPlayerIsLosing;
    public static event Action OnPlayerIsWinning;

    public static void BroadcastOnAvatarIsSet(PlayerType playerType) 
    {
        OnAvatarIsSet?.Invoke(playerType);
    }

    public static void BroadcastOnPlayerIsLosing()
    {
        OnPlayerIsLosing?.Invoke();
    }

    public static void BroadcastOnPlayerIsWinning()
    {
        OnPlayerIsWinning?.Invoke();
    }
}
