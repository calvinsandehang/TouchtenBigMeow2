using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Security;
using UnityEngine;
using static GlobalDefine;

public class Big2GMStateOpenGame : BaseState<GMState>
{
    private Big2GMStateMachine GMSM;
    public Big2GMStateOpenGame(GMState key, Big2GMStateMachine stateMachine) : base(key)
    {
        GMSM = stateMachine;
    }

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

    }

    private void InitializeGame() 
    {
        Big2GMStateMachine.WinnerIsDetermined = false;

        InitializeDeck();
        DeckShuffle();

        if (!GMSM.IsInitialized)
            InitializePlayer();
        else
            ResetPlayerCard();        

        DealCards();

        if (!GMSM.IsInitialized)
        {
            GMSM.IsInitialized = true;
            DetermineWhoPlayFirst();
        }

        if (GMSM.IsInitialized)
        {
            PreviousWinnerGoFirst();
        }
    }

    private void PreviousWinnerGoFirst()
    {
        int winnerPlayerIndex = GMSM.FindElementIndex(GMSM.PlayerHands, GMSM.WinnerPlayer);

        GMSM.SetTurn(winnerPlayerIndex);
    }

    private void InitializeDeck()
    {
        GMSM.DeckModel = new DeckModel(GMSM.Deck);
    }

    private void DeckShuffle()
    {
        GMSM.DeckModel.Shuffle();
    }

    private void InitializePlayer()
    {
        // create hands for each player
        for (int i = 0; i < GMSM.PlayerNumber; i++)
        {
            GameObject playerHandObject = GMSM.InstantiatePlayer();
            Big2PlayerHand playerHand = playerHandObject.GetComponent<Big2PlayerHand>();
            Big2PlayerUIManager playerUIManager = playerHandObject.GetComponent<Big2PlayerUIManager>();

            if (i == 0)            
                playerHand.PlayerType = PlayerType.Human;            
            else
                playerHand.PlayerType = PlayerType.AI;

            // initializae ID
            playerHand.InitializePlayerID(i);
            // initialize UI Elements
            playerUIManager.SetupSkipNotificationButton(GMSM.PlayerUIComponents[i].SkipNotification);
            playerUIManager.SetupProfilePicture(GMSM.PlayerUIComponents[i].ProfilePicture);
            playerUIManager.SetupCardParent(GMSM.PlayerUIComponents[i].CardParent);

            GMSM.PlayerHands.Add(playerHand);
        }
    }

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
                    Debug.LogError("Not enough card on the deck");
                    return;
                }
            }
        }
        GMSM.BroadcastCardHasBeenDealt();

    }

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

    private void ResetPlayerCard() 
    {
        
    }


}
