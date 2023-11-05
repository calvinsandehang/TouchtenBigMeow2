using Big2Meow.DeckNCard;
using Big2Meow.FSM;
using Big2Meow.Gameplay;
using Big2Meow.UI;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;


namespace Big2Meow.Player
{
    /// <summary>
    /// Represents a player's hand in the Big2 card game.
    /// </summary>
    public class Big2PlayerHand : MonoBehaviour, ISubscriber, IPlayer
    {
        /// <summary>
        /// Gets or sets the type of the player (Human, AI, etc.).
        /// </summary>
        public PlayerType PlayerType { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the player.
        /// </summary>
        public int PlayerID { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of cards a player can hold in their hand.
        /// </summary>
        public int HandSize { get; set; }

        private List<CardModel> playerCards = new List<CardModel>();
        private bool hasThreeOfDiamonds;
        private bool hasQuadrupleTwo;

        private Big2GMStateMachine gameMaster;
        private Big2CardSubmissionCheck cardSubmissionCheck;
        private Big2PokerHands pokerHandCheck;

        #region Monobehaviour
        private void Awake()
        {
            ParameterInitialization();
            SubscribeEvent();
        }

        private void Start()
        {
            ComponentInitialization();
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }

        #endregion

        #region Initialization
        /// <summary>
        /// Initializes parameters related to the player.
        /// </summary>
        public void ParameterInitialization()
        {
            gameMaster = Big2GMStateMachine.Instance;
            cardSubmissionCheck = GetComponent<Big2CardSubmissionCheck>();
            pokerHandCheck = new Big2PokerHands();
        }

        /// <summary>
        /// Initializes components and dependencies for the player.
        /// </summary>
        public void ComponentInitialization()
        {
            if (PlayerType == PlayerType.Human)
            {
                // Only Human has Card Evaluator to evaluate the cards click by the Human
                Big2PlayerCardEvaluator cardEvaluator = gameObject.AddComponent<Big2PlayerCardEvaluator>();
                cardEvaluator.InitializeCardEvaluator(cardSubmissionCheck);
            }
        }

        /// <summary>
        /// Initializes the unique identifier of the player.
        /// </summary>
        /// <param name="index">The index to assign as the player's identifier.</param>
        public void InitializePlayerID(int index)
        {
            PlayerID = index;
        }


        #endregion

        #region Player methods
        /// <summary>
        /// Adds a card to the player's hand.
        /// </summary>
        /// <param name="card">The CardModel to add to the player's hand.</param>
        public void AddCard(CardModel card)
        {
            playerCards.Add(card);

            UIPlayerHandManager.Instance.DisplayCards(playerCards, PlayerID, PlayerType);

            if (gameMaster.CheckGameInFirstRound())
            {
                if (card.CardRank == Rank.Three && card.CardSuit == Suit.Diamonds)
                    hasThreeOfDiamonds = true;
            }
        }

        /// <summary>
        /// Checks if the number of cards in the player's hand is below six and broadcasts an event if true.
        /// </summary>
        public void CheckCardBelowSix()
        {
            if (playerCards.Count < 6)
            {
                Big2GlobalEvent.BroadcastPlayerCardLessThanSix();
            }
        }

        /// <summary>
        /// Checks if the player has a quadruple of Two cards.
        /// </summary>
        /// <returns>True if the player has a quadruple of Two cards, otherwise false.</returns>
        public bool CheckHavingQuadrupleTwoCard()
        {
            hasQuadrupleTwo = pokerHandCheck.HasAllFourTwos(playerCards);
            return hasQuadrupleTwo;
        }

        /// <summary>
        /// Checks if the player has the Three of Diamonds card.
        /// </summary>
        /// <returns>True if the player has the Three of Diamonds, otherwise false.</returns>
        public bool CheckHavingThreeOfDiamonds()
        {
            return hasThreeOfDiamonds;
        }

        /// <summary>
        /// Checks if the player has won by having no cards left in their hand.
        /// </summary>
        public void CheckWinningCondition()
        {
            if (playerCards.Count == 0)
            {
                Debug.Log($"Player {PlayerID} has dropped their last card");
                Big2GlobalEvent.BroadcastPlayerDropLastCard(this);
            }
        }
        #endregion

        /// <summary>
        /// Retrieves the cards currently held by the player.
        /// </summary>
        /// <returns>A list of CardModel representing the player's cards.</returns>
        public List<CardModel> GetPlayerCards()
        {
            return playerCards;
        }


        /// <summary>
        /// Looks up and retrieves the type of the player.
        /// </summary>
        /// <returns>The type of the player (Human, AI, etc.).</returns>
        public PlayerType PlayerTypeLookUp()
        {
            return PlayerType;
        }

        /// <summary>
        /// Removes specified cards from the player's hand.
        /// </summary>
        /// <param name="removedCards">A list of CardModel to be removed from the player's hand.</param>
        public void RemoveCards(List<CardModel> removedCards)
        {
            // Create a HashSet of cards to be removed based on their rank and suit
            HashSet<CardModel> cardsToRemove = new HashSet<CardModel>(removedCards);

            // Remove the cards from playerCards that match the criteria
            playerCards.RemoveAll(card => cardsToRemove.Contains(card));

            UIPlayerHandManager.Instance.DisplayCards(playerCards, PlayerID, PlayerType);

            // Notify UI to update the displayed cards
            if (PlayerType == PlayerType.Human)
            {
                Big2PlayerCardEvaluator.Instance.DeregisterCards(removedCards);
            }

            CheckCardBelowSix();
            CheckWinningCondition();
        }

        /// <summary>
        /// Resets the player's hand by clearing all cards.
        /// </summary>
        /// <param name="playerHand">The Big2PlayerHand instance representing the player's hand.</param>
        /// 
        public void ResetPlayerCard()
        {
            playerCards.Clear();
        }
        public void ResetPlayerCard(Big2PlayerHand playerHand)
        {
            playerCards.Clear();
        }

        #region Subscribe Event
        /// <summary>
        /// Subscribes to relevant game events.
        /// </summary>
        public void SubscribeEvent()
        {
            Big2GlobalEvent.SubscribePlayerDropLastCard(ResetPlayerCard);
            Big2GlobalEvent.SubscribeHavingQuadrupleTwo(ResetPlayerCard);
        }

        /// <summary>
        /// Unsubscribes from relevant game events.
        /// </summary>
        public void UnsubscribeEvent()
        {
            Big2GlobalEvent.UnsubscribePlayerDropLastCard(ResetPlayerCard);
            Big2GlobalEvent.UnsubscribeHavingQuadrupleTwo(ResetPlayerCard);
        }
        #endregion


        #region Testing Purposes
        public void EvaluateCardInHand()
        {
            //handEvaluator.EvaluateHand(playerCards.ToList());
        }

        // Add this method to your Big2PlayerHand class
        public List<CardModel> GetPlayerCardsForEditor()
        {
            return playerCards;
        }

        #endregion
    }

}
