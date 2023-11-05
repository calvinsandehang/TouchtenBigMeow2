using Big2Meow.DeckNCard;
using Big2Meow.FSM;
using Big2Meow.Gameplay;
using Big2Meow.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GlobalDefine;

namespace Big2Meow.AI 
{
    /// <summary>
    /// Represents a simple AI for the Big2 card game that decides moves based on predefined logic.
    /// </summary>
    [DefaultExecutionOrder(1)]
    public class Big2SimpleAI : MonoBehaviour
    {
        [SerializeField]
        private float _turnDelay = 3f;

        private Big2PlayerHand playerHand;        
        private Big2PokerHands big2PokerHands;
        private Big2CardSorter cardSorter;
        private Big2CardComparer big2CardComparer;
        private List<CardModel> aiCards;
        public AiCardInfo aiCardInfo;

        // Table
        private Big2TableManager big2TableManager;
        private CardInfo tableInfo;
        private HandType currentTableHandType;
        private HandRank currentTableHandRank;
        private List<CardModel> currentTableCards;

        #region Monobehaviour
        private void Awake()
        {
            playerHand = GetComponent<Big2PlayerHand>();         
            big2PokerHands = new Big2PokerHands();
            big2CardComparer = new Big2CardComparer();
            cardSorter = GetComponent<Big2CardSorter>();
        }

        private void Start()
        {
            big2TableManager = Big2TableManager.Instance;
        }
        #endregion

        #region AI Decision Making

        /// <summary>
        /// Initiates AI decision making with a delay.
        /// </summary>
        public void InitiateAiDecisionMaking()
        {
            StartCoroutine(AIDecisionMaking());
        }

        /// <summary>
        /// This coroutine handles the AI's decision-making process during their turn.
        /// </summary>
        private IEnumerator AIDecisionMaking()
        {
            // Wait for the predefined turn delay before making a decision
            yield return new WaitForSeconds(_turnDelay);

            // Update the AI's cards from their hand
            aiCards = playerHand.GetPlayerCards();

            // Display the AI's cards for debugging purposes
            //DebugAIHand(aiCards);

            // Check the current state of the table to make an informed decision
            Big2TableLookUp();

            // Decide the course of action based on the table's state
            if (currentTableHandRank == HandRank.None)
            {
                HandleEmptyTable();
            }
            else
            {
                // Attempt to play a card package that beats the current table hand
                bool hasPlayed = TryPlayBeatingHand();

                if (!hasPlayed)
                {
                    // If no suitable hand is found, skip the turn
                    SkipTurn();
                }
            }

        }

        /// <summary>
        /// Display the AI's cards in the console for debugging purposes.
        /// </summary>
        /// <param name="aiCards">The list of AI's card models.</param>
        private void DebugAIHand(List<CardModel> aiCards)
        {
            string aiCardsContent = string.Join(", ", aiCards.Select(card => $"Rank: {card.CardRank}, Suit: {card.CardSuit}"));
            Debug.Log($"AI Cards: {aiCardsContent}");
        }

        /// <summary>
        /// Handle the situation where the table is empty and AI has to lead the turn.
        /// </summary>
        private void HandleEmptyTable()
        {
            if (Big2GMStateMachine.DetermineWhoGoFirst)
            {
                HandleThreeOfDiamonds();
            }
            else
            {
                HandleLowestHand();
            }
            EndTurn();
        }

        /// <summary>
        /// Attempts to play a hand that beats the current table's hand.
        /// If a suitable hand is found, it plays that hand and ends the turn.
        /// </summary>
        /// <returns>True if a suitable hand is played, false otherwise.</returns>
        private bool TryPlayBeatingHand()
        {
            AiCardInfo aiCardInfo = SortCardsByCurrentTableHandType(aiCards);

            foreach (var cardPackage in aiCardInfo.CardPackages)
            {
                //DebugCardPackage(cardPackage);

                if (CompareCardWithTableCards(cardPackage.CardPackageContent))
                {
                    CardInfo submittedCardInfo = EvaluateSelectedCards(cardPackage.CardPackageContent);
                    OnSubmitCard(submittedCardInfo, cardPackage.CardPackageContent);
                    EndTurn();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sorts the AI's cards based on the current table hand type.
        /// </summary>
        /// <param name="aiCards">The AI's current hand.</param>
        /// <returns>A sorted AiCardInfo object with potential card packages to play.</returns>
        private AiCardInfo SortCardsByCurrentTableHandType(List<CardModel> aiCards)
        {
            switch (currentTableHandType)
            {
                case HandType.Single:
                    return cardSorter.SortPlayerHandHighCardByLowestHand(aiCards);
                case HandType.Pair:
                    return cardSorter.SortPlayerPairCardByLowestHand(aiCards);
                case HandType.ThreeOfAKind:
                    return cardSorter.SortPlayerThreeOfAKindCardByLowestHand(aiCards);
                case HandType.FiveCards:
                    return SortCardsForFiveCardHand();
                default:
                    throw new InvalidOperationException($"Unhandled table hand type: {currentTableHandType}");
            }
        }

        /// <summary>
        /// Sorts the AI's cards for a five-card hand based on the current table hand rank.
        /// </summary>
        /// <returns>A sorted AiCardInfo object with potential card packages to play.</returns>
        private AiCardInfo SortCardsForFiveCardHand()
        {
            switch (currentTableHandRank)
            {
                case HandRank.Straight:
                    return cardSorter.SortPlayerStraightCardByLowestHand(aiCards);
                case HandRank.Flush:
                    return cardSorter.SortPlayerFlushCardByLowestHand(aiCards);
                case HandRank.FullHouse:
                    return cardSorter.SortPlayerFullHouseCardByLowestHand(aiCards);
                case HandRank.FourOfAKind:
                    return cardSorter.SortPlayerFourOfAKindCardByLowestHand(aiCards);
                case HandRank.StraightFlush:
                    return cardSorter.SortPlayerStraightFlushCardByLowestHand(aiCards);
                default:
                    throw new InvalidOperationException($"Unhandled five card hand rank: {currentTableHandRank}");
            }
        }

        /// <summary>
        /// Prints the contents of a card package for debugging purposes.
        /// </summary>
        /// <param name="cardPackage">The card package to display.</param>
        private void DebugCardPackage(CardPackage cardPackage)
        {
            string cardPackageContent = string.Join(", ", cardPackage.CardPackageContent.Select(card => $"Rank: {card.CardRank}, Suit: {card.CardSuit}"));
            Debug.Log($"Attempting with card package: {cardPackageContent}");
        }


        #endregion

        #region Helper Methods

        /// <summary>
        /// Handles playing the Three of Diamonds card.
        /// </summary>
        private void HandleThreeOfDiamonds()
        {
            CardInfo lowestHandInfo = big2PokerHands.GetThreeOfDiamonds(aiCards);
            OnSubmitCard(lowestHandInfo, lowestHandInfo.CardComposition);
        }

        /// <summary>
        /// Handles playing the lowest hand available.
        /// </summary>
        private void HandleLowestHand()
        {
            CardInfo lowestHandInfo = big2PokerHands.GetLowestHand(aiCards);
            OnSubmitCard(lowestHandInfo, lowestHandInfo.CardComposition);
        }

        /// <summary>
        /// Checks if a card package is suitable to be played.
        /// </summary>
        /// 

        private bool IsCardPackageSuitable(CardPackage cardPackage)
        {
            if (!CompareHandType(cardPackage.CardPackageContent) && currentTableHandType != HandType.None) return false;
            if (!CompareHandRank(cardPackage.CardPackageRank)) return false;
            if (!CompareCardWithTableCards(cardPackage.CardPackageContent)) return false;

            return true;
        }

        /// <summary>
        /// Looks up the current state of the Big2 table.
        /// </summary>
        private void Big2TableLookUp()
        {
            tableInfo = big2TableManager.TableLookUp();
            currentTableHandType = tableInfo.HandType;
            currentTableHandRank = tableInfo.HandRank;
            currentTableCards = new List<CardModel>();
        }

        /// <summary>
        /// Compares the hand type of submitted cards with the current table hand type.
        /// </summary>
        private bool CompareHandType(List<CardModel> submittedCardModels)
        {
            int cardCount = submittedCardModels.Count;
            Debug.Log("CompareHandType. currentTableHandType : " + currentTableHandType + ", cardCount : " + cardCount);
            switch (cardCount)
            {
                case 0:
                    return currentTableHandType == HandType.None;
                case 1:
                    return currentTableHandType == HandType.Single;
                case 2:
                    return currentTableHandType == HandType.Pair;
                case 3:
                    return currentTableHandType == HandType.ThreeOfAKind;
                case 5:
                    return currentTableHandType == HandType.FiveCards;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Compares the hand rank of submitted cards with the current table hand rank.
        /// </summary>
        private bool CompareHandRank(HandRank selectedCardHandRank)
        {
            Debug.Log("CompareHandRank. currentTableHandRank : " + currentTableHandRank + ", selectedCardHandRank : " + selectedCardHandRank);
            switch (currentTableHandRank)
            {
                case HandRank.None:
                    return true;
                case HandRank.HighCard:
                    // Allow any hand rank that is equal or higher
                    return selectedCardHandRank == HandRank.HighCard;
                case HandRank.Pair:
                    // Allow any hand rank that is equal or higher, except HighCard
                    return selectedCardHandRank == HandRank.Pair;
                case HandRank.ThreeOfAKind:
                    // Allow any hand rank that is equal or higher, except HighCard and Pair
                    return selectedCardHandRank == HandRank.ThreeOfAKind;
                case HandRank.Straight:
                case HandRank.Flush:
                case HandRank.FullHouse:
                case HandRank.FourOfAKind:
                case HandRank.StraightFlush:
                    // Allow only the same or higher hand rank
                    return selectedCardHandRank >= currentTableHandRank;
            }

            return false;
        }

        /// <summary>
        /// Compares the selected cards with the current table cards.
        /// </summary>
        private bool CompareCardWithTableCards(List<CardModel> bestHandCards)
        {
            tableInfo = big2TableManager.TableLookUp();
            currentTableCards = tableInfo.CardComposition;

            return big2CardComparer.CompareHands(bestHandCards, currentTableCards);
        }

        /// <summary>
        /// Evaluates the selected cards to determine their hand type and rank.
        /// </summary>
        private CardInfo EvaluateSelectedCards(List<CardModel> selectedCard)
        {
            var bestHand = big2PokerHands.GetBestHand(selectedCard);
            var selectedCardHandType = bestHand.HandType;
            var selectedCardHandRank = bestHand.HandRank;
            var bestHandCards = bestHand.CardComposition;
            return new CardInfo(selectedCardHandType, selectedCardHandRank, bestHandCards);
        }

        #endregion

        #region AI Action Methods

        /// <summary>
        /// Handles the submission of cards by the AI player.
        /// </summary>
        /// <param name="submittedCardInfo">Information about the submitted cards.</param>
        /// <param name="submittedCards">The cards being submitted.</param>
        private void OnSubmitCard(CardInfo submittedCardInfo, List<CardModel> submittedCards)
        {
            // Create a list of card descriptions
            List<string> cardDescriptions = new List<string>();

            // Debug each submitted card and add its description to the list
            foreach (var card in submittedCards)
            {
                cardDescriptions.Add(card.CardRank + " of " + card.CardSuit);
            }

            // Join the card descriptions into a single string
            string submittedCardsString = string.Join(", ", cardDescriptions);

            // Log the submitted cards
            Debug.Log("Player " + (playerHand.PlayerID) + " SubmitCard: " + submittedCardsString);

            // Update table and remove cards
            Big2GlobalEvent.BroadcastSubmitCard(submittedCardInfo);
            playerHand.RemoveCards(submittedCards);
        }

        /// <summary>
        /// Ends the AI player's turn and takes appropriate actions.
        /// </summary>
        private void EndTurn()
        {
            if (!Big2GMStateMachine.WinnerIsDetermined)
            {
                Debug.Log($"AI {playerHand.PlayerID} end turn, redirect to waiting state");
                Big2GlobalEvent.BroadcastAIFinishTurnGlobal(playerHand);
            }
            else
            {
                Debug.Log($"AI {playerHand.PlayerID} end turn, but not redirect to waiting state");
            }
        }

        /// <summary>
        /// Handles the AI player's decision to skip their turn.
        /// </summary>
        private void SkipTurn()
        {
            Debug.Log("Player " + (playerHand.PlayerID) + " Skip Turn");
            Big2GlobalEvent.BroadcastAISkipTurnGlobal(playerHand);
        }

        #endregion
    }
}

