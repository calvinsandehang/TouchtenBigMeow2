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
    private UIPlayerHandManager uiPlayerHandManager;
    private Big2CardSubmissionCheck cardSubmissionCheck;

    private bool inFirstRound;
    private bool hasThreeOfDiamonds;

    public static event Action<Big2PlayerHand> OnPlayerLastCardIsDropped;
    public static event Action OnPlayerCardLessThanSix;

    private void Awake()
    {
        ParameterInitialization();        
       
        SubscribeEvent();
    }

    private void Start()
    {
        ComponentInitialization();

    }
    private void ComponentInitialization()
    {
        if (PlayerType == PlayerType.Human) 
        {
            CardEvaluator cardEvaluator = this.gameObject.AddComponent<CardEvaluator>();
            cardEvaluator.InitializeCardEvaluator(cardSubmissionCheck);
        }        
    }

    private void SubscribeEvent() 
    {
        //dealer.OnDealerFinishDealingCards += EvaluateCardInHand; testing
        OnPlayerLastCardIsDropped += ResetPlayerCard;
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
        Debug.Log("Initialize Player : " + index);  
    }

    // temporary
    private void ResetPlayerCard(Big2PlayerHand playerHand) 
    {
        playerCards.Clear();
    }

    public void RemoveCards(List<CardModel> removedCards)
    {
        // Create a HashSet of cards to be removed based on their rank and suit
        HashSet<CardModel> cardsToRemove = new HashSet<CardModel>(removedCards);

        // Remove the cards from playerCards that match the criteria
        playerCards.RemoveAll(card => cardsToRemove.Contains(card));

        UIPlayerHandManager.Instance.DisplayCards(playerCards, PlayerID);
        // Notify UI to update the displayed cards
        if (PlayerType == PlayerType.Human) 
        {
            CardEvaluator.Instance.DeregisterCard(removedCards);
            NotifyObserver(playerCards, PlayerID);
        }

        CheckCardBelowSix();
        CheckWinningCondition();
    }

    private void CheckCardBelowSix()
    {
        if (playerCards.Count < 6) 
        {
            OnPlayerCardLessThanSix?.Invoke();
        }
    }

    private void CheckWinningCondition()
    {
        if (playerCards.Count == 0)
        {
            Debug.Log($"Player {PlayerID} drop his last card");
            OnPlayerLastCardIsDropped?.Invoke(this);
        }
    }


    #region Helper
    private void ParameterInitialization()
    {
        gameMaster = Big2GMStateMachine.Instance;
        cardSubmissionCheck = GetComponent<Big2CardSubmissionCheck>();
        // Injecting this instance to the UIPlayerHandManager
        if (PlayerType == PlayerType.Human)
        {
            //uiPlayerHandManager = UIPlayerHandManager.Instance;
            //uiPlayerHandManager.InitialializedPlayerHand(this);
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
