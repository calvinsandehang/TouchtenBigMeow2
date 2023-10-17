using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GlobalDefine;

public class Big2PokerHands 
{
    public Tuple<TableState, HandRank, List<CardModel>, int> GetBestHand(List<CardModel> hand)
    {
        var methods = new List<Func<List<CardModel>, Tuple<TableState, HandRank, List<CardModel>, int>>>()
        {
            CheckStraightFlush,
            CheckFourOfAKind,
            CheckFullHouse,
            CheckFlush,
            CheckStraight,
            CheckThreeOfAKind,
            CheckOnePair,
            CheckHighCard
        };

        foreach (var method in methods)
        {
            var result = method.Invoke(hand);
            if (result.Item2 != HandRank.None)
            {
                return result;
            }
        }

        // In case no hand is found, which should never happen if CheckHighCard is correct
        return Tuple.Create(TableState.None, HandRank.None, new List<CardModel>(), 0);
    }

    #region Check Combination
    #region Straight Flush
    // Adjusted CheckStraightFlush method to handle the Big Two rules
    public Tuple<TableState, HandRank, List<CardModel>, int> CheckStraightFlush(List<CardModel> hand)
    {
        var suitedGroups = hand.GroupBy(card => card.CardSuit);
        foreach (var group in suitedGroups)
        {
            var orderedGroup = group.OrderBy(card => card.CardRank).ToList(); // Order by rank; Big Two ranks differently.
            for (int i = 0; i <= orderedGroup.Count - 5; i++)
            {
                var fiveCards = orderedGroup.Skip(i).Take(5).ToList();
                if (IsStraight(fiveCards))
                {
                    int straightFlushPoints = fiveCards.Sum(card => (int)card.CardRank + (int)card.CardSuit);
                    Debug.Log($"Straight Flush: Points: {straightFlushPoints}, Cards: {string.Join(", ", fiveCards)}");
                    return Tuple.Create(TableState.FiveCards, HandRank.StraightFlush, fiveCards, straightFlushPoints);
                }
            }
        }
        return Tuple.Create(TableState.None, HandRank.None, new List<CardModel>(), 0);
    }

    #endregion
    #region Four of a kind
    public Tuple<TableState, HandRank, List<CardModel>, int> CheckFourOfAKind(List<CardModel> hand)
    {
        var rankGroups = hand.GroupBy(card => card.CardRank);
        var fourOfAKind = rankGroups.FirstOrDefault(grp => grp.Count() == 4);
        if (fourOfAKind != null)
        {
            // Calculate the points for this four of a kind.
            int fourPoints = fourOfAKind.Sum(card => (int)card.CardRank); // do not add suit points

            // Add the lowest other card to complete the five card hand.
            var otherCards = hand.Except(fourOfAKind);
            CardModel fifthCard = otherCards.OrderBy(card => card.CardRank).First();

            // Debug log for the combination and points
            var bestHand = fourOfAKind.Concat(new List<CardModel> { fifthCard }).ToList();
            Debug.Log($"Four of a Kind: Points: {fourPoints}, Cards: {string.Join(", ", bestHand)}");

            return Tuple.Create(TableState.FiveCards, HandRank.FourOfAKind, bestHand, fourPoints);
        }

        return Tuple.Create(TableState.None, HandRank.None, new List<CardModel>(), 0);
    }
    #endregion
    #region Full House
    public Tuple<TableState, HandRank, List<CardModel>, int> CheckFullHouse(List<CardModel> hand)
    {
        var groups = hand.GroupBy(card => card.CardRank);
        var threeOfAKinds = groups.Where(grp => grp.Count() >= 3);
        var pairs = groups.Where(grp => grp.Count() == 2); // Only exact pairs should be considered

        List<CardModel> bestFullHouse = null;
        int highestPoints = 0;

        foreach (var three in threeOfAKinds)
        {
            // Calculate the points for this three of a kind, including the suit
            int threePoints = three.Sum(card => (int)card.CardRank + (int)card.CardSuit); // Points from three of a kind including their suits

            foreach (var pair in pairs)
            {
                if (three.Key != pair.Key)
                {
                    // Check if this full house is the highest so far.
                    if (threePoints > highestPoints)
                    {
                        bestFullHouse = three.Concat(pair).ToList();
                        highestPoints = threePoints;
                    }
                }
            }
        }

        if (bestFullHouse != null)
        {
            Debug.Log($"Full House: Points: {highestPoints}, Cards: {string.Join(", ", bestFullHouse)}");
            return Tuple.Create(TableState.FiveCards, HandRank.FullHouse, bestFullHouse, highestPoints);
        }

        return Tuple.Create(TableState.None, HandRank.None, new List<CardModel>(), 0);
    }


    #endregion
    #region Flush
    public Tuple<TableState, HandRank, List<CardModel>, int> CheckFlush(List<CardModel> hand)
    {
        var suitedGroups = hand.GroupBy(card => card.CardSuit);
        var flush = suitedGroups.FirstOrDefault(grp => grp.Count() >= 5);
        if (flush != null)
        {
            var flushCards = flush.OrderByDescending(card => card.CardRank).Take(5).ToList();
            int points = flushCards.Sum(card => (int)card.CardRank + (int)card.CardSuit);
            Debug.Log($"Flush: Points: {points}, Cards: {string.Join(", ", flushCards)}");
            return Tuple.Create(TableState.FiveCards, HandRank.Flush, flushCards, points);
        }
        return Tuple.Create(TableState.None, HandRank.None, new List<CardModel>(), 0);
    }
    #endregion
    #region Straight
    public Tuple<TableState, HandRank, List<CardModel>, int> CheckStraight(List<CardModel> hand)
    {
        var distinctCards = hand.GroupBy(card => card.CardRank).Select(grp => grp.First());
        var orderedCards = distinctCards.OrderBy(card => card.CardRank).ToList(); // Order by ascending, since we've changed the enum values.
        for (int i = 0; i <= orderedCards.Count - 5; i++)
        {
            var fiveCards = orderedCards.Skip(i).Take(5).ToList();
            if (IsStraight(fiveCards))
            {
                int points = fiveCards.Sum(card => (int)card.CardRank + (int)card.CardSuit);
                Debug.Log($"Straight: Points: {points}, Cards: {string.Join(", ", fiveCards)}");
                return Tuple.Create(TableState.FiveCards, HandRank.Straight, fiveCards, points);
            }
        }
        // Special case for the low-end straight (A, 2, 3, 4, 5) which is now (2, 3, 4, 5, 6) because Ace and Two are high cards.
        if (orderedCards.Skip(orderedCards.Count - 5).Take(5).Select(card => card.CardRank).SequenceEqual(new[] { Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six }))
        {
            var lowStraight = orderedCards.Skip(orderedCards.Count - 5).Take(5).ToList();
            int points = lowStraight.Sum(card => (int)card.CardRank + (int)card.CardSuit);
            Debug.Log($"Straight: Points: {points}, Cards: {string.Join(", ", lowStraight)}");
            return Tuple.Create(TableState.FiveCards, HandRank.Straight, lowStraight, points);
        }
        return Tuple.Create(TableState.None, HandRank.None, new List<CardModel>(), 0);
    }
    #endregion
    #region Three of a kind
    public Tuple<TableState, HandRank, List<CardModel>, int> CheckThreeOfAKind(List<CardModel> hand)
    {
        var rankGroups = hand.GroupBy(card => card.CardRank);
        var threeOfAKindGroups = rankGroups.Where(grp => grp.Count() == 3).ToList();

        List<CardModel> bestThreeOfAKind = null;
        int highestPoints = 0;

        foreach (var threeOfAKind in threeOfAKindGroups)
        {
            // Calculate the points for this three of a kind.
            int threePoints = threeOfAKind.Sum(card => (int)card.CardRank + (int)card.CardSuit);

            // Check if this three of a kind has higher points than the current best three of a kind.
            if (threePoints > highestPoints)
            {
                bestThreeOfAKind = threeOfAKind.ToList();
                highestPoints = threePoints;
            }
        }

        if (bestThreeOfAKind != null)
        {
            // Debug log for the combination and points
            Debug.Log($"Three of a Kind: Points: {highestPoints}, Cards: {string.Join(", ", bestThreeOfAKind)}");

            return Tuple.Create(TableState.ThreeOfAKind, HandRank.ThreeOfAKind, bestThreeOfAKind, highestPoints);
        }

        return Tuple.Create(TableState.None, HandRank.None, new List<CardModel>(), 0);
    }
    #endregion
    #region Check One Pair
    public Tuple<TableState, HandRank, List<CardModel>, int> CheckOnePair(List<CardModel> hand)
    {
        var rankGroups = hand.GroupBy(card => card.CardRank);
        var pairs = rankGroups.Where(grp => grp.Count() == 2).ToList();

        List<CardModel> bestPair = null;
        int highestPoints = 0;

        foreach (var pair in pairs)
        {

            int pairPoints = pair.Sum(card => ((int)card.CardRank) + (int)card.CardSuit);

            // Check if this pair has higher points than the current best pair.
            if (pairPoints > highestPoints)
            {
                bestPair = pair.ToList();
                highestPoints = pairPoints;
            }
        }

        if (bestPair != null)
        {
            // Debug log for the combination and points
            Debug.Log($"One Pair: Points: {highestPoints}, Cards: {string.Join(", ", bestPair)}");

            return Tuple.Create(TableState.Pair, HandRank.Pair, bestPair, highestPoints);
        }

        return Tuple.Create(TableState.None, HandRank.None, new List<CardModel>(), 0);
    }
    #endregion
    #region High Card
    public Tuple<TableState, HandRank, List<CardModel>, int> CheckHighCard(List<CardModel> hand)
    {
        if (hand.Count == 0) 
        {
            Debug.Log("The hand is empty, please check if something wrong");
            return Tuple.Create(TableState.None, HandRank.None, new List<CardModel>(), 0);
        }
            
        var highCard = hand.OrderByDescending(card => card.CardRank).First();
        int cardPoints = (int)highCard.CardRank + (int)highCard.CardSuit;
        Debug.Log($"High Card: {highCard.CardRank} of {highCard.CardSuit}, Points: {cardPoints}");
        return Tuple.Create(TableState.Single,HandRank.HighCard, new List<CardModel>() { highCard }, cardPoints);
    }
    #endregion
    #region Helper
    // Adjusted IsStraight method to handle the Big Two rules
    public bool IsStraight(List<CardModel> cards)
    {
        if (cards.Count < 5) return false; // A straight requires at least 5 cards.

        cards = cards.OrderBy(card => card.CardRank).ToList(); // Sorting by rank

        // Check for the special case straight of Ace-2-3-4-5
        if (cards[0].CardRank == Rank.Three && cards[1].CardRank == Rank.Four &&
            cards[2].CardRank == Rank.Five && cards[3].CardRank == Rank.Ace &&
            cards[4].CardRank == Rank.Two)
        {
            return true; // This is a valid straight in Big Two.
        }

        // Check for the special case straight of 2-3-4-5-6
        if (cards[0].CardRank == Rank.Two && cards[1].CardRank == Rank.Three &&
            cards[2].CardRank == Rank.Four && cards[3].CardRank == Rank.Five &&
            cards[4].CardRank == Rank.Six)
        {
            return true; // This is a valid straight in Big Two.
        }

        // Explicitly disallow the sequence J-Q-K-A-2
        if (cards[0].CardRank == Rank.Jack && cards[1].CardRank == Rank.Queen &&
            cards[2].CardRank == Rank.King && cards[3].CardRank == Rank.Ace &&
            cards[4].CardRank == Rank.Two)
        {
            return false; // This sequence is not allowed in Big Two.
        }

        // Check for the standard straight
        for (int i = 0; i < cards.Count - 1; i++)
        {
            if (cards[i + 1].CardRank - cards[i].CardRank != 1)
            {
                return false; // Cards are not in consecutive rank order
            }
        }
        return true; // The hand is a valid straight
    }
    #endregion
    #endregion
}
