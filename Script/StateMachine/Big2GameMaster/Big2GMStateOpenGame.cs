using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

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
        // Cleanup or additional logic when exiting the state
    }

    public override GMState GetActiveState()
    {
        return GMState.OpenGame;
    }

    public override void UpdateState()
    {
        // Additional state-specific update logic if needed
    }

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

        // Determining player turn
        if (!GMSM.IsInitialized)
        {
            GMSM.IsInitialized = true;
            DetermineWhoPlayFirst();
        }
        else
        {
            PreviousWinnerGoFirst();
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

    /// <summary>
    /// Initialize the deck for the game.
    /// </summary>
    private void InitializeDeck()
    {
        if (GMSM.DeckModel == null)
        {
            GMSM.DeckModel = new DeckModel(GMSM.Deck);
        }
        else
        {
            GMSM.DeckModel.ResetDeck(); // Reset the deck for a new game
        }

        ShuffleDeck();
    }

    /// <summary>
    /// Shuffle the deck.
    /// </summary>
    private void ShuffleDeck()
    {
        GMSM.DeckModel.Shuffle();
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

    /// <summary>
    /// Deal cards to each player.
    /// </summary>
    private void DealCards()
    {
        // Deal 13 cards to each player
        for (int i = 0; i < +GMSM.TotalCardInHandPerplayer; i++)
        {
            foreach (Big2PlayerHand playerHand in GMSM.PlayerHands)
            {
                CardModel card = GMSM.DeckModel.DrawCard();

                if (card != null)
                {
                    playerHand.AddCard(card);
                }
                else
                {
                    Debug.LogError("Not enough cards in the deck");
                    return;
                }
            }
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
}
