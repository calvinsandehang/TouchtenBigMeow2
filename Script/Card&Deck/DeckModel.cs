using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Big2Meow.DeckNCard
{
    /// <summary>
    /// Represents a deck of cards.
    /// </summary>
    public class DeckModel
    {
        /// <summary>
        /// Optimization
        /// referenceDeck is used as a reference to avoid repeating the deck initialization process in subsequent games.
        /// </summary>
        private List<CardModel> referenceDeck;
        public List<CardModel> currentDeck;

        /// <summary>
        /// Initializes a new instance of the DeckModel class using a DeckSO.
        /// </summary>
        /// <param name="deckSO">The DeckSO containing card definitions.</param>
        public DeckModel(DeckSO deckSO)
        {
            // Create a reference deck from DeckSO
            referenceDeck = new List<CardModel>();
            foreach (var cardSO in deckSO.Cards)
            {
                referenceDeck.Add(cardSO.CreateNewCardModel());
            }

            /* Debug Card
            // Create a single string that lists all cards in the deck
            string refDeckContent = "Reference deck content: ";
            foreach (var card in referenceDeck)
            {
                refDeckContent += $"{card.ToString()}, ";
            }
            refDeckContent = refDeckContent.TrimEnd(' ', ',');

            // Now print this single string to the console
            Debug.Log(refDeckContent);
            */

            // Set the current deck as the reference deck
            currentDeck = new List<CardModel>(referenceDeck);
        }

        /// <summary>
        /// Shuffles the current deck using the Knuth shuffle algorithm.
        /// </summary>
        public void Shuffle()
        {
            for (int currentIndex = 0; currentIndex < currentDeck.Count; currentIndex++)
            {
                int randomIndex = Random.Range(currentIndex, currentDeck.Count);
                CardModel tempCard = currentDeck[currentIndex];

                currentDeck[currentIndex] = currentDeck[randomIndex];
                currentDeck[randomIndex] = tempCard;
            }
        }

        /// <summary>
        /// Draws the top card from the current deck.
        /// Returns null if the deck is empty.
        /// </summary>
        /// <returns>The top card from the deck.</returns>
        public CardModel DrawCard()
        {
            if (currentDeck.Count > 0)
            {
                CardModel topCard = currentDeck[0];
                currentDeck.RemoveAt(0); // Remove the card model from the deck
                return topCard;
            }

            return null;
        }

        /// <summary>
        /// Gets the current number of cards in the reference deck.
        /// </summary>
        /// <returns>The number of cards in the reference deck.</returns>
        public int CardRemaining()
        {
            return referenceDeck.Count;
        }

        /// <summary>
        /// Resets the current deck to the reference deck.
        /// </summary>
        public void ResetDeck()
        {
            // Clear the current deck, preparing it for repopulation
            currentDeck.Clear();

            // Add all cards back from the reference deck
            foreach (var card in referenceDeck)
            {
                currentDeck.Add(card);
            }

            //Debug.Log("Deck has been reset.");

            /*Debug Card
            // Create a single string that lists all cards in the deck

            string currentDeckContent = "Current deck content: ";
            foreach (var card in currentDeck)
            {
                currentDeckContent += $"{card.ToString()}, ";
            }
            currentDeckContent = currentDeckContent.TrimEnd(' ', ',');

            // Now print this single string to the console
            Debug.Log(currentDeckContent);
            */
        }





    }
}
   
