using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public CardModel[] playerCards;
    private const int playerHandSize = 13;

    private Dealer dealer;
    private PlayerHandEvaluator handEvaluator;

    private void Awake()
    {
        playerCards = new CardModel[playerHandSize];
        handEvaluator =GetComponent<PlayerHandEvaluator>();
        dealer = Dealer.Instance;
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

    public void AddCard(CardModel card, int index) 
    {
        if (index >= 0 && index < playerCards.Length) 
        {
            playerCards[index] = card;
        }
        else
        {
            Debug.LogError("Index out of range");
        }

        // If this is the last card, notify that the hand is complete.
        if (index == playerCards.Length - 1)
        {
            UIPlayerHandManager.Instance.InitialDisplayCards(playerCards);
        }

    }

    
    public void EvaluateCardInHand() 
    {
        /*
        if (playerCards.Length == 13) // check correct number of cards
        {
            backHand.SetCards(playerCards.Take(5).ToArray()); // first 5 cards to back hand
            midHand.SetCards(playerCards.Skip(5).Take(5).ToArray()); // Next 5 cards to mid hand
            frontHand.SetCards(playerCards.Skip(10).Take(3).ToArray()); // Remaining 3 cards to front hand
        }

        
        */

        handEvaluator.EvaluateHand(playerCards.ToList());

    }

    // Get the cards in the player hand
    // Would be useful for sorting cards
    public CardModel[] GetPlayerCards() 
    {
        return playerCards;
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }
}
