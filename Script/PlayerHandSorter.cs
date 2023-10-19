using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static GlobalDefine;

public class PlayerHandSorter
{
    public void SortPlayerHandByRank(List<GameObject> cardsObjectsInPlayerHand)
    {
        cardsObjectsInPlayerHand.Sort((card1, card2) =>
        {
            UISelectableCard selectableCard1 = card1.GetComponent<UISelectableCard>();
            UISelectableCard selectableCard2 = card2.GetComponent<UISelectableCard>();

            CardModel cardModel1 = selectableCard1.GetCardModel();
            CardModel cardModel2 = selectableCard2.GetCardModel();

            return cardModel1.CardRank.CompareTo(cardModel2.CardRank);
        });
        UpdateCardPositions(cardsObjectsInPlayerHand);
    }

    public void SortPlayerHandBySuit(List<GameObject> cardsObjectsInPlayerHand)
    {
        cardsObjectsInPlayerHand.Sort((card1, card2) =>
        {
            UISelectableCard selectableCard1 = card1.GetComponent<UISelectableCard>();
            UISelectableCard selectableCard2 = card2.GetComponent<UISelectableCard>();

            CardModel cardModel1 = selectableCard1.GetCardModel();
            CardModel cardModel2 = selectableCard2.GetCardModel();

            return cardModel1.CardSuit.CompareTo(cardModel2.CardSuit);
        });
        UpdateCardPositions(cardsObjectsInPlayerHand);
    }

    public void SortPlayerHandByBestHand(List<GameObject> cardsObjectsInPlayerHand, CardPool cardPool, Transform cardParent)
    {
        List<CardModel> cardModelsInPlayerHand = new List<CardModel>();

        for (int i = 0; i < cardsObjectsInPlayerHand.Count; i++)
        {
            UISelectableCard selectableCard = cardsObjectsInPlayerHand[i].GetComponent<UISelectableCard>();
            CardModel card = selectableCard.GetCardModel();
            cardModelsInPlayerHand.Add(card);
        }

        Big2PokerHands pokerHandsChecker = new Big2PokerHands();
        List<CardModel> tempCardModels = new List<CardModel>(cardModelsInPlayerHand);
        List<CardModel> sortedCardModels = new List<CardModel>();

        while (tempCardModels.Count > 0)
        {
            // Get the best hand from the remaining cards
            CardInfo cardInfo = pokerHandsChecker.GetBestHand(tempCardModels);
            List<CardModel> bestHandCards = cardInfo.CardComposition;

            // Log for debugging
            //Debug.Log("Best hand rank: " + cardInfo.HandRank.ToString());
            foreach (var card in bestHandCards)
            {
                Debug.Log("Card in best hand: " + card.CardRank.ToString() + " of " + card.CardSuit.ToString());
            }

            // Add the best hand cards to the sorted list
            sortedCardModels.AddRange(bestHandCards);

            // Remove the best hand cards from tempCardModels for the next iteration
            foreach (var card in bestHandCards)
            {
                tempCardModels.Remove(card);
            }
        }

       

        // Update the player's hand with the sorted cards and reflect this in the UI
        cardModelsInPlayerHand = new List<CardModel>(sortedCardModels); // Replace the list, don't clear and re-add
        cardModelsInPlayerHand.Reverse();

        UpdateCardPosition(cardsObjectsInPlayerHand, cardModelsInPlayerHand, cardPool, cardParent);
    }

    private void UpdateCardPosition(List<GameObject> cardsObjectsInPlayerHand, List<CardModel> cardModelsInPlayerHand, CardPool cardPool, Transform cardParent)
    {
        // Clear out the old cards.
        while (cardsObjectsInPlayerHand.Count > 0)
        {
            var card = cardsObjectsInPlayerHand[0];
            cardsObjectsInPlayerHand.RemoveAt(0);
            cardPool.ReturnCard(card);
        }

        for (int i = 0; i < cardModelsInPlayerHand.Count; i++)
        {
            GameObject cardGO = cardPool.GetCard();
            cardGO.transform.SetParent(cardParent, false);
            cardGO.transform.SetSiblingIndex(i);
            UISelectableCard selectableCard = cardGO.GetComponent<UISelectableCard>();
            selectableCard.Initialize(cardModelsInPlayerHand[i]); // Adjust this if necessary to match CardModel structure.
            cardsObjectsInPlayerHand.Add(cardGO);
        }
    }

    private void UpdateCardPositions(List<GameObject> cardGameObjects)
    {
        for (int i = 0; i < cardGameObjects.Count; i++)
        {
            // This is a placeholder for whatever logic you use to position your cards in the scene.
            // You may set their position, change their order in the hierarchy, etc.
            cardGameObjects[i].transform.SetSiblingIndex(i);
        }
    }
}
