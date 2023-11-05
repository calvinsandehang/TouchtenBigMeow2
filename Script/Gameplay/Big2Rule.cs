using Big2Meow.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.Gameplay
{
    /// <summary>
    /// Represents the rules checker for the Big2 card game.
    /// </summary>
    public class Big2Rule
    {
        /// <summary>
        /// Checks if there is a rule violation where a player has all four twos.
        /// </summary>
        /// <param name="playerHands">A list of player hands to check for the violation.</param>
        /// <returns>Returns true if the violation is found; otherwise, false.</returns>
        public bool CheckBig2RuleViolation(List<Big2PlayerHand> playerHands)
        {
            foreach (var playerHand in playerHands)
            {
                if (playerHand.CheckHavingQuadrupleTwoCard())
                {
                    Debug.Log($"Rule violation: Player {playerHand.PlayerID} has all four twos.");
                    // Broadcasting the event that a player has all four twos.
                    Big2GlobalEvent.BroadcastHavingQuadrupleTwo();
                    return true; // Exit early upon finding a rule violation.
                }
            }

            // If the loop completes without finding a violation, return false.
            return false;
        }
    }
}




