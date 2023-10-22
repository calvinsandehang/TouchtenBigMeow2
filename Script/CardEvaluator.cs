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
    
    /*
    public delegate void CardEvaluation (List<CardModel> evaluatedCard);
    public static event CardEvaluation OnSingleCardEvaluated;
    public static event CardEvaluation OnPairCardEvaluated;
    public static event CardEvaluation OnThreeOfAKindCardEvaluated;
    public static event CardEvaluation OnFiveCardsEvaluated;
    */

    // Define a dictionary to map TableState to events
    //private Dictionary<HandType, CardEvaluation> stateToEvent = new Dictionary<HandType, CardEvaluation>();

    public Big2CardSubmissionCheck playerSubmissionCheck; 

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

        //InitializeDictionary();
    }
  
   
    #region Selecting & Deselecting card
    public void RegisterCard(CardModel card)
    {
        //Debug.Log("Register Card : " + card.CardRank +" "+ card.CardSuit);
        // Check if a card with the same properties is already in the list
        if (!selectedCards.Any(c => c.Equals(card)))
        {
            selectedCards.Add(card);
        }
        else
        {
            Debug.LogWarning("Attempted to register a card that is already registered: " + card.ToString());
        }

        playerSubmissionCheck.SubmissionCheck(selectedCards);
    }

    public void DeregisterCard(CardModel card)
    {
        //Debug.Log("DeregisterCard");

        // Find the card in the list that matches the specific properties
        var foundCard = selectedCards.FirstOrDefault(c => c.Equals(card));
        if (foundCard != null)
        {
            selectedCards.Remove(foundCard);

            playerSubmissionCheck.SubmissionCheck(selectedCards);

            // for testing
            //DisplayHandCombination(selectedCards); 
        }
        else
        {
            Debug.LogWarning("Attempted to deregister a card that isn't registered: " + card.ToString());
        }
        
        // for testing
        //CheckDisplayNothing();
    }

    public void DeregisterCard(List<CardModel> cardsToRemove)
    {
        // Remove all cards in cardsToRemove from selectedCards
        selectedCards.RemoveAll(card => cardsToRemove.Contains(card));

        // Check if there are any cards left in selectedCards
        if (selectedCards.Count <= 0)
        {
            
            playerSubmissionCheck.SubmissionCheck(selectedCards);
            // for testing
            //CheckDisplayNothing();
            //DisplayHandCombination(selectedCards); 
        }
        else
        {
            playerSubmissionCheck.SubmissionCheck(selectedCards);
            // for testing
            //DisplayHandCombination(selectedCards); //testing
        }
    }

    

    #endregion

    #region Testing
    /*

    [SerializeField]
    private TextMeshProUGUI _text;

    private void CheckDisplayNothing()
    {
        if (selectedCards.Count == 0)
        {
            _text.text = "";
        }
    }
    private void DisplayHandCombination(List<CardModel> selectedCards)
    {
        var result = EvaluateHand(selectedCards);
        var handRank = result.HandRank.ToString();
        var cardComposition = string.Join(", ", result.CardComposition);
       
        string evaluationText = $"Rank : {handRank}, Cards: {cardComposition}";

        _text.text = evaluationText;
    }
    */
    #endregion

    public CardInfo EvaluateHand(List<CardModel> cards)
    {
        Big2PokerHands pokerHandsChecker = new Big2PokerHands();
        var cardInfo = pokerHandsChecker.GetBestHand(cards);

        return cardInfo;
    }

    private HandType CheckPlayerCardTableState(Tuple<HandType, HandRank, List<CardModel>, int> bestHand)
    {
        HandType currentCombinationTableState = bestHand.Item1;

        /*
        if (stateToEvent.ContainsKey(currentCombinationTableState))
        {
            stateToEvent[currentCombinationTableState]?.Invoke(bestHand.Item3);
        }
        */

        return currentCombinationTableState;
    }

    #region Helper
    public void InitializeCardEvaluator(Big2CardSubmissionCheck submissionCheck) 
    {
        playerSubmissionCheck = submissionCheck;
    }
    /*
    private void InitializeDictionary()
    {
        // Initialize the dictionary with mappings
        stateToEvent[HandType.Single] = OnSingleCardEvaluated;
        stateToEvent[HandType.Pair] = OnPairCardEvaluated;
        stateToEvent[HandType.ThreeOfAKind] = OnThreeOfAKindCardEvaluated;
        stateToEvent[HandType.FiveCards] = OnFiveCardsEvaluated;
    }
    */
    #endregion
}
