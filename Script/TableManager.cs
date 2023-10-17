using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

public class TableManager : SubjectTable
{
    [SerializeField]
    private Button _submitCard;

    public static TableManager Instance { get; private set; }
    public ObservableList<CardModel> tableCardModel { get; set; } = new ObservableList<CardModel>(new List<CardModel>());
    public List<CardModel> SelectedCardModel = new List<CardModel>();
    public TableState CurrentTableState = TableState.Single;
    public HandRank CurrentTableHandRank = HandRank.None;
    private CardEvaluator cardEvaluator;

    private void OnTableCardModelChanged(List<CardModel> newCardModels) 
    {
        NotifyObserverAssigningCard(newCardModels);
    }
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

        tableCardModel.ListChanged += OnTableCardModelChanged;

        //_submitCard.onClick.AddListener(ReceivePlayerCard);
    }

    private void Start()
    {
        NotifyObserverTableState(CurrentTableState, CurrentTableHandRank);
        CardEvaluator.OnSelectedCard += AssignTableCards;
    }
    private void OnDestroy()
    {
        // Unsubscribe from the event when the script is destroyed
        CardEvaluator.OnSelectedCard -= AssignTableCards;
    }

    private void ReceivePlayerCard() 
    {
        
    }

    public void AssignTableState(TableState tableState, HandRank tableRank) 
    {
        CurrentTableState = tableState;
        NotifyObserverTableState(tableState, tableRank);
    }

    public void AssignTableCards(List<CardModel> submittedCards)
    {
        tableCardModel.Clear();
        tableCardModel.AddRange(submittedCards);
        //NotifyObserverAssigningCard(submittedCards);
    }

    public void SubmitCards(List<CardModel> submittedCards, HandRank CardRank) 
    {
        CheckTableState(submittedCards);
    }

    public TableState CheckTableState(List<CardModel> cardsOnTable)
    {
        int cardCount = cardsOnTable.Count;
        switch (cardCount)
        {
            case 0:
                return TableState.None;
            case 1:
                return TableState.Single;
            case 2:
                return TableState.Pair;
            case 3:
                return TableState.ThreeOfAKind;
            case 4:
                return TableState.FiveCards;
            default:
                return TableState.None;
        }
    }    

    public bool CanSubmitCards(List<CardModel> selectedCards, HandRank CardRank)
    {
        switch (CurrentTableState)
        {
            case TableState.Single:
                return ValidateSingle(selectedCards);
            case TableState.Pair:
                return ValidatePair(selectedCards);
            case TableState.ThreeOfAKind:
                return ValidateThreeOfAKind(selectedCards);
            case TableState.FiveCards:
                return ValidateFiveCards(selectedCards, CardRank);
            default:
                return false;
        }
    }

    private bool ValidateSingle(List<CardModel> selectedCards)
    {
        if (selectedCards.Count != 1) return false;

        CardModel tableCard = tableCardModel.List[0]; // Access the underlying List<T>
        CardModel selectedCard = selectedCards[0];

        if (selectedCard.CardRank > tableCard.CardRank) return true;
        if (selectedCard.CardRank == tableCard.CardRank && selectedCard.CardSuit > tableCard.CardSuit) return true;

        return false;
    }

    private bool ValidatePair(List<CardModel> selectedCards)
    {
        if (selectedCards.Count != 2) return false;

        selectedCards.Sort((x, y) => y.CardRank.CompareTo(x.CardRank));
        tableCardModel.List.Sort((x, y) => y.CardRank.CompareTo(x.CardRank)); // Access the underlying List<T>

        if (selectedCards[0].CardRank > tableCardModel.List[0].CardRank) return true;
        if (selectedCards[0].CardRank == tableCardModel.List[0].CardRank && selectedCards[0].CardSuit > tableCardModel.List[0].CardSuit) return true;

        return false;
    }


    private bool ValidateThreeOfAKind(List<CardModel> selectedCards)
    {
        if (selectedCards.Count != 3) return false;
        // Assumption: All 3 cards in ThreeOfAKind are of the same CardRank
        return selectedCards[0].CardRank > tableCardModel.List[0].CardRank;
    }

    private bool ValidateFiveCards(List<CardModel> submittedCards, HandRank CardRank)
    {
        if (submittedCards.Count != 5) return false;
        // Here you would implement logic to check for valid combinations and higher value based on CardRank.
        // This function can get complex since you need to check for straights, flushes, full house, etc.
        // If necessary, you can break down the validation into helper methods for each type.
        return false;
    }




}
