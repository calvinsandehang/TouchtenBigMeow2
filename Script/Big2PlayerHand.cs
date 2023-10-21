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
    public int PlayerID;

    private List<CardModel> playerCards = new List<CardModel>();
    private const int playerHandSize = 13;

    private Big2GMStateMachine gameMaster;
    private PlayerHandEvaluator handEvaluator;
    private UIPlayerHandManager uiPlayerHandManager;

    private bool inFirstRound;
    private bool hasThreeOfDiamonds;

    public event Action OnHandLastCardIsDropped;

    private void Awake()
    {
        ParameterInitialization();
       
        SubscribeEvent();
    }

    private void SubscribeEvent() 
    {
        //dealer.OnDealerFinishDealingCards += EvaluateCardInHand; testing
    }

    private void UnsubscribeEvent()
    {
        //dealer.OnDealerFinishDealingCards -= EvaluateCardInHand; testing
    }

    public void AddCard(CardModel card) 
    {
        playerCards.Add(card);

        // If this is the last card, notify that the hand is complete.
        //if (PlayerType == PlayerType.Human)
            UIPlayerHandManager.Instance.DisplayCards(playerCards, PlayerID);

        inFirstRound = gameMaster.CheckGameInFirstRound();

        if (gameMaster.CheckGameInFirstRound()) 
        {
            if (card.CardRank == Rank.Three && card.CardSuit == Suit.Diamonds)
                hasThreeOfDiamonds = true;    
        }
    }

    public void InitializePlayerID(int index) 
    {
        PlayerID = index;
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
            UIPlayerHandManager.Instance.DisplayCards(playerCards, PlayerID);
            CardEvaluator.Instance.DeregisterCard(removedCards);
            NotifyObserver(playerCards, PlayerID);
        }

        CheckWinningCondition();
    }

    private void CheckWinningCondition()
    {
        if (playerCards.Count == 0)
        {
            OnHandLastCardIsDropped?.Invoke();
        }
    }


    #region Helper
    private void ParameterInitialization()
    {
        handEvaluator = GetComponent<PlayerHandEvaluator>();
        gameMaster = Big2GMStateMachine.Instance;        

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


    // Get the cards in the player hand
    // Would be useful for sorting cards
    #region Look Up & Properties
    

    public bool CheckHavingThreeOfDiamonds() 
    {
        return hasThreeOfDiamonds;
    }

    public List<CardModel> GetPlayerCards() 
    {
        return playerCards;
    }
    #endregion


    private void OnDisable()
    {
        UnsubscribeEvent();
    }

    #region Testing Purposes
    public void EvaluateCardInHand()
    {
        //handEvaluator.EvaluateHand(playerCards.ToList());
    }
    #endregion
}
