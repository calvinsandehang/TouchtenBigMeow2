using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public static PlayerHand Instance { get; private set; }
    public List<CardModel> playerCards = new List<CardModel>();
    private const int playerHandSize = 13;

    private Dealer dealer;
    private PlayerHandEvaluator handEvaluator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

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

    public void AddCard(CardModel card) 
    {
        playerCards.Add(card);

        // If this is the last card, notify that the hand is complete.
        UIPlayerHandManager.Instance.InitialDisplayCards(playerCards);
    }

    public void RemoveCards(List<CardModel> removedCards)
    {
        // Create a HashSet of cards to be removed based on their rank and suit
        HashSet<CardModel> cardsToRemove = new HashSet<CardModel>(removedCards);

        // Remove the cards from playerCards that match the criteria
        playerCards.RemoveAll(card => cardsToRemove.Contains(card));

        // Notify UI to update the displayed cards
        UIPlayerHandManager.Instance.InitialDisplayCards(playerCards);
        CardEvaluator.Instance.DeregisterCard(removedCards);
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
    public List<CardModel> GetPlayerCards() 
    {
        return playerCards;
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }
}
