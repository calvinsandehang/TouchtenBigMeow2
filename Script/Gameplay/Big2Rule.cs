using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2Rule
{
    public bool CheckBig2RuleViolation(List<Big2PlayerHand> playerHands)
    {
        foreach (var player in playerHands)
        {
            if (player.CheckHavingQuadrupleTwoCard())
            {
                // Using string interpolation for cleaner and more readable code.
                Debug.Log($"Rule violation: Player {player.PlayerID} has all four twos.");
                Big2GlobalEvent.BroadcastHavingQuadrupleTwo();
                return true;
            }
        }
        return false;
    }
}
