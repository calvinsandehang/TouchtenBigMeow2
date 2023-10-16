using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckModel : MonoBehaviour
{
    private List<CardModel> deck;

    // constructor that takes a DeckSO and create CardModels for each CardSO
    public DeckModel(DeckSO deckSO) 
    {
        deck = new List<CardModel>();
        foreach (var cardSO in deckSO.Cards)
        {
            deck.Add(cardSO.CreateNewCardModel());
        }
    }

    // Method to shuffle the deck
    public void Shuffle() 
    {
        // simple Knuth shuffle algorithm
        for (int currentIndex = 0; currentIndex < deck.Count; currentIndex++) 
        {
            int randomIndex = Random.Range(currentIndex, deck.Count);
            CardModel tempCard = deck[currentIndex];
            
            deck[currentIndex] = deck[randomIndex];
            deck[randomIndex] = tempCard;

        }
    }

    // Method to draw the top card from the deck
    // Returns null if the dec is empty
    public CardModel DrawCard() 
    {
        if (deck.Count > 0) 
        {
            CardModel topCard = deck[0];
            deck.RemoveAt(0); // remove the card model from the deck
            return topCard;
        }

        return null;
    }

    public List<CardModel> GetCurrentDeck() 
    {
        return deck;
    }

    // Method to get the current number of cards in the deck
    public int CardRemaining() 
    {
        return deck.Count;
    }
}
