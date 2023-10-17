using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using TMPro;
using UnityEngine;
using static GlobalDefine;

public class CardEvaluator : MonoBehaviour, IObserverCard
{
    public static CardEvaluator Instance { get;  private set; }
    public List<CardModel> selectedCards = new List<CardModel>();
    private List<Tuple<HandRank, List<CardModel>, int>> rankedHands = new List<Tuple<HandRank, List<CardModel>, int>>();

    [SerializeField]
    private TextMeshProUGUI _text;

    
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
    }

    #region Selecting & Deselecting card
    public void RegisterCard(CardModel card)
    {
        Debug.Log(card.CardRank +" "+ card.CardSuit);
        // Check if a card with the same properties is already in the list
        if (!selectedCards.Any(c => c.Equals(card)))
        {
            selectedCards.Add(card);
            Debug.Log(selectedCards);
            DisplayHandCombination(selectedCards);
        }
        else
        {
            Debug.LogWarning("Attempted to register a card that is already registered: " + card.ToString());
        }
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

        CheckDisplayNothing();
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
        var item1 = result.Item1.ToString();
        var item2 = string.Join(", ", result.Item2);
        var item3 = result.Item3.ToString();
        string evaluationText = $"Rank : {item1}, Cards: {item2} ,Points: {item3}";

        _text.text = evaluationText;
    }

    public Tuple<HandRank, List<CardModel>, int> EvaluateHand(List<CardModel> cards)
    {
        Big2PokerHands pokerHandsChecker = new Big2PokerHands();
        var bestHand = pokerHandsChecker.GetBestHand(cards);
        Debug.Log(cards.Count);
        return bestHand;
    }     
}
