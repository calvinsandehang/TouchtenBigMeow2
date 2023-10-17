using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using TMPro;
using UnityEngine;
using static GlobalDefine;

public class CardEvaluator : SubjectCardEvaluator
{
    public static CardEvaluator Instance { get;  private set; }
    public List<CardModel> selectedCards = new List<CardModel>();
    private List<Tuple<HandRank, List<CardModel>, int>> rankedHands = new List<Tuple<HandRank, List<CardModel>, int>>();

    [SerializeField]
    private TextMeshProUGUI _text;

    public delegate void CardEvaluation (List<CardModel> evaluatedCard);
    public static event CardEvaluation OnSelectedCard;
    public static event CardEvaluation OnSingleCardEvaluated;
    public static event CardEvaluation OnPairCardEvaluated;
    public static event CardEvaluation OnThreeOfAKindCardEvaluated;
    public static event CardEvaluation OnFiveCardsEvaluated;

    // Define a dictionary to map TableState to events
    private Dictionary<TableState, CardEvaluation> stateToEvent = new Dictionary<TableState, CardEvaluation>();

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

        InitializeDictionary();
    }   

    #region Selecting & Deselecting card
    public void RegisterCard(CardModel card)
    {
        Debug.Log("Register Card : " + card.CardRank +" "+ card.CardSuit);
        // Check if a card with the same properties is already in the list
        if (!selectedCards.Any(c => c.Equals(card)))
        {
            selectedCards.Add(card);

            // For testing
            DisplayHandCombination(selectedCards);
        }
        else
        {
            Debug.LogWarning("Attempted to register a card that is already registered: " + card.ToString());
        }

        NotifyObserverAboutEvaluatedCard(selectedCards);
    }

    public void DeregisterCard(CardModel card)
    {

        // Find the card in the list that matches the specific properties
        var foundCard = selectedCards.FirstOrDefault(c => c.Equals(card));
        if (foundCard != null)
        {
            selectedCards.Remove(foundCard);

            if (selectedCards.Count <= 0)
                return;
            DisplayHandCombination(selectedCards);
        }
        else
        {
            Debug.LogWarning("Attempted to deregister a card that isn't registered: " + card.ToString());
        }

        NotifyObserverAboutEvaluatedCard(selectedCards);
        CheckDisplayNothing();
    }

    public void DeregisterCard(List<CardModel> cardsToRemove)
    {
        // Remove all cards in cardsToRemove from selectedCards
        selectedCards.RemoveAll(card => cardsToRemove.Contains(card));

        // Check if there are any cards left in selectedCards
        if (selectedCards.Count <= 0)
        {
            //DisplayHandCombination(selectedCards);
            //NotifyObserverAboutEvaluatedCard(selectedCards);
            CheckDisplayNothing();
        }
        else
        {
            DisplayHandCombination(selectedCards);
            NotifyObserverAboutEvaluatedCard(selectedCards);
        }
    }


    private void CheckDisplayNothing()
    {
        if (selectedCards.Count == 0)
        {
            _text.text = "";
        }
    }
    #endregion

    private void DisplayHandCombination(List<CardModel> selectedCards)
    {
        var result = EvaluateHand(selectedCards);
        var item2 = result.Item2.ToString();
        var item3 = string.Join(", ", result.Item3);
        var item4 = result.Item4.ToString();
       
        string evaluationText = $"Rank : {item2}, Cards: {item3} ,Points: {item4}";

        _text.text = evaluationText;
    }

    public Tuple<TableState, HandRank, List<CardModel>, int> EvaluateHand(List<CardModel> cards)
    {
        Big2PokerHands pokerHandsChecker = new Big2PokerHands();
        var bestHand = pokerHandsChecker.GetBestHand(cards);

        //TableState playerCardTbState = CheckPlayerCardTableState(bestHand);
        //TableState tableManagerTbState = TableManager.Instance.CurrentTableState;



        return bestHand;
    }

    private TableState CheckPlayerCardTableState(Tuple<TableState, HandRank, List<CardModel>, int> bestHand)
    {
        TableState currentCombinationTableState = bestHand.Item1;

        if (stateToEvent.ContainsKey(currentCombinationTableState))
        {
            stateToEvent[currentCombinationTableState]?.Invoke(bestHand.Item3);
        }

        return currentCombinationTableState;
    }

    #region Helper
    private void InitializeDictionary()
    {
        // Initialize the dictionary with mappings
        stateToEvent[TableState.Single] = OnSingleCardEvaluated;
        stateToEvent[TableState.Pair] = OnPairCardEvaluated;
        stateToEvent[TableState.ThreeOfAKind] = OnThreeOfAKindCardEvaluated;
        stateToEvent[TableState.FiveCards] = OnFiveCardsEvaluated;
    }
    #endregion
}
