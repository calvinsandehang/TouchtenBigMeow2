using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

[InfoBox("Bridge between Player and Table")]
public class CardSubmissionCheck : MonoBehaviour, IObserverCardEvaluator
{
    public static CardSubmissionCheck Instance { get; private set; }

    private Big2TableManager big2TableManager;
    private CardInfo tableInfo;
    public HandType currentTableHandType;
    private HandRank currentTableHandRank;
    private List<CardModel> currentTableCards;

    private List<CardModel> submittedCards = new List<CardModel>();
    private CardInfo submittedCardInfo;
    private bool matchingHandType;

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
        InitializeBig2TableManager();
    }

    private void InitializeBig2TableManager()
    {
        big2TableManager = Big2TableManager.Instance;
        Big2TableLookUp();
    }

    private void Big2TableLookUp()
    {
        tableInfo = big2TableManager.TableLookUp();
        currentTableHandType = tableInfo.HandType;
        currentTableHandRank = tableInfo.HandRank;
        currentTableCards = new List<CardModel>();
    }

    public void OnSubmitCard() 
    {
        Big2TableManager.Instance.UpdateTableCards(submittedCardInfo);
        PlayerHand.Instance.RemoveCards(submittedCards);
    }

    // Receive selected card from PlayerSelectedCardEvaluator
    public void OnNotifySelectedCards(List<CardModel> selectedCard)
    {
        Big2TableLookUp(); 
        // Check if the hand type of the selected cards is allowed
        if (!CompareHandType(selectedCard) && currentTableHandType != HandType.None)
        {
            NotAllowedToSubmitCard?.Invoke();
            Debug.Log("HandType mismatch");
            return;
        }

        ClearSubmittedCardList();

        // Evaluate the selected cards to determine their hand type and rank
        submittedCardInfo = EvaluateSelectedCards(selectedCard);

        // Check if the hand rank of the selected cards is allowed
        if (!CompareHandRank(submittedCardInfo.HandRank))
        {
            NotAllowedToSubmitCard?.Invoke();
            Debug.Log("Selected card hand rank is lower than the table card / not suitable");
            return;
        }

        // Compare the selected cards with the current table cards
        if (!CompareSelectedCardsWithTableCards(submittedCardInfo.CardComposition))
        {
            NotAllowedToSubmitCard?.Invoke();
            Debug.Log("Selected cards value is lower than the table cards");
            return;
        }

        // If all checks pass, add the selected cards to the submitted cards
        AddNewSubmittedCardToSubmittedCardList();

        AllowedToSubmitCard?.Invoke();
    }

    private void AddNewSubmittedCardToSubmittedCardList()
    {
        submittedCards.AddRange(submittedCardInfo.CardComposition);
    }

    private void ClearSubmittedCardList()
    {
        submittedCards.Clear();
    }

    private CardInfo EvaluateSelectedCards(List<CardModel> selectedCard)
    {
        Big2PokerHands checkSelectedCard = new Big2PokerHands();
        var bestHand = checkSelectedCard.GetBestHand(selectedCard);
        var selectedCardHandType = bestHand.HandType;
        var selectedCardHandRank = bestHand.HandRank;
        var bestHandCards = bestHand.CardComposition;
        return new CardInfo (selectedCardHandType, selectedCardHandRank, bestHandCards);
    }

    private bool CompareSelectedCardsWithTableCards(List<CardModel> bestHandCards)
    {
        Big2CardComparer big2CardComparer = new Big2CardComparer();
        tableInfo = big2TableManager.TableLookUp();
        currentTableCards = tableInfo.CardComposition;

        return big2CardComparer.CompareHands(bestHandCards, currentTableCards);
    }  

    private bool CompareHandRank(HandRank selectedCardHandRank)
    {
        switch (currentTableHandRank)
        {
            case HandRank.None:
                return true;
            case HandRank.HighCard:
                // Allow any hand rank that is equal or higher
                if (selectedCardHandRank == HandRank.HighCard)
                    return true;
                else
                    return false;                
            case HandRank.Pair:
                // Allow any hand rank that is equal or higher, except HighCard
                return selectedCardHandRank == HandRank.Pair;
            case HandRank.ThreeOfAKind:
                // Allow any hand rank that is equal or higher, except HighCard and Pair
                return selectedCardHandRank == HandRank.ThreeOfAKind;
            case HandRank.Straight:
            case HandRank.Flush:
            case HandRank.FullHouse:
            case HandRank.FourOfAKind:
            case HandRank.StraightFlush:
                // Allow only the same or higher hand rank
                return selectedCardHandRank >= currentTableHandRank;
        }

        return false;
    }
    private bool CompareHandType(List<CardModel> submittedCardModels)
    {
        int cardCount = submittedCardModels.Count;

        switch (cardCount)
        {
            case 0:
                return currentTableHandType == HandType.None;
            case 1:
                return currentTableHandType == HandType.Single;
            case 2:
                return currentTableHandType == HandType.Pair;
            case 3:
                return currentTableHandType == HandType.ThreeOfAKind;
            case 5:
                return currentTableHandType == HandType.FiveCards;
            default:
                return false;
        }
    }


    #region Observer Pattern
    public void AddSelfToSubjectList()
    {
        // Assuming both TableManager and CardEvaluator have lists of observers
        CardEvaluator.Instance?.AddObserver(this);
    }

    public void RemoveSelfToSubjectList()
    {
        CardEvaluator.Instance?.RemoveObserver(this);
    }
    #endregion

    #region Helper
    private void OnDestroy()
    {
        RemoveSelfToSubjectList();
    }

    private void OnEnable()
    {
        AddSelfToSubjectList();
    }

    private void OnDisable()
    {
        RemoveSelfToSubjectList();  
    }

    public void OnNotifyHandType(HandType HandType, HandRank tableRank)
    {
        Debug.Log("currentHandType " + HandType);
        currentTableHandType = HandType;
        currentTableHandRank = tableRank;
    }

    public void OnNotifyAssigningCard(List<CardModel> cardModels)
    {
        
    }
    #endregion

}