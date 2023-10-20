using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static GlobalDefine;

public class Big2PlayerHand : SubjectPlayer
{
    public PlayerType PlayerType;

    public List<CardModel> playerCards = new List<CardModel>();
    private const int playerHandSize = 13;

    private Dealer dealer;
    private PlayerHandEvaluator handEvaluator;
    private UIPlayerHandManager uiPlayerHandManager;

    private void Awake()
    {
        ParameterInitialization();
       
        SubscribeEvent();
    }

   

    private void SubscribeEvent() 
    {
        dealer.OnDealerFinishDealingCards += EvaluateCardInHand;
    }

    private void UnsubscribeEvent()
    {
        dealer.OnDealerFinishDealingCards -= EvaluateCardInHand;
    }

    public void AddCard(CardModel card) 
    {
        playerCards.Add(card);

        // If this is the last card, notify that the hand is complete.
        if (PlayerType == PlayerType.Human)
            UIPlayerHandManager.Instance.DisplayCards(playerCards);

    }

    public void RemoveCards(List<CardModel> removedCards)
    {
        // Create a HashSet of cards to be removed based on their rank and suit
        HashSet<CardModel> cardsToRemove = new HashSet<CardModel>(removedCards);

        // Remove the cards from playerCards that match the criteria
        playerCards.RemoveAll(card => cardsToRemove.Contains(card));

        // Notify UI to update the displayed cards
        if (PlayerType == PlayerType.Human) 
        {
            UIPlayerHandManager.Instance.DisplayCards(playerCards);
            CardEvaluator.Instance.DeregisterCard(removedCards);
            NotifyObserver(playerCards);
        }
    }


    #region Helper
    private void ParameterInitialization()
    {
        handEvaluator = GetComponent<PlayerHandEvaluator>();
        dealer = Dealer.Instance;

        // Injecting this instance to the UIPlayerHandManager
        if (PlayerType == PlayerType.Human)
        {
            uiPlayerHandManager = UIPlayerHandManager.Instance;
            uiPlayerHandManager.InitialializedPlayerHand(this);
        }
    }

    public PlayerType PlayerTypeLookUp() 
    {
        return PlayerType;
    }
    #endregion
    public void EvaluateCardInHand() 
    {
        handEvaluator.EvaluateHand(playerCards.ToList());
    }

    // Get the cards in the player hand
    // Would be useful for sorting cards
    public List<CardModel> GetPlayerCards() 
    {
        return playerCards;
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }
}
