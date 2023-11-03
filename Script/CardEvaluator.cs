using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GlobalDefine;

/// <summary>
/// A class responsible for evaluating and managing selected cards for submission.
/// Only Human Player can have this component
/// </summary>
public class CardEvaluator : MonoBehaviour
{
    public static CardEvaluator Instance { get; private set; }
    public List<CardModel> SelectedCards { get; private set; } = new List<CardModel>();
    private List<Tuple<HandRank, List<CardModel>, int>> RankedHands = new List<Tuple<HandRank, List<CardModel>, int>>();

    public Big2CardSubmissionCheck PlayerSubmissionCheck;

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

    #region Selecting & Deselecting Cards

    /// <summary>
    /// Register a card for evaluation.
    /// </summary>
    public void RegisterCard(CardModel card)
    {
        if (!SelectedCards.Any(c => c.Equals(card)))
        {
            SelectedCards.Add(card);
        }
        else
        {
            Debug.LogWarning("Attempted to register a card that is already registered: " + card.ToString());
        }

        PlayerSubmissionCheck.SubmissionCheck(SelectedCards);
    }

    /// <summary>
    /// Deregister a card from evaluation.
    /// </summary>
    public void DeregisterCard(CardModel card)
    {
        var foundCard = SelectedCards.FirstOrDefault(c => c.Equals(card));
        if (foundCard != null)
        {
            SelectedCards.Remove(foundCard);
            PlayerSubmissionCheck.SubmissionCheck(SelectedCards);
        }
        else
        {
            Debug.LogWarning("Attempted to deregister a card that isn't registered: " + card.ToString());
        }
    }

    /// <summary>
    /// Deregister a list of cards from evaluation.
    /// </summary>
    public void DeregisterCards(List<CardModel> cardsToRemove)
    {
        SelectedCards.RemoveAll(card => cardsToRemove.Contains(card));
        PlayerSubmissionCheck.SubmissionCheck(SelectedCards);
    }

    #endregion

    #region Helper

    /// <summary>
    /// Initialize the CardEvaluator with a submission check.
    /// </summary>
    public void InitializeCardEvaluator(Big2CardSubmissionCheck submissionCheck)
    {
        PlayerSubmissionCheck = submissionCheck;
    }

    #endregion
}
