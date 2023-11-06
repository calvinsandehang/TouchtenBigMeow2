using Big2Meow.DeckNCard;
using Big2Meow.FSM;
using Big2Meow.Gameplay;
using Big2Meow.Injection;
using Big2Meow.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;


namespace Big2Meow.Player
{
    /// <summary>
    /// Manages the card submission and validation between players and the table.
    /// </summary>
    [InfoBox("Bridge between Player and Table")]
    public class Big2CardSubmissionCheck : MonoBehaviour
    {
        #region Fields
        // Table
        private Big2TableManager big2TableManager;
        private CardInfo tableInfo;
        public HandType currentTableHandType;
        private HandRank currentTableHandRank;
        private List<CardModel> currentTableCards;

        private Button submitCardButton;
        private List<CardModel> submittedCards = new List<CardModel>();
        private List<CardModel> currentSelectedCard = new List<CardModel>();
        private CardInfo submittedCardInfo;

        private Big2PlayerStateMachine playerSM;
        private Big2PokerHands pokerHandChecker;
        private Big2PlayerHand playerHand;
        private PlayerType playerType;

        private bool isPlaying = false;

        [SerializeField]
        private int _turnDelay = 2;
        #endregion

        #region Events    
        public event Action OnPlayerFinishTurnLocal; // subs : Big2PlayerStateMachine
        #endregion

        #region Monobehavior
        /// <summary>
        /// Initializes the class and subscribes to events.
        /// </summary>
        private void Start()
        {
            ParameterInitialization();
            SubscribeEvent();
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }

        #endregion

        /// <summary>
        /// Handles card submission and validation.
        /// </summary>
        /// <param name="selectedCard">The list of selected cards for submission.</param>

        #region Card Check Logic
        /// <summary>
        /// Handles card submission and validation.
        /// </summary>
        /// <param name="selectedCard">The list of selected cards for submission.</param>
        public void SubmissionCheck(List<CardModel> selectedCard)
        {
            currentSelectedCard = selectedCard;

            // Broadcast an event to notify that card submission is not allowed.
            Big2GlobalEvent.BroadcastCardSubmissionNotAllowed();

            // Check if no cards are selected for submission.
            if (selectedCard.Count == 0)
            {
                //Debug.Log("No cards selected for submission.");
                return;
            }

            // Check if the selected cards contain the Three of Diamonds at the initial round.
            if (Big2GMStateMachine.DetermineWhoGoFirst &&
                !selectedCard.Exists(card => card.CardRank == Rank.Three && card.CardSuit == Suit.Diamonds))
            {
                Debug.LogWarning("The Three of Diamonds must be included in the initial round.");
                Big2GlobalEvent.BroadcastMustIncludeThreeOfDiamond();
                return;
            }

            // Lookup the current table information.
            Big2TableLookUp();

            // Check if the hand type of the selected cards is allowed.
            if (!CompareHandType(selectedCard) && currentTableHandType != HandType.None)
            {
                Debug.Log("Invalid hand type for the current table.");
                return;
            }

            // Clear the submitted cards list.
            ClearSubmittedCardList();

            // Evaluate the selected cards to determine their hand type and rank.
            submittedCardInfo = EvaluateSelectedCards(selectedCard);

            // Check if the hand rank of the selected cards is allowed.
            if (!CompareHandRank(submittedCardInfo.HandRank) || !CheckCardCount(selectedCard, submittedCardInfo))
            {
                Debug.Log("Invalid hand rank or card count.");
                return;
            }

            // Compare the selected cards with the current table cards.
            if (!CompareSelectedCardsWithTableCards(submittedCardInfo.CardComposition))
            {
                Debug.Log("Selected cards do not match the table cards.");
                return;
            }

            // If all checks pass, add the selected cards to the submitted cards.
            AddNewSubmittedCardToSubmittedCardList();

            if (isPlaying)
            {
                // Broadcast an event to notify that card submission is allowed.
                Big2GlobalEvent.BroadcastCardSubmissionAllowed();
            }
            
        }


        /// <summary>
        /// Checks if the card count of selected cards matches the hand type.
        /// </summary>
        private bool CheckCardCount(List<CardModel> selectedCards, CardInfo cardInfo)
        {
            int bestHandCardCount = cardInfo.CardComposition.Count;
            int selectedCardCount = selectedCards.Count;

            return bestHandCardCount == selectedCardCount;
        }

        /// <summary>
        /// Adds the newly submitted cards to the submitted cards list.
        /// </summary>
        private void AddNewSubmittedCardToSubmittedCardList()
        {
            submittedCards.AddRange(submittedCardInfo.CardComposition);
        }

        /// <summary>
        /// Clears the submitted cards list.
        /// </summary>
        private void ClearSubmittedCardList()
        {
            submittedCards.Clear();
        }

        /// <summary>
        /// Evaluates the selected cards to determine their hand type and rank.
        /// </summary>
        private CardInfo EvaluateSelectedCards(List<CardModel> selectedCard)
        {
            var bestHand = pokerHandChecker.GetBestHand(selectedCard);
            var selectedCardHandType = bestHand.HandType;
            var selectedCardHandRank = bestHand.HandRank;
            var bestHandCards = bestHand.CardComposition;
            return new CardInfo(selectedCardHandType, selectedCardHandRank, bestHandCards);
        }

        /// <summary>
        /// Compares the selected cards with the current table cards.
        /// </summary>
        private bool CompareSelectedCardsWithTableCards(List<CardModel> bestHandCards)
        {
            Big2CardComparer big2CardComparer = new Big2CardComparer();
            tableInfo = big2TableManager.TableLookUp();
            currentTableCards = tableInfo.CardComposition;

            return big2CardComparer.CompareHands(bestHandCards, currentTableCards);
        }

        /// <summary>
        /// Compares the hand rank of selected cards with the current table hand rank.
        /// </summary>
        private bool CompareHandRank(HandRank selectedCardHandRank)
        {
            return currentTableHandRank switch
            {
                HandRank.None => true,
                HandRank.HighCard => selectedCardHandRank == HandRank.HighCard,
                HandRank.Pair => selectedCardHandRank >= HandRank.Pair,
                HandRank.ThreeOfAKind => selectedCardHandRank >= HandRank.ThreeOfAKind,
                _ => selectedCardHandRank >= currentTableHandRank
            };
        }


        /// <summary>
        /// Compares the hand type of selected cards with the current table hand type.
        /// </summary>
        private bool CompareHandType(List<CardModel> submittedCardModels)
        {
            int cardCount = submittedCardModels.Count;

            return cardCount switch
            {
                0 => currentTableHandType == HandType.None,
                1 => currentTableHandType == HandType.Single,
                2 => currentTableHandType == HandType.Pair,
                3 => currentTableHandType == HandType.ThreeOfAKind,
                5 => currentTableHandType == HandType.FiveCards,
                _ => false
            };
        }
        #endregion

        #region Submission Button
        /// <summary>
        /// Sets up the submission button and initializes its behavior.
        /// </summary>
        private void SetupSubmissionButton()
        {
            submitCardButton = UIButtonInjector.Instance.GetButton(ButtonType.SubmitCard);
            submitCardButton.onClick.AddListener(OnSubmitCard);

            UIPlayerSubmissionButton submitButtonBehaviour = submitCardButton.GetComponent<UIPlayerSubmissionButton>();
            submitButtonBehaviour.InitializeButton(this);
        }

        /// <summary>
        /// Handles the submission of cards and updates the game state.
        /// </summary>
        public void OnSubmitCard()
        {
            //Debug.Log("OnSubmitCard");
            Big2GlobalEvent.BroadcastSubmitCard(submittedCardInfo);
            playerHand.RemoveCards(submittedCards);

            Big2GlobalEvent.BroadcastCardSubmissionNotAllowed();
            StartCoroutine(DelayedAction(EndTurn, _turnDelay));

            isPlaying = false;
        }

        /// <summary>
        /// Ends the player's turn and handles the game state.
        /// </summary>
        private void EndTurn()
        {
            if (!Big2GMStateMachine.WinnerIsDetermined)
            {
                Debug.Log("player end turn, redirect to waiting state");
                Big2GlobalEvent.BroadcastPlayerFinishTurnGlobal(playerHand);
                OnPlayerFinishTurnLocal?.Invoke();
            }
            else
            {
                Debug.Log("player end turn, but not redirect to waiting state");
            }
        }
        #endregion

        #region Event
        /// <summary>
        /// Subscribes to relevant events.
        /// </summary>
        private void SubscribeEvent()
        {
            playerSM.OnPlayerIsPlaying += OnPlaying;
        }

        /// <summary>
        /// Unsubscribes from relevant events.
        /// </summary>
        private void UnsubscribeEvent()
        {
            playerSM.OnPlayerIsPlaying -= OnPlaying;
        }

        private void OnPlaying() 
        {
            isPlaying = true;

            SubmissionCheck(currentSelectedCard);
        }
        #endregion

        #region Helper
        /// <summary>
        /// Looks up the current table information.
        /// </summary>
        private void Big2TableLookUp()
        {
            tableInfo = big2TableManager.TableLookUp();
            currentTableHandType = tableInfo.HandType;
            currentTableHandRank = tableInfo.HandRank;
            currentTableCards = new List<CardModel>();
        }

        /// <summary>
        /// Initializes various parameters and subscribes to events.
        /// </summary>
        private void ParameterInitialization()
        {
            playerHand = GetComponent<Big2PlayerHand>();
            playerSM = GetComponent<Big2PlayerStateMachine>();
            playerType = playerHand.PlayerTypeLookUp();

            if (playerType == PlayerType.Human)
            {
                SetupSubmissionButton();
            }

            big2TableManager = Big2TableManager.Instance;
            Big2TableLookUp();

            pokerHandChecker = new Big2PokerHands();

            SubscribeEvent();
        }


        /// <summary>
        /// Executes an action after a specified delay.
        /// </summary>
        private IEnumerator DelayedAction(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        #endregion
    }

}
