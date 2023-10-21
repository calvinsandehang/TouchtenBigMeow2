using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using static GlobalDefine;

public class Big2SimpleAI : MonoBehaviour
{
    private Big2PlayerHand playerHand;
    private Big2PlayerStateMachine playerStateMachine;
    private Big2CardSubmissionCheck cardSubmissionCheck;
    private Big2PlayerSkipTurnHandler skipTurnHandler;
    private Big2PokerHands pokerHand;
    private Big2CardSorter cardSorter;

    private List<CardModel> aiCards, sortedAiCards;
    public AiCardInfo aiCardInfo;

    private Big2TableManager big2TableManager;
    private CardInfo tableInfo;
    private HandType currentTableHandType;
    private HandRank currentTableHandRank;
    private List<CardModel> currentTableCards;

    public bool IsStartingTheTurn { get; set; }

    private void Awake()
    {
        playerHand = GetComponent<Big2PlayerHand>();
        playerStateMachine = GetComponent<Big2PlayerStateMachine>();
        cardSubmissionCheck = GetComponent<Big2CardSubmissionCheck>();
        skipTurnHandler = GetComponent<Big2PlayerSkipTurnHandler>();

        pokerHand = new Big2PokerHands();
        cardSorter = new Big2CardSorter();
    }

    private void Start()
    {
        big2TableManager = Big2TableManager.Instance;
    }

    public void InitiateAiDecisionMaking()
    {
        // reference the owned cards

        aiCards = playerHand.GetPlayerCards();
        // sort the hand by best hand
        // make a list of possible hand rank and the corresponding card
        aiCardInfo = cardSorter.SortPlayerHandByBestHand(aiCards);

        // check if it is higher/suittable for the table
        Big2TableLookUp();

        // IF THERE IS NO ANOTHER CARD ON THE TABLE
        if (currentTableHandRank == HandRank.None)
        {
            // choose the lowest card
            Big2PokerHands big2PokerHands = new Big2PokerHands();
            CardInfo lowestHandInfo = big2PokerHands.GetLowestHand(aiCards);
            List<CardModel> lowestHandCards = lowestHandInfo.CardComposition;
            OnSubmitCard(lowestHandInfo, lowestHandCards);
        }
        // IF THERE IS ANOTHER CARD ON THE TABLE
        else
        {
            for (int i = 0; i < aiCardInfo.CardPackages.Count; i++)
            {
                var cardPackage = aiCardInfo.CardPackages[i];
                var cardPackageComposition = cardPackage.CardPackageContent;
                Debug.Log($"Card Package Type: {cardPackage.CardPackageType}");
                Debug.Log($"Card Package Rank: {cardPackage.CardPackageRank}");

                if (!CompareHandType(cardPackageComposition) && currentTableHandType != HandType.None)
                {
                    //NotAllowedToSubmitCard?.Invoke();
                    Debug.Log("HandType mismatch");
                    continue;
                }

                // Check if the hand rank of the selected cards is allowed
                if (!CompareHandRank(cardPackage.CardPackageRank))
                {
                    //NotAllowedToSubmitCard?.Invoke();
                    Debug.Log("Selected card hand rank is lower than the table card / not suitable");
                    continue;
                }

                // Compare the selected cards with the current table cards
                if (!CompareSelectedCardsWithTableCards(cardPackageComposition))
                {
                    //NotAllowedToSubmitCard?.Invoke();
                    Debug.Log("Selected cards value is lower than the table cards");
                    return;
                }

                // card is suitable, submit card
                CardInfo submittedCardInfo = EvaluateSelectedCards(cardPackageComposition);
                OnSubmitCard(submittedCardInfo, cardPackageComposition);
                /*
                for (int j = 0; j < cardPackage.CardPackageContent.Count; j++)
                {
                    var card = cardPackage.CardPackageContent[j];
                    Debug.Log($"Card Package Content {j}: {card}");
                }
                */
            }
        }

        // if not, skip turn
        // if yes, submit card
        // remove the submitted card
    }

    private void Big2TableLookUp()
    {
        tableInfo = big2TableManager.TableLookUp();
        currentTableHandType = tableInfo.HandType;
        currentTableHandRank = tableInfo.HandRank;
        currentTableCards = new List<CardModel>();
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

    private bool CompareSelectedCardsWithTableCards(List<CardModel> bestHandCards)
    {
        Big2CardComparer big2CardComparer = new Big2CardComparer();
        tableInfo = big2TableManager.TableLookUp();
        currentTableCards = tableInfo.CardComposition;

        return big2CardComparer.CompareHands(bestHandCards, currentTableCards);
    }

    private CardInfo EvaluateSelectedCards(List<CardModel> selectedCard)
    {
        Big2PokerHands checkSelectedCard = new Big2PokerHands();
        var bestHand = checkSelectedCard.GetBestHand(selectedCard);
        var selectedCardHandType = bestHand.HandType;
        var selectedCardHandRank = bestHand.HandRank;
        var bestHandCards = bestHand.CardComposition;
        return new CardInfo(selectedCardHandType, selectedCardHandRank, bestHandCards);
    }

    private void OnSubmitCard(CardInfo submittedCardInfo, List<CardModel> submittedCards)
    {
        Debug.Log("OnSubmitCard");
        Big2TableManager.Instance.UpdateTableCards(submittedCardInfo);
        playerHand.RemoveCards(submittedCards);

        //NotAllowedToSubmitCard?.Invoke();
    }
}
