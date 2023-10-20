using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GlobalDefine;

public class Big2CardComparer
{
    public bool CompareHands(List<CardModel> playerHand, List<CardModel> tableHand)
    {
        // Get the hand ranks for both hands
        HandRank playerRank = EvaluateHand(playerHand);
        HandRank tableRank = EvaluateHand(tableHand);

        if (tableRank == HandRank.None) // table is empty, always return true
            return true;

        // Hands have the same rank, compare the cards within the same rank
        switch (playerRank)
        {
            case GlobalDefine.HandRank.HighCard:
            case GlobalDefine.HandRank.Straight:
            case GlobalDefine.HandRank.Flush:
            case GlobalDefine.HandRank.StraightFlush:
            case GlobalDefine.HandRank.RoyalFlush:
                // For these hand ranks, compare the highest cards
                return CompareHighCards(playerHand, tableHand);
            case GlobalDefine.HandRank.Pair:
                // For pair, compare the pairs first
                return ComparePairs(playerHand, tableHand);
            case GlobalDefine.HandRank.ThreeOfAKind:
            case GlobalDefine.HandRank.FourOfAKind:
            case GlobalDefine.HandRank.FullHouse:
                // For three of a kind and four of a kind, compare the set of three/four cards
                return CompareThreeOrFourOfAKind(playerHand, tableHand);
            default:
                return false; // In all other cases, return false
        }
    }


    private HandRank EvaluateHand(List<CardModel> hand)
    {
        var pokerHands = new Big2PokerHands();
        var cardInfo = pokerHands.GetBestHand(hand);

        return cardInfo.HandRank; // The second item in the Tuple is the HandRank
    }

    private bool CompareHighCards(List<CardModel> playerHand, List<CardModel> tableHand)
    {
        // Sort both hands in descending order of card rank and then by suit in ascending order
        playerHand.Sort((a, b) =>
        {
            int rankComparison = ((int)b.CardRank).CompareTo((int)a.CardRank);
            if (rankComparison != 0)
                return rankComparison;
            return ((int)a.CardSuit).CompareTo((int)b.CardSuit);
        });

        tableHand.Sort((a, b) =>
        {
            int rankComparison = ((int)b.CardRank).CompareTo((int)a.CardRank);
            if (rankComparison != 0)
                return rankComparison;
            return ((int)a.CardSuit).CompareTo((int)b.CardSuit);
        });

        for (int i = 0; i < playerHand.Count; i++)
        {
            int rankComparison = playerHand[i].CardRank.CompareTo(tableHand[i].CardRank);

            if (rankComparison > 0)
            {
                return true; // Player's card is higher by rank
            }
            else if (rankComparison < 0)
            {
                return false; // Table's card is higher by rank
            }
            else // If ranks are equal, compare the suits
            {
                int suitComparison = playerHand[i].CardSuit.CompareTo(tableHand[i].CardSuit);
                if (suitComparison > 0)
                {
                    return true; // Player's card is higher by suit
                }
                else if (suitComparison < 0)
                {
                    return false; // Table's card is higher by suit
                }
            }
        }

        return false; // Hands are equal, return false
    }




    private bool ComparePairs(List<CardModel> playerHand, List<CardModel> tableHand)
    {
        // Find the pairs in each hand
        var playerPair = FindPair(playerHand);
        var tablePair = FindPair(tableHand);

        if (playerPair != null && tablePair != null)
        {
            // Compare the ranks of the pairs
            int comparisonResult = playerPair[0].CardRank.CompareTo(tablePair[0].CardRank);

            if (comparisonResult > 0)
            {
                return true; // Player's pair is higher
            }
            else if (comparisonResult < 0)
            {
                return false; // table's pair is higher
            }
            else
            {
                // Pairs have the same rank, compare the remaining high cards
                return CompareHighCards(playerHand, tableHand);
            }
        }
        else if (playerPair != null)
        {
            return true; // Player has a pair, table does not
        }
        else if (tablePair != null)
        {
            return false; // table has a pair, player does not
        }
        else
        {
            // Neither has a pair, compare the high cards
            return CompareHighCards(playerHand, tableHand);
        }
    }

    private bool CompareThreeOrFourOfAKind(List<CardModel> playerHand, List<CardModel> tableHand)
    {
        // Find the three or four of a kind in each hand
        var playerSet = FindThreeOrFourOfAKind(playerHand);
        var tableSet = FindThreeOrFourOfAKind(tableHand);

        if (playerSet != null && tableSet != null)
        {
            // Compare the ranks of the sets
            int comparisonResult = playerSet[0].CardRank.CompareTo(tableSet[0].CardRank);

            if (comparisonResult > 0)
            {
                return true; // Player's set is higher
            }
            else if (comparisonResult < 0)
            {
                return false; // table's set is higher
            }
            else
            {
                // Sets have the same rank, compare the remaining high cards
                return CompareHighCards(playerHand, tableHand);
            }
        }
        else if (playerSet != null)
        {
            return true; // Player has a set, table does not
        }
        else if (tableSet != null)
        {
            return false; // table has a set, player does not
        }
        else
        {
            // Neither has a set, compare the high cards
            return CompareHighCards(playerHand, tableHand);
        }
    }

    private List<CardModel> FindPair(List<CardModel> hand)
    {
        var rankGroups = hand.GroupBy(card => card.CardRank);
        return rankGroups.FirstOrDefault(grp => grp.Count() == 2)?.ToList();
    }

    private List<CardModel> FindThreeOrFourOfAKind(List<CardModel> hand)
    {
        var rankGroups = hand.GroupBy(card => card.CardRank);
        return rankGroups.FirstOrDefault(grp => grp.Count() >= 3)?.ToList();
    }
}
