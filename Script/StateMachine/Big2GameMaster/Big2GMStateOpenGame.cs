using Big2Meow.DeckNCard;
using Big2Meow.Gameplay;
using Big2Meow.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

namespace Big2Meow.FSM
{
    public class Big2GMStateOpenGame : BaseState<GMState>
    {
        private Big2GMStateMachine GMSM;

        public Big2GMStateOpenGame(GMState key, Big2GMStateMachine stateMachine) : base(key)
        {
            GMSM = stateMachine;
        }

        /// <summary>
        /// Enter the Open Game state.
        /// </summary>
        public override void EnterState()
        {
            Debug.Log("GM in Open Game State");
            InitializeGame();
        }

        public override void ExitState()
        {
        }

        public override GMState GetActiveState()
        {
            return GMState.OpenGame;
        }

        public override void UpdateState()
        {
            // Additional state-specific update logic if needed
        }

        #region Initialization
        /// <summary>
        /// Initialize the game state.
        /// </summary>
        private void InitializeGame()
        {
            Big2GMStateMachine.WinnerIsDetermined = false;

            InitializeDeck();

            if (!GMSM.IsInitialized)
                InitializePlayer();

            DealCards();

            bool violatingRule = GMSM.Big2Rule.CheckBig2RuleViolation(GMSM.PlayerHands);

            if (!violatingRule)
            {
                DetermineTurn();
                GMSM.IsInitialized = true;
            }
            else
            {
                // global event has been broadcasted in CheckBig2RuleViolation()
                // should there is a violation
            }
        }

        /// <summary>
        /// Initializes the deck for the game based on the selected deck type.
        /// </summary>
        private void InitializeDeck()
        {
            // Check if the DeckModel is already initialized.
            if (GMSM.DeckModel == null)
            {
                // Initialize based on the deck type.
                GMSM.DeckModel = GMSM.DeckType switch
                {
                    DeckType.Normal => new DeckModel(GMSM.NormalDeck),
                    DeckType.Test => new DeckModel(GMSM.TestDeck.SetupTestDeck()),
                    _ => throw new InvalidOperationException("Unknown deck type")
                };
            }
            else
            {
                // Reset the deck for a new game if already initialized.
                GMSM.DeckModel.ResetDeck();
            }

            // Shuffle the deck if it's a normal game.
            if (GMSM.DeckType == DeckType.Normal)
                ShuffleDeck();
        }

        /// <summary>
        /// Initialize player hands and UI components.
        /// </summary>
        private void InitializePlayer()
        {
            // Create hands for each player
            for (int i = 0; i < GMSM.PlayerNumber; i++)
            {
                GameObject playerHandObject = GMSM.InstantiatePlayer();
                Big2PlayerHand playerHand = playerHandObject.GetComponent<Big2PlayerHand>();
                Big2PlayerUIManager playerUIManager = playerHandObject.GetComponent<Big2PlayerUIManager>();

                if (i == 0)
                    playerHand.PlayerType = PlayerType.Human;
                else
                    playerHand.PlayerType = PlayerType.AI;

                // Initialize ID
                playerHand.InitializePlayerID(i);
                // Initialize UI Elements
                playerUIManager.SetupSkipNotificationButton(GMSM.PlayerUIComponents[i].SkipNotification);
                playerUIManager.SetupProfilePicture(GMSM.PlayerUIComponents[i].ProfilePicture);
                playerUIManager.SetupCardParent(GMSM.PlayerUIComponents[i].CardParent);

                GMSM.PlayerHands.Add(playerHand);
            }
        }
        #endregion

        #region Check Rule Violation

        private bool CheckBig2RuleViolation()
        {
            foreach (var player in GMSM.PlayerHands)
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


        #endregion
        #region Determine Turn
        private void DetermineTurn()
        {
            if (!GMSM.IsInitialized)
            {
                DetermineWhoPlayFirst();
            }
            else
            {
                PreviousWinnerGoFirst();
            }
        }

        /// <summary>
        /// Determine the player who plays first based on having the three of diamonds.
        /// </summary>
        private void DetermineWhoPlayFirst()
        {
            foreach (var player in GMSM.PlayerHands)
            {
                if (player.CheckHavingThreeOfDiamonds())
                {
                    GMSM.CurrentPlayerIndex = GMSM.PlayerHands.IndexOf(player);
                    GMSM.SetTurn(GMSM.CurrentPlayerIndex);
                    Debug.Log("The player that goes first is Player " + (GMSM.CurrentPlayerIndex));
                    break;
                }
            }
        }

        /// <summary>
        /// Determine the player who won the previous game and let them go first.
        /// </summary>
        private void PreviousWinnerGoFirst()
        {
            int winnerPlayerIndex = GMSM.FindElementIndex(GMSM.PlayerHands, GMSM.WinnerPlayer);

            GMSM.SetTurn(winnerPlayerIndex);
        }
        #endregion

        #region Deal Cards
        /// <summary>
        /// Deals cards to each player according to the deck type.
        /// </summary>
        private void DealCards()
        {
            switch (GMSM.DeckType)
            {
                case DeckType.Normal:
                    DealCardsRoundRobin();
                    break;
                case DeckType.Test:
                    DealCardsSequentially();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported deck type for dealing cards.");
            }
        }

        /// <summary>
        /// Deals cards to players in a round-robin fashion.
        /// </summary>
        private void DealCardsRoundRobin()
        {
            for (int i = 0; i < GMSM.TotalCardInHandPerplayer; i++)
            {
                foreach (Big2PlayerHand playerHand in GMSM.PlayerHands)
                {
                    TryDealCardToPlayer(playerHand);
                }
            }
        }

        /// <summary>
        /// Deals all cards to each player sequentially.
        /// </summary>
        private void DealCardsSequentially()
        {
            foreach (Big2PlayerHand playerHand in GMSM.PlayerHands)
            {
                for (int cardIndex = 0; cardIndex < GMSM.TotalCardInHandPerplayer; cardIndex++)
                {
                    TryDealCardToPlayer(playerHand);
                }
            }
        }

        /// <summary>
        /// Attempts to deal a single card to the given player hand.
        /// </summary>
        /// <param name="playerHand">The player hand to deal a card to.</param>
        private void TryDealCardToPlayer(Big2PlayerHand playerHand)
        {
            CardModel card = GMSM.DeckModel.DrawCard();

            if (card != null)
            {
                playerHand.AddCard(card);
            }
            else
            {
                Debug.LogError("Not enough cards in the deck.");
                // Consider adding some error handling here, like breaking out of the loop.
            }
        }


        /// <summary>
        /// Shuffle the deck.
        /// </summary>
        private void ShuffleDeck()
        {
            GMSM.DeckModel.Shuffle();
        }

        #endregion

    }
}

