using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

[InfoBox("Bridge between Player and Table")]
public class PlayerSubmissionCheck : MonoBehaviour, IObserverTable, IObserverCardEvaluator
{
    public static PlayerSubmissionCheck Instance { get; private set; }
    public TableState currentTableState;
    private HandRank currentTableHandRank;
    private List<CardModel> currentTableCards = new List<CardModel>();
    private List<CardModel> submittedCards = new List<CardModel>();
    private bool matchingTableState;

    public delegate void CardSubmission();

    public static event CardSubmission AllowedToSubmitCard;
    public static event CardSubmission NotAllowedToSubmitCard;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // Register this script to be notified of changes in CardEvaluator and Table
        AddSelfToSubjectList();
    }

    public void OnSubmitCard() 
    {
        //TableManager.Instance.tableCardModel.Clear();
        TableManager.Instance.tableCardModel.AddRange(submittedCards);


        PlayerHand.Instance.RemoveCards(submittedCards);
    }

    public void OnNotifySelectedCards(List<CardModel> selectedCard)
    {
        submittedCards.Clear();
        Big2PokerHands checkSelectedCard = new Big2PokerHands();
        var BestHand = checkSelectedCard.GetBestHand(selectedCard);

        TableState selectedCardTableState = BestHand.Item1;

        if (!CompareTableState(selectedCardTableState)) 
        {
            Debug.Log("TableState mismatch");
            return; // return when TableState mismatch
        }

        HandRank selectedCardHandRank = BestHand.Item2;

        if (!CompareHandRank(selectedCardHandRank))
        {
            Debug.Log("Selected card hand rank is lower than the table card / not suitable");
            return; // return when hand rank is lower or not suitable
        }

        // TO DO: Check value depending on the card combination
        List<CardModel> bestHandCards = BestHand.Item3;

        if (!CompareSelectedCardPoints(bestHandCards))
        {
            Debug.Log("Selected cards value is lower than the table cards");
            return;
        }

        submittedCards.AddRange(bestHandCards);
        AllowedToSubmitCard?.Invoke();
    }

    private bool CompareSelectedCardPoints(List<CardModel> bestHandCards)
    {
        switch (currentTableHandRank)
        {
            case HandRank.None:
                return true;
            case HandRank.HighCard:
                return CompareHighCard(bestHandCards);
                
            case HandRank.Pair:
                return false;
                
            case HandRank.ThreeOfAKind:

                return false;
            case HandRank.Straight:

                return false;
            case HandRank.Flush:

                return false;
            case HandRank.FullHouse:

                return false;
            case HandRank.FourOfAKind:

                return false;
            case HandRank.StraightFlush:

                return false;
        }
        return false;
    }

    #region Compare Card Value
    private bool CompareHighCard(List<CardModel> bestHandCards)
    {
        if (currentTableCards.Count == 0)
            return true; // table is empty, no need to compare

        // Sort both the best hand cards and current table cards in descending order of rank
        bestHandCards.Sort((a, b) => (int)b.CardRank - (int)a.CardRank);
        currentTableCards.Sort((a, b) => (int)b.CardRank - (int)a.CardRank);

        for (int i = 0; i < bestHandCards.Count; i++)
        {
            // Compare the ranks of the corresponding cards in both hands
            int comparisonResult = bestHandCards[i].CardRank.CompareTo(currentTableCards[i].CardRank);

            if (comparisonResult > 0)
            {
                // The best hand card has a higher rank than the corresponding table card
                // Handle the case where the best hand wins this round
                Debug.Log($"Best hand card {bestHandCards[i].CardRank} beats table card {currentTableCards[i].CardRank}");
                return true;
            }
            else if (comparisonResult < 0)
            {
                // The table card has a higher rank than the corresponding best hand card
                // Handle the case where the table card wins this round
                Debug.Log($"Table card {currentTableCards[i].CardRank} beats best hand card {bestHandCards[i].CardRank}");
                return false;
            }
            else
            {
                // Both cards have the same rank
                // Handle ties or additional comparisons (e.g., suit)
                Debug.Log($"Both cards have the same rank: {bestHandCards[i].CardRank}");
                return false;
            }
        }
        return false;

        // You can add more logic as needed for additional comparisons or handling ties.
    }
    #endregion

    private bool CompareHandRank(HandRank selectedCardHandRank)
    {
        // Check if the selected card's hand rank is equal or higher than the table's hand rank
        if (selectedCardHandRank >= currentTableHandRank)
        {
            switch (currentTableHandRank)
            {
                case HandRank.None:
                case HandRank.HighCard:
                    // Allow any hand rank that is equal or higher
                    return true;
                case HandRank.Pair:
                    // Allow any hand rank that is equal or higher, except HighCard
                    return selectedCardHandRank != HandRank.HighCard;
                case HandRank.ThreeOfAKind:
                    // Allow any hand rank that is equal or higher, except HighCard and Pair
                    return selectedCardHandRank != HandRank.HighCard && selectedCardHandRank != HandRank.Pair;
                case HandRank.Straight:
                case HandRank.Flush:
                case HandRank.FullHouse:
                case HandRank.FourOfAKind:
                case HandRank.StraightFlush:
                    // Allow only the same or higher hand rank
                    return selectedCardHandRank >= currentTableHandRank;
            }
        }

        return false;
    }

    private bool CompareTableState(TableState selectedCardTableState)
    {
        if (selectedCardTableState == currentTableState)
            return true;
        else
            return false;
    }

    #region Observer Pattern
    public void AddSelfToSubjectList()
    {
        // Assuming both TableManager and CardEvaluator have lists of observers
        TableManager.Instance?.AddObserver(this);
        CardEvaluator.Instance?.AddObserver(this);
    }

    public void RemoveSelfToSubjectList()
    {
        TableManager.Instance?.RemoveObserver(this);
        CardEvaluator.Instance?.RemoveObserver(this);
    }
    #endregion

    #region Helper
    private void OnDestroy()
    {
        RemoveSelfToSubjectList();
    }

    private void OnDisable()
    {
        RemoveSelfToSubjectList();  
    }

    public void OnNotifyTableState(TableState tableState, HandRank tableRank)
    {
        currentTableState = tableState;
        currentTableHandRank = tableRank;
    }

    public void OnNotifyAssigningCard(List<CardModel> cardModels)
    {
        
    }
    #endregion

}
