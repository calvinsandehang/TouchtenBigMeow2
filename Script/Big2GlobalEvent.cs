using System;
using System.Collections.Generic;
using static GlobalDefine;

public static class Big2GlobalEvent
{
    private static List<Action<PlayerType>> avatarIsSetListeners = new List<Action<PlayerType>>();
    private static List<Action> playerCardLessThanSixListeners = new List<Action>();
    private static List<Action<Big2PlayerHand>> playerLastCardDroppedListeners = new List<Action<Big2PlayerHand>>();
    private static List<Action<Big2PlayerHand>> playerFinishTurnGlobalListeners = new List<Action<Big2PlayerHand>>();
    private static List<Action> cardSubmissionAllowedListeners = new List<Action>();
    private static List<Action> cardSubmissionNotAllowedListeners = new List<Action>();
    private static List<Action<Big2PlayerHand>> playerSkipTurnGlobalListeners = new List<Action<Big2PlayerHand>>();
    private static List<Action<Big2PlayerHand>> aiFinishTurnGlobalListeners = new List<Action<Big2PlayerHand>>();
    private static List<Action<Big2PlayerHand>> aiSkipTurnGlobalListeners = new List<Action<Big2PlayerHand>>();

    #region Event : AvatarIsSet
    public static void SubscribeAvatarIsSet(Action<PlayerType> listener)
    {
        if (!avatarIsSetListeners.Contains(listener))
        {
            avatarIsSetListeners.Add(listener);
        }
    }

    public static void UnsubscribeAvatarIsSet(Action<PlayerType> listener)
    {
        if (avatarIsSetListeners.Contains(listener))
        {
            avatarIsSetListeners.Remove(listener);
        }
    }

    public static void BroadcastAvatarIsSet(PlayerType playerType)
    {
        foreach (var listener in avatarIsSetListeners)
        {
            listener.Invoke(playerType);
        }
    }
    #endregion
    #region Event : CardLessThanSix
    public static void SubscribePlayerCardLessThanSix(Action listener)
    {
        if (!playerCardLessThanSixListeners.Contains(listener))
        {
            playerCardLessThanSixListeners.Add(listener);
        }
    }

    public static void UnsubscribePlayerCardLessThanSix(Action listener)
    {
        if (playerCardLessThanSixListeners.Contains(listener))
        {
            playerCardLessThanSixListeners.Remove(listener);
        }
    }

    public static void BroadcastPlayerCardLessThanSix()
    {
        foreach (var listener in playerCardLessThanSixListeners)
        {
            listener.Invoke();
        }
    }
    #endregion
    #region Event : PlayerDropLastCard
    public static void SubscribePlayerDropLastCard(Action<Big2PlayerHand> listener)
    {
        if (!playerLastCardDroppedListeners.Contains(listener))
        {
            playerLastCardDroppedListeners.Add(listener);
        }
    }

    public static void UnsubscribePlayerDropLastCard(Action<Big2PlayerHand> listener)
    {
        if (playerLastCardDroppedListeners.Contains(listener))
        {
            playerLastCardDroppedListeners.Remove(listener);
        }
    }

    public static void BroadcastPlayerDropLastCard(Big2PlayerHand player)
    {
        foreach (var listener in playerLastCardDroppedListeners)
        {
            listener.Invoke(player);
        }
    }
    #endregion
    #region Event : PlayerFinishTurn
    public static void SubscribePlayerFinishTurnGlobal(Action<Big2PlayerHand> listener)
    {
        if (!playerFinishTurnGlobalListeners.Contains(listener))
        {
            playerFinishTurnGlobalListeners.Add(listener);
        }
    }

    public static void UnsubscribePlayerFinishTurnGlobal(Action<Big2PlayerHand> listener)
    {
        if (playerFinishTurnGlobalListeners.Contains(listener))
        {
            playerFinishTurnGlobalListeners.Remove(listener);
        }
    }

    public static void BroadcastPlayerFinishTurnGlobal(Big2PlayerHand player)
    {
        foreach (var listener in playerFinishTurnGlobalListeners)
        {
            listener.Invoke(player);
        }
    }
    #endregion
    #region Event : CardSubmissionAllowed
    public static void SubscribeCardSubmissionAllowed(Action listener)
    {
        if (!cardSubmissionAllowedListeners.Contains(listener))
        {
            cardSubmissionAllowedListeners.Add(listener);
        }
    }

    public static void UnsubscribeCardSubmissionAllowed(Action listener)
    {
        if (cardSubmissionAllowedListeners.Contains(listener))
        {
            cardSubmissionAllowedListeners.Remove(listener);
        }
    }

    public static void BroadcastCardSubmissionAllowed()
    {
        foreach (var listener in cardSubmissionAllowedListeners)
        {
            listener.Invoke();
        }
    }
    #endregion
    #region Event : CardSubmissionNotAllowed
    public static void SubscribeCardSubmissionNotAllowed(Action listener)
    {
        if (!cardSubmissionNotAllowedListeners.Contains(listener))
        {
            cardSubmissionNotAllowedListeners.Add(listener);
        }
    }

    public static void UnsubscribeCardSubmissionNotAllowed(Action listener)
    {
        if (cardSubmissionNotAllowedListeners.Contains(listener))
        {
            cardSubmissionNotAllowedListeners.Remove(listener);
        }
    }

    public static void BroadcastCardSubmissionNotAllowed()
    {
        foreach (var listener in cardSubmissionNotAllowedListeners)
        {
            listener.Invoke();
        }
    }
    #endregion
    #region Event : PlayerSkipTurnGlobal
    public static void SubscribePlayerSkipTurnGlobal(Action<Big2PlayerHand> listener)
    {
        if (!playerSkipTurnGlobalListeners.Contains(listener))
        {
            playerSkipTurnGlobalListeners.Add(listener);
        }
    }

    public static void UnsubscribePlayerSkipTurnGlobal(Action<Big2PlayerHand> listener)
    {
        if (playerSkipTurnGlobalListeners.Contains(listener))
        {
            playerSkipTurnGlobalListeners.Remove(listener);
        }
    }

    public static void BroadcastPlayerSkipTurnGlobal(Big2PlayerHand player)
    {
        foreach (var listener in playerSkipTurnGlobalListeners)
        {
            listener.Invoke(player);
        }
    }
    #endregion
    #region Event : AIFinishTurnGlobal
    public static void SubscribeAIFinishTurnGlobal(Action<Big2PlayerHand> listener)
    {
        if (!aiFinishTurnGlobalListeners.Contains(listener))
        {
            aiFinishTurnGlobalListeners.Add(listener);
        }
    }

    public static void UnsubscribeAIFinishTurnGlobal(Action<Big2PlayerHand> listener)
    {
        if (aiFinishTurnGlobalListeners.Contains(listener))
        {
            aiFinishTurnGlobalListeners.Remove(listener);
        }
    }

    public static void BroadcastAIFinishTurnGlobal(Big2PlayerHand player)
    {
        foreach (var listener in aiFinishTurnGlobalListeners)
        {
            listener.Invoke(player);
        }
    }
    #endregion
    #region Event : AISkipTurnGlobal
    public static void SubscribeAISkipTurnGlobal(Action<Big2PlayerHand> listener)
    {
        if (!aiSkipTurnGlobalListeners.Contains(listener))
        {
            aiSkipTurnGlobalListeners.Add(listener);
        }
    }

    public static void UnsubscribeAISkipTurnGlobal(Action<Big2PlayerHand> listener)
    {
        if (aiSkipTurnGlobalListeners.Contains(listener))
        {
            aiSkipTurnGlobalListeners.Remove(listener);
        }
    }

    public static void BroadcastAISkipTurnGlobal(Big2PlayerHand player)
    {
        foreach (var listener in aiSkipTurnGlobalListeners)
        {
            listener.Invoke(player);
        }
    }
    #endregion



}
