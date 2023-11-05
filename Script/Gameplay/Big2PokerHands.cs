using Big2Meow.DeckNCard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GlobalDefine;


namespace Big2Meow.Gameplay
{
    public class Big2PokerHands
    {
        /// <summary>
        /// Determines the best hand from the given list of cards.
        /// </summary>
        /// <param name="hand">The list of card models to evaluate.</param>
        /// <returns>A CardInfo object representing the best hand.</returns>
        public CardInfo GetBestHand(List<CardModel> hand)
        {
            // List of methods to check for each type of hand, ordered by rank.
            var checkMethods = new List<Func<List<CardModel>, CardInfo>>()
            {
                CheckStraightFlush,
                CheckFourOfAKind,
                CheckFullHouse,
                CheckFlush,
                CheckStraight,
                CheckThreeOfAKind,
                CheckHighestOnePair,
                CheckHighCardForHighestValue
            };

            foreach (var checkMethod in checkMethods)
            {
                var result = checkMethod(hand);
                if (result.HandRank != HandRank.None)
                {
                    return result;
                }
            }

            // This should theoretically never be reached if CheckHighCardForHighestValue is implemented correctly.
            return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        }

        /// <summary>
        /// Determines the lowest hand from the given list of cards.
        /// </summary>
        /// <param name="hand">The list of card models to evaluate.</param>
        /// <returns>A CardInfo object representing the lowest hand.</returns>
        public CardInfo GetLowestHand(List<CardModel> hand)
        {
            // List of methods to check for each type of hand, ordered by reverse rank.
            var checkMethods = new List<Func<List<CardModel>, CardInfo>>()
            {
                CheckHighCardForLowestValue,
                CheckLowestPair,
                CheckThreeOfAKind,
                CheckStraight,
                CheckFlush,
                CheckFullHouse,
                CheckFourOfAKind,
                CheckStraightFlush
                };

            foreach (var checkMethod in checkMethods)
            {
                var result = checkMethod(hand);
                if (result.HandRank != HandRank.None)
                {
                    return result;
                }
            }

            // This should theoretically never be reached if CheckHighCardForLowestValue is implemented correctly.
            return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        }

        #region Check Combination
        #region Straight Flush
        /// <summary>
        /// Checks for the highest straight flush within a hand following Big Two rules.
        /// </summary>
        /// <param name="hand">The player's hand to evaluate.</param>
        /// <returns>A CardInfo object representing the straight flush found, or none if not present.</returns>
        public CardInfo CheckStraightFlush(List<CardModel> hand)
        {
            var suitedGroups = hand.GroupBy(card => card.CardSuit);
            List<CardModel> bestStraightFlush = null;
            int highestPoints = 0;

            foreach (var group in suitedGroups)
            {
                var orderedGroup = group.OrderBy(card => card.CardRank).ToList();
                for (int i = 0; i <= orderedGroup.Count - 5; i++)
                {
                    var fiveCards = orderedGroup.Skip(i).Take(5).ToList();
                    if (IsStraight(fiveCards))
                    {
                        int straightFlushPoints = fiveCards.Sum(card => (int)card.CardRank + (int)card.CardSuit);
                        if (straightFlushPoints > highestPoints)
                        {
                            highestPoints = straightFlushPoints;
                            bestStraightFlush = fiveCards;
                        }
                    }
                }
            }

            if (bestStraightFlush != null)
            {
                // Reordering the found straight flush for proper ranking
                bestStraightFlush = bestStraightFlush.OrderByDescending(card => card.CardRank)
                                                     .ThenByDescending(card => card.CardSuit)
                                                     .ToList();
                return new CardInfo(HandType.FiveCards, HandRank.StraightFlush, bestStraightFlush);
            }

            return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        }

        #endregion
        #region Four of a kind
        /// <summary>
        /// Checks for the highest four of a kind in a hand.
        /// </summary>
        /// <param name="hand">The player's hand to evaluate.</param>
        /// <returns>A CardInfo object representing the four of a kind found, or none if not present.</returns>
        public CardInfo CheckFourOfAKind(List<CardModel> hand)
        {
            var rankGroups = hand.GroupBy(card => card.CardRank);
            var fourOfAKindGroups = rankGroups.Where(grp => grp.Count() == 4).ToList();

            if (fourOfAKindGroups.Any())
            {
                var highestFourOfAKind = fourOfAKindGroups.OrderByDescending(group => group.Key)
                                                           .ThenByDescending(group => group.First().CardSuit)
                                                           .First();

                var otherCards = hand.Except(highestFourOfAKind).OrderByDescending(card => card.CardRank);
                CardModel fifthCard = otherCards.First();

                var bestHand = highestFourOfAKind.Concat(new[] { fifthCard })
                                                 .OrderByDescending(card => card.CardRank)
                                                 .ThenByDescending(card => card.CardSuit)
                                                 .ToList();

                return new CardInfo(HandType.FiveCards, HandRank.FourOfAKind, bestHand);
            }

            return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        }

        #endregion
        #region Full House
        /// <summary>
        /// Checks for the best full house combination within a hand.
        /// </summary>
        /// <param name="hand">The player's hand to evaluate.</param>
        /// <returns>A CardInfo object representing the full house found, or none if not present.</returns>
        public CardInfo CheckFullHouse(List<CardModel> hand)
        {
            var groups = hand.GroupBy(card => card.CardRank);
            var threeOfAKinds = groups.Where(grp => grp.Count() >= 3);
            var pairs = groups.Where(grp => grp.Count() == 2);

            List<CardModel> bestFullHouse = null;
            int highestPoints = 0;

            foreach (var three in threeOfAKinds)
            {
                int threePoints = three.Sum(card => (int)card.CardRank + (int)card.CardSuit);
                var sortedThree = three.OrderByDescending(card => card.CardSuit).ToList();

                foreach (var pair in pairs)
                {
                    if (three.Key != pair.Key)
                    {
                        int pairPoints = pair.Sum(card => (int)card.CardRank + (int)card.CardSuit);
                        int totalPoints = threePoints + pairPoints;

                        if (totalPoints > highestPoints || bestFullHouse == null)
                        {
                            bestFullHouse = sortedThree.Concat(pair.OrderByDescending(card => card.CardSuit)).ToList();
                            highestPoints = totalPoints;
                        }
                        else if (totalPoints == highestPoints && pair.Key < bestFullHouse[3].CardRank)
                        {
                            bestFullHouse = sortedThree.Concat(pair.OrderByDescending(card => card.CardSuit)).ToList();
                        }
                    }
                }
            }

            if (bestFullHouse != null)
            {
                return new CardInfo(HandType.FiveCards, HandRank.FullHouse, bestFullHouse);
            }

            return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        }
        #endregion
        #region Flush
        /// <summary>
        /// Checks for the highest flush within a hand.
        /// </summary>
        /// <param name="hand">The player's hand to evaluate.</param>
        /// <returns>A CardInfo object representing the flush found, or none if not present.</returns>
        public CardInfo CheckFlush(List<CardModel> hand)
        {
            var suitedGroups = hand.GroupBy(card => card.CardSuit);
            List<CardModel> bestFlushCards = null;
            int highestPoints = 0;

            foreach (var group in suitedGroups.Where(group => group.Count() >= 5))
            {
                var flushCards = group.OrderByDescending(card => card.CardRank).Take(5).ToList();
                int points = flushCards.Sum(card => (int)card.CardRank + (int)card.CardSuit);

                if (points > highestPoints)
                {
                    highestPoints = points;
                    bestFlushCards = flushCards;
                }
            }

            if (bestFlushCards != null)
            {
                return new CardInfo(HandType.FiveCards, HandRank.Flush, bestFlushCards);
            }

            return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        }
        #endregion
        #region Straight
        public CardInfo CheckStraight(List<CardModel> hand)
        {
            var distinctCards = hand.GroupBy(card => card.CardRank).Select(grp => grp.First());
            var orderedCards = distinctCards
                .OrderBy(card => card.CardRank)
                .ThenByDescending(card => card.CardSuit) // Consider the highest suit when ranks are the same
                .ToList();

            List<CardModel> bestStraight = null;
            int highestPoints = 0;

            for (int i = 0; i <= orderedCards.Count - 5; i++)
            {
                var fiveCards = orderedCards.Skip(i).Take(5).ToList();
                if (IsStraight(fiveCards))
                {
                    int points = fiveCards.Sum(card => (int)card.CardRank + (int)card.CardSuit);

                    // Check if this straight has higher points than the current best straight
                    if (points > highestPoints)
                    {
                        highestPoints = points;
                        bestStraight = fiveCards;
                    }
                    else if (points == highestPoints)
                    {
                        // Compare suits and choose the one with the higher suit
                        var currentHighestSuit = bestStraight.Max(card => card.CardSuit);
                        var newStraightHighestSuit = fiveCards.Max(card => card.CardSuit);
                        if (newStraightHighestSuit > currentHighestSuit)
                        {
                            bestStraight = fiveCards;
                        }
                    }
                }
            }

            // Special case for the low-end straight (A, 2, 3, 4, 5) which is now (2, 3, 4, 5, 6) because Ace and Two are high cards.
            if (orderedCards.Skip(orderedCards.Count - 5).Take(5).Select(card => card.CardRank).SequenceEqual(new[] { Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six }))
            {
                var lowStraight = orderedCards.Skip(orderedCards.Count - 5).Take(5).ToList();
                int points = lowStraight.Sum(card => (int)card.CardRank + (int)card.CardSuit);

                // Check if this low straight has higher points than the current best straight
                if (points > highestPoints)
                {
                    bestStraight = lowStraight;
                }
                else if (points == highestPoints)
                {
                    // Compare suits and choose the one with the higher suit
                    var currentHighestSuit = bestStraight.Max(card => card.CardSuit);
                    var newStraightHighestSuit = lowStraight.Max(card => card.CardSuit);
                    if (newStraightHighestSuit > currentHighestSuit)
                    {
                        bestStraight = lowStraight;
                    }
                }
            }

            if (bestStraight != null)
            {
                // Sort in descending order based on CardRank and Suit
                bestStraight = bestStraight.OrderByDescending(card => card.CardRank).ThenByDescending(card => card.CardSuit).ToList();

                //Debug.Log($"Straight: Points: {highestPoints}, Cards: {string.Join(", ", bestStraight)}");

                return new CardInfo(HandType.FiveCards, HandRank.Straight, bestStraight);
            }

            return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        }

        #endregion
        #region Three of a kind
        public CardInfo CheckThreeOfAKind(List<CardModel> hand)
        {
            var rankGroups = hand.GroupBy(card => card.CardRank);
            var threeOfAKindGroups = rankGroups.Where(grp => grp.Count() == 3).ToList();

            List<CardModel> bestThreeOfAKind = null;
            int highestPoints = 0;

            foreach (var threeOfAKind in threeOfAKindGroups)
            {
                // Calculate the points for this three of a kind.
                int threePoints = threeOfAKind.Sum(card => (int)card.CardRank); // Only sum the ranks for comparison.

                // Check if this three of a kind has higher points than the current best three of a kind.
                if (threePoints > highestPoints || bestThreeOfAKind == null)
                {
                    bestThreeOfAKind = threeOfAKind.ToList();
                    highestPoints = threePoints;
                }
            }

            if (bestThreeOfAKind != null)
            {
                // Sort the bestThreeOfAKind list by suit
                bestThreeOfAKind = bestThreeOfAKind.OrderByDescending(card => card.CardSuit).ToList();

                // Debug log for the combination and points
                //Debug.Log($"Three of a Kind: Points: {highestPoints}, Cards: {string.Join(", ", bestThreeOfAKind)}");

                return new CardInfo(HandType.ThreeOfAKind, HandRank.ThreeOfAKind, bestThreeOfAKind);
            }

            return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        }



        #endregion
        #region Check One Pair
        public CardInfo CheckHighestOnePair(List<CardModel> hand)
        {
            var rankGroups = hand.GroupBy(card => card.CardRank);

            // Debug the rank groups
            foreach (var group in rankGroups)
            {
                //Debug.Log($"Group Key: {group.Key}, Count: {group.Count()}, Elements: {string.Join(", ", group)}");
            }

            var pairs = rankGroups.Where(grp => grp.Count() == 2).ToList();

            // Debug the pairs
            foreach (var pair in pairs)
            {
                //Debug.Log($"Pair Group: {pair.Key}, Count: {pair.Count()}, Elements: {string.Join(", ", pair)}");
            }

            List<CardModel> bestPair = null;
            int highestPoints = 0;

            foreach (var pair in pairs)
            {
                var sortedPair = pair.OrderByDescending(card => (int)card.CardRank)
                                    .ThenByDescending(card => card.CardSuit)
                                    .ToList();

                int pairPoints = sortedPair.Sum(card => ((int)card.CardRank));

                if (pairPoints > highestPoints)
                {
                    bestPair = sortedPair;
                    highestPoints = pairPoints;
                }
            }

            if (bestPair != null)
            {
                // Ensure that the bestPair list is sorted with the highest suit at [0]
                bestPair = bestPair.OrderByDescending(card => card.CardSuit).ToList();

                //Debug.Log($"One Pair: Points: {highestPoints}, Cards: {string.Join(", ", bestPair)}");
                return new CardInfo(HandType.Pair, HandRank.Pair, bestPair);
            }

            return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        }

        public CardInfo CheckLowestPair(List<CardModel> hand)
        {
            var rankGroups = hand.GroupBy(card => card.CardRank);

            // Debug the rank groups
            foreach (var group in rankGroups)
            {
                // Debug.Log($"Group Key: {group.Key}, Count: {group.Count()}, Elements: {string.Join(", ", group)}");
            }

            var pairs = rankGroups.Where(grp => grp.Count() == 2).ToList();

            // Debug the pairs
            foreach (var pair in pairs)
            {
                // Debug.Log($"Pair Group: {pair.Key}, Count: {pair.Count()}, Elements: {string.Join(", ", pair)}");
            }

            List<CardModel> lowestPair = null;
            int lowestPoints = int.MaxValue;

            foreach (var pair in pairs)
            {
                var sortedPair = pair.OrderBy(card => (int)card.CardRank)
                                    .ThenBy(card => card.CardSuit)
                                    .ToList();

                int pairPoints = sortedPair.Sum(card => ((int)card.CardRank));

                if (pairPoints < lowestPoints)
                {
                    lowestPair = sortedPair;
                    lowestPoints = pairPoints;
                }
            }

            if (lowestPair != null)
            {
                // Ensure that the lowestPair list is sorted with the lowest suit at [0]
                lowestPair = lowestPair.OrderBy(card => card.CardSuit).ToList();

                //Debug.Log($"Low Pair: Points: {lowestPoints}, Cards: {string.Join(", ", lowestPair)}");
                return new CardInfo(HandType.Pair, HandRank.Pair, lowestPair);
            }

            return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        }



        #endregion
        #region High Card
        public CardInfo CheckHighCardForHighestValue(List<CardModel> hand)
        {
            if (hand.Count == 0)
            {
                //Debug.Log("The hand is empty, please check if something wrong");
                return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
            }

            var highCard = hand.OrderByDescending(card => card.CardRank).First();
            int cardPoints = (int)highCard.CardRank + (int)highCard.CardSuit;
            //Debug.Log($"High Card: {highCard.CardRank} of {highCard.CardSuit}, Points: {cardPoints}");
            return new CardInfo(HandType.Single, HandRank.HighCard, new List<CardModel>() { highCard });
        }

        public CardInfo CheckHighCardForLowestValue(List<CardModel> hand)
        {
            if (hand.Count == 0)
            {
                //Debug.Log("The hand is empty, please check if something is wrong.");
                return new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
            }

            var lowCard = hand.OrderBy(card => card.CardRank).First();
            int cardPoints = (int)lowCard.CardRank + (int)lowCard.CardSuit;
            //Debug.Log($"Low Card: {lowCard.CardRank} of {lowCard.CardSuit}, Points: {cardPoints}");
            return new CardInfo(HandType.Single, HandRank.HighCard, new List<CardModel>() { lowCard });
        }

        #endregion
        #region Check Specific
        public CardInfo GetThreeOfDiamonds(List<CardModel> hand)
        {
            List<CardModel> threeOfDiamondsList = new List<CardModel>();

            foreach (var card in hand)
            {
                if (card.CardRank == GlobalDefine.Rank.Three && card.CardSuit == GlobalDefine.Suit.Diamonds)
                {
                    threeOfDiamondsList.Add(card);
                }
            }

            if (threeOfDiamondsList.Count > 0)
            {
                return new CardInfo(GlobalDefine.HandType.Single, GlobalDefine.HandRank.HighCard, threeOfDiamondsList);
            }
            else
            {
                return new CardInfo(GlobalDefine.HandType.None, GlobalDefine.HandRank.None, new List<CardModel>());
            }
        }
        public bool HasAllFourTwos(List<CardModel> hand)
        {
            // Flags to determine if the two of each suit is present
            bool hasTwoOfHearts = false;
            bool hasTwoOfClubs = false;
            bool hasTwoOfDiamonds = false;
            bool hasTwoOfSpades = false;

            // Iterate through the hand and set flags if the two of each suit is found
            foreach (var card in hand)
            {
                if (card.CardRank == GlobalDefine.Rank.Two)
                {
                    switch (card.CardSuit)
                    {
                        case GlobalDefine.Suit.Hearts:
                            hasTwoOfHearts = true;
                            break;
                        case GlobalDefine.Suit.Clubs:
                            hasTwoOfClubs = true;
                            break;
                        case GlobalDefine.Suit.Diamonds:
                            hasTwoOfDiamonds = true;
                            break;
                        case GlobalDefine.Suit.Spades:
                            hasTwoOfSpades = true;
                            break;
                    }
                }

                // If all twos are found, no need to continue checking
                if (hasTwoOfHearts && hasTwoOfClubs && hasTwoOfDiamonds && hasTwoOfSpades)
                {
                    return true;
                }
            }

            // Return true only if all four twos are present
            return hasTwoOfHearts && hasTwoOfClubs && hasTwoOfDiamonds && hasTwoOfSpades;
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
}

