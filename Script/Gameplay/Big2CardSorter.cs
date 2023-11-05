using Big2Meow.DeckNCard;
using Big2Meow.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static GlobalDefine;


namespace Big2Meow.Gameplay
{
    /// <summary>
    /// Class responsible for sorting and managing player hands in a Big2 card game.
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    public class Big2CardSorter : MonoBehaviour
    {
        private Big2PokerHands pokerHandsChecker;
        private List<CardModel> sortedCardModels = new List<CardModel>();
        private List<CardModel> cardModelsInPlayerHand = new List<CardModel>();


        private void Awake()
        {
            pokerHandsChecker = new Big2PokerHands();
            aiCardInfo = new AiCardInfo();
            cardPackage = new CardPackage();
        }

        #region Sort Human Card
        /// <summary>
        /// Sorts player hand cards by rank.
        /// </summary>
        /// <param name="cardsObjectsInPlayerHand">List of card GameObjects in the player's hand.</param>
        /// <param name="playerType">The type of player (Human or AI).</param>
        public void SortPlayerHandByRank(List<GameObject> cardsObjectsInPlayerHand, PlayerType playerType)
        {
            cardsObjectsInPlayerHand.Sort((card1, card2) =>
            {
                UISelectableCard selectableCard1 = card1.GetComponent<UISelectableCard>();
                UISelectableCard selectableCard2 = card2.GetComponent<UISelectableCard>();

                CardModel cardModel1 = selectableCard1.GetCardModel();
                CardModel cardModel2 = selectableCard2.GetCardModel();

                return cardModel1.CardRank.CompareTo(cardModel2.CardRank);
            });

            UpdateCardPositions(cardsObjectsInPlayerHand);
        }

        /// <summary>
        /// Sorts player hand cards by suit.
        /// </summary>
        /// <param name="cardsObjectsInPlayerHand">List of card GameObjects in the player's hand.</param>
        /// <param name="playerType">The type of player (Human or AI).</param>
        public void SortPlayerHandBySuit(List<GameObject> cardsObjectsInPlayerHand, PlayerType playerType)
        {
            cardsObjectsInPlayerHand.Sort((card1, card2) =>
            {
                UISelectableCard selectableCard1 = card1.GetComponent<UISelectableCard>();
                UISelectableCard selectableCard2 = card2.GetComponent<UISelectableCard>();

                CardModel cardModel1 = selectableCard1.GetCardModel();
                CardModel cardModel2 = selectableCard2.GetCardModel();

                return cardModel1.CardSuit.CompareTo(cardModel2.CardSuit);
            });

            UpdateCardPositions(cardsObjectsInPlayerHand);
        }


        public void SortPlayerHandByBestHand(List<GameObject> cardsObjectsInPlayerHand,
        CardPool cardPool, Transform cardParent, PlayerType playerType)
        {
            // Clear any previous state
            cardModelsInPlayerHand.Clear();
            sortedCardModels.Clear();

            Debug.Log("Initial count of card objects in player's hand: " + cardsObjectsInPlayerHand.Count);

            // Convert GameObjects to CardModels
            foreach (var cardObject in cardsObjectsInPlayerHand)
            {
                UISelectableCard selectableCard = cardObject.GetComponent<UISelectableCard>();
                if (selectableCard != null)
                {
                    CardModel cardModel = selectableCard.GetCardModel();
                    cardModelsInPlayerHand.Add(cardModel);
                }
            }

            // Use a new list to prevent modifying the list while iterating over it
            tempCardModels = new List<CardModel>(cardModelsInPlayerHand);

            // Loop to process the hand until all cards have been sorted
            while (tempCardModels.Count > 0)
            {
                CardInfo cardInfo = pokerHandsChecker.GetBestHand(tempCardModels);

                // Safety check in case GetBestHand fails or returns an empty composition
                if (cardInfo == null || cardInfo.CardComposition == null || cardInfo.CardComposition.Count == 0)
                {
                    Debug.LogWarning("GetBestHand returned a null or empty hand. Exiting the sort loop.");
                    break; // Exit the loop to prevent hanging
                }

                List<CardModel> bestHandCards = cardInfo.CardComposition;
                Debug.Log("Best hand card count this iteration: " + bestHandCards.Count);

                sortedCardModels.AddRange(bestHandCards);

                foreach (var card in bestHandCards)
                {
                    tempCardModels.Remove(card);
                }

                // Additional safety check in case no cards were removed
                if (bestHandCards.Count == 0)
                {
                    Debug.LogWarning("No cards were removed from the hand. Exiting the sort loop.");
                    break;
                }
            }

            Debug.Log("Count of sorted card models: " + sortedCardModels.Count);

            // Ensure cardModelsInPlayerHand is a distinct list, not just a reference
            cardModelsInPlayerHand = new List<CardModel>(sortedCardModels);
            cardModelsInPlayerHand.Reverse(); // Reverse if needed

            Debug.Log("Count of card models in player's hand after sorting: " + cardModelsInPlayerHand.Count);

            // Update UI positions
            UpdateCardPosition(cardsObjectsInPlayerHand, cardModelsInPlayerHand, cardPool, cardParent, playerType);
        }
        #endregion

        #region Sort AI Card
        private List<CardModel> singleCardList = new List<CardModel>();
        private CardPackage cardPackage;
        private AiCardInfo aiCardInfo;
        private List<CardModel> tempCardModels = new List<CardModel>();
        public AiCardInfo SortPlayerHandHighCardByLowestHand(List<CardModel> playerCards)
        {
            tempCardModels.Clear();
            tempCardModels.AddRange(playerCards); // Reuse tempCardModels by clearing and adding items
            aiCardInfo.ClearCardPackages(); // Assuming there's a method to clear previous card packages

            // Continue sorting while there are cards left in tempCardModels
            while (tempCardModels.Count > 0)
            {
                // Find the lowest card by ordering the tempCardModels by Rank and then by Suit
                CardModel lowestCard = tempCardModels
                    .OrderBy(card => card.CardRank)
                    .ThenBy(card => card.CardSuit)
                    .FirstOrDefault();

                if (lowestCard == null)
                {
                    Debug.LogWarning("No more cards to sort.");
                    break;
                }

                // Now we create a single card list for the lowest card
                List<CardModel> singleCardList = new List<CardModel> { lowestCard };

                // Check the best hand for this single lowest card
                CardInfo lowestCardInfo = pokerHandsChecker.GetBestHand(singleCardList);

                if (lowestCardInfo == null || lowestCardInfo.CardComposition.Count == 0)
                {
                    Debug.LogWarning("No valid hand found for the card: " + lowestCard);
                    break;
                }

                // Create a new card package for the lowest card
                CardPackage cardPackage = new CardPackage
                {
                    CardPackageType = lowestCardInfo.HandType,
                    CardPackageRank = lowestCardInfo.HandRank,
                    CardPackageContent = new List<CardModel>(lowestCardInfo.CardComposition)
                };

                // Add the new card package to aiCardInfo
                aiCardInfo.AddCardPackage(cardPackage);

                // Remove the card from tempCardModels for the next iteration
                tempCardModels.Remove(lowestCard);
            }

            return aiCardInfo;
        }

        public AiCardInfo SortPlayerPairCardByLowestHand(List<CardModel> playerCards)
        {
            tempCardModels.Clear();
            tempCardModels.AddRange(playerCards); // Reuse tempCardModels by clearing and adding items
            aiCardInfo.ClearCardPackages(); // Assuming there's a method to clear previous card packages

            // Group the cards by rank
            var groupedByRank = tempCardModels
                .GroupBy(card => card.CardRank)
                .Where(group => group.Count() >= 2) // We only want groups that form a pair
                .OrderBy(group => group.Key); // Ordering by the rank of the group

            foreach (var group in groupedByRank)
            {
                // Take the lowest two cards of the same rank to form a pair
                List<CardModel> pair = group.OrderBy(card => card.CardSuit).Take(2).ToList();

                CardInfo pairCardInfo = pokerHandsChecker.GetBestHand(pair);

                if (pairCardInfo == null || pairCardInfo.CardComposition.Count == 0)
                {
                    Debug.LogWarning("No valid hand found for the pair: " + string.Join(", ", pair.Select(card => card.ToString())));
                    continue;
                }

                CardPackage cardPackage = new CardPackage
                {
                    CardPackageType = pairCardInfo.HandType,
                    CardPackageRank = pairCardInfo.HandRank,
                    CardPackageContent = new List<CardModel>(pairCardInfo.CardComposition)
                };

                aiCardInfo.AddCardPackage(cardPackage);

                // Remove the cards from the pair from tempCardModels for the next iteration
                tempCardModels.RemoveAll(card => pair.Contains(card));
            }

            // If no pairs were found and added, this means there were no valid pairs in the hand
            if (aiCardInfo.CardPackages.Count == 0)
            {
                Debug.LogWarning("No pairs found in the hand.");
            }

            return aiCardInfo;
        }

        public AiCardInfo SortPlayerThreeOfAKindCardByLowestHand(List<CardModel> playerCards)
        {
            tempCardModels.Clear();
            tempCardModels.AddRange(playerCards); // Reuse tempCardModels by clearing and adding items
            aiCardInfo.ClearCardPackages(); // Assuming there's a method to clear previous card packages

            // Group the cards by rank
            var groupedByRank = tempCardModels
                .GroupBy(card => card.CardRank)
                .Where(group => group.Count() >= 3) // We only want groups that form a three of a kind
                .OrderBy(group => group.Key); // Ordering by the rank of the group

            foreach (var group in groupedByRank)
            {
                // Take the lowest three cards of the same rank to form a three of a kind
                List<CardModel> threeOfAKind = group.OrderBy(card => card.CardSuit).Take(3).ToList();

                CardInfo threeOfAKindCardInfo = pokerHandsChecker.GetBestHand(threeOfAKind);

                if (threeOfAKindCardInfo == null || threeOfAKindCardInfo.CardComposition.Count == 0)
                {
                    Debug.LogWarning("No valid hand found for the three of a kind: " + string.Join(", ", threeOfAKind.Select(card => card.ToString())));
                    continue;
                }

                CardPackage cardPackage = new CardPackage
                {
                    CardPackageType = threeOfAKindCardInfo.HandType,
                    CardPackageRank = threeOfAKindCardInfo.HandRank,
                    CardPackageContent = new List<CardModel>(threeOfAKindCardInfo.CardComposition)
                };

                aiCardInfo.AddCardPackage(cardPackage);

                // Remove the cards from the three of a kind from tempCardModels for the next iteration
                tempCardModels.RemoveAll(card => threeOfAKind.Contains(card));
            }

            // If no three of a kinds were found and added, this means there were no valid three of a kinds in the hand
            if (aiCardInfo.CardPackages.Count == 0)
            {
                Debug.LogWarning("No three of a kinds found in the hand.");
            }

            return aiCardInfo;
        }

        public AiCardInfo SortPlayerStraightCardByLowestHand(List<CardModel> playerCards)
        {
            tempCardModels.Clear();
            tempCardModels.AddRange(playerCards);
            aiCardInfo.ClearCardPackages();

            // Sort the cards by rank and suit
            var sortedCards = tempCardModels.OrderBy(card => card.CardRank).ThenBy(card => card.CardSuit).ToList();

            for (int i = 0; i <= sortedCards.Count - 5; i++)
            {
                // Check for straight (consecutive ranks)
                if (sortedCards[i].CardRank + 4 == sortedCards[i + 4].CardRank &&
                    sortedCards[i].CardRank + 1 == sortedCards[i + 1].CardRank &&
                    sortedCards[i].CardRank + 2 == sortedCards[i + 2].CardRank &&
                    sortedCards[i].CardRank + 3 == sortedCards[i + 3].CardRank)
                {
                    var straightCards = sortedCards.GetRange(i, 5);
                    CardInfo straightCardInfo = pokerHandsChecker.GetBestHand(straightCards);

                    if (straightCardInfo != null && straightCardInfo.CardComposition.Count == 5)
                    {
                        CardPackage cardPackage = new CardPackage
                        {
                            CardPackageType = straightCardInfo.HandType,
                            CardPackageRank = straightCardInfo.HandRank,
                            CardPackageContent = new List<CardModel>(straightCardInfo.CardComposition)
                        };

                        aiCardInfo.AddCardPackage(cardPackage);
                        // Remove the straight cards from tempCardModels for the next iteration
                        tempCardModels.RemoveAll(card => straightCards.Contains(card));
                        // Straight found, break to prevent overlapping straights
                        break;
                    }
                }
            }

            if (aiCardInfo.CardPackages.Count == 0)
            {
                Debug.LogWarning("No straights found in the hand.");
            }

            return aiCardInfo;
        }

        public AiCardInfo SortPlayerFlushCardByLowestHand(List<CardModel> playerCards)
        {
            tempCardModels.Clear();
            tempCardModels.AddRange(playerCards);
            aiCardInfo.ClearCardPackages();

            // Group the cards by suit
            var groupedBySuit = tempCardModels.GroupBy(card => card.CardSuit)
                                              .Where(group => group.Count() >= 5)
                                              .OrderBy(group => group.Key);

            foreach (var group in groupedBySuit)
            {
                // Take the lowest five cards of the same suit to form a flush
                List<CardModel> flushCards = group.OrderBy(card => card.CardRank).Take(5).ToList();

                CardInfo flushCardInfo = pokerHandsChecker.GetBestHand(flushCards);

                if (flushCardInfo != null && flushCardInfo.CardComposition.Count == 5)
                {
                    CardPackage cardPackage = new CardPackage
                    {
                        CardPackageType = flushCardInfo.HandType,
                        CardPackageRank = flushCardInfo.HandRank,
                        CardPackageContent = new List<CardModel>(flushCardInfo.CardComposition)
                    };

                    aiCardInfo.AddCardPackage(cardPackage);
                    // Remove the flush cards from tempCardModels for the next iteration
                    tempCardModels.RemoveAll(card => flushCards.Contains(card));
                    // Flush found, break to prevent selecting lower ranked flush
                    break;
                }
            }

            if (aiCardInfo.CardPackages.Count == 0)
            {
                Debug.LogWarning("No flushes found in the hand.");
            }

            return aiCardInfo;
        }

        public AiCardInfo SortPlayerFullHouseCardByLowestHand(List<CardModel> playerCards)
        {
            tempCardModels.Clear();
            tempCardModels.AddRange(playerCards);
            aiCardInfo.ClearCardPackages();

            var rankGroups = tempCardModels.GroupBy(card => card.CardRank)
                                           .OrderBy(group => group.Key)
                                           .ToList();

            CardInfo fullHouseCardInfo = null;
            CardPackage cardPackage = null;

            // Find three of a kind
            var threeOfAKindGroup = rankGroups.FirstOrDefault(g => g.Count() >= 3);
            if (threeOfAKindGroup != null)
            {
                // Find a pair outside of the three of a kind
                var pairGroup = rankGroups.FirstOrDefault(g => g.Count() >= 2 && g.Key != threeOfAKindGroup.Key);
                if (pairGroup != null)
                {
                    var threeOfAKind = threeOfAKindGroup.OrderBy(card => card.CardSuit).Take(3).ToList();
                    var pair = pairGroup.OrderBy(card => card.CardSuit).Take(2).ToList();
                    var fullHouseCards = threeOfAKind.Concat(pair).ToList();

                    fullHouseCardInfo = pokerHandsChecker.GetBestHand(fullHouseCards);

                    if (fullHouseCardInfo != null && fullHouseCardInfo.CardComposition.Count == 5)
                    {
                        cardPackage = new CardPackage
                        {
                            CardPackageType = fullHouseCardInfo.HandType,
                            CardPackageRank = fullHouseCardInfo.HandRank,
                            CardPackageContent = new List<CardModel>(fullHouseCardInfo.CardComposition)
                        };

                        aiCardInfo.AddCardPackage(cardPackage);
                    }
                }
            }

            if (cardPackage == null)
            {
                Debug.LogWarning("No full house found in the hand.");
            }

            return aiCardInfo;
        }

        public AiCardInfo SortPlayerFourOfAKindCardByLowestHand(List<CardModel> playerCards)
        {
            tempCardModels.Clear();
            tempCardModels.AddRange(playerCards);
            aiCardInfo.ClearCardPackages();

            var groupedByRank = tempCardModels.GroupBy(card => card.CardRank)
                                              .Where(group => group.Count() == 4)
                                              .OrderBy(group => group.Key);

            foreach (var group in groupedByRank)
            {
                var fourOfAKindCards = group.OrderBy(card => card.CardSuit).ToList();
                CardInfo fourOfAKindCardInfo = pokerHandsChecker.GetBestHand(fourOfAKindCards);

                if (fourOfAKindCardInfo != null && fourOfAKindCardInfo.CardComposition.Count == 4)
                {
                    CardPackage cardPackage = new CardPackage
                    {
                        CardPackageType = fourOfAKindCardInfo.HandType,
                        CardPackageRank = fourOfAKindCardInfo.HandRank,
                        CardPackageContent = new List<CardModel>(fourOfAKindCardInfo.CardComposition)
                    };

                    aiCardInfo.AddCardPackage(cardPackage);
                    break;
                }
            }

            if (aiCardInfo.CardPackages.Count == 0)
            {
                Debug.LogWarning("No four of a kinds found in the hand.");
            }

            return aiCardInfo;
        }

        public AiCardInfo SortPlayerStraightFlushCardByLowestHand(List<CardModel> playerCards)
        {
            tempCardModels.Clear();
            tempCardModels.AddRange(playerCards);
            aiCardInfo.ClearCardPackages();

            var sortedBySuitAndRank = tempCardModels.GroupBy(card => card.CardSuit)
                                                    .SelectMany(group => group.OrderBy(card => card.CardRank))
                                                    .ToList();

            for (int i = 0; i <= sortedBySuitAndRank.Count - 5; i++)
            {
                if (sortedBySuitAndRank[i].CardSuit == sortedBySuitAndRank[i + 4].CardSuit &&
                    sortedBySuitAndRank[i].CardRank + 4 == sortedBySuitAndRank[i + 4].CardRank)
                {
                    var straightFlushCards = sortedBySuitAndRank.GetRange(i, 5);
                    CardInfo straightFlushCardInfo = pokerHandsChecker.GetBestHand(straightFlushCards);

                    if (straightFlushCardInfo != null && straightFlushCardInfo.CardComposition.Count == 5)
                    {
                        CardPackage cardPackage = new CardPackage
                        {
                            CardPackageType = straightFlushCardInfo.HandType,
                            CardPackageRank = straightFlushCardInfo.HandRank,
                            CardPackageContent = new List<CardModel>(straightFlushCardInfo.CardComposition)
                        };

                        aiCardInfo.AddCardPackage(cardPackage);
                        break;
                    }
                }
            }

            if (aiCardInfo.CardPackages.Count == 0)
            {
                Debug.LogWarning("No straight flushes found in the hand.");
            }

            return aiCardInfo;
        }
        #endregion










        private void UpdateCardPosition(List<GameObject> cardsObjectsInPlayerHand,
            List<CardModel> cardModelsInPlayerHand, CardPool cardPool,
            Transform cardParent, PlayerType playerType)
        {
            // Clear out the old cards.
            while (cardsObjectsInPlayerHand.Count > 0)
            {
                var card = cardsObjectsInPlayerHand[0];
                cardsObjectsInPlayerHand.RemoveAt(0);
                cardPool.ReturnCard(card);
            }

            for (int i = 0; i < cardModelsInPlayerHand.Count; i++)
            {
                GameObject cardGO = cardPool.GetCard();
                cardGO.transform.SetParent(cardParent, false);
                cardGO.transform.localRotation = Quaternion.identity;  // Reset rotation to 0,0,0
                cardGO.transform.SetSiblingIndex(i);
                UISelectableCard selectableCard = cardGO.GetComponent<UISelectableCard>();
                selectableCard.Initialize(cardModelsInPlayerHand[i], playerType); // Adjust this if necessary to match CardModel structure.
                cardsObjectsInPlayerHand.Add(cardGO);
            }
        }


        private void UpdateCardPositions(List<GameObject> cardGameObjects)
        {
            for (int i = 0; i < cardGameObjects.Count; i++)
            {
                // This is a placeholder for whatever logic you use to position your cards in the scene.
                // You may set their position, change their order in the hierarchy, etc.
                cardGameObjects[i].transform.SetSiblingIndex(i);
            }
        }

    }
}



