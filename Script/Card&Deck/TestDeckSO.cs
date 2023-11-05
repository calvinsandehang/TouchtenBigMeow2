using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.DeckNCard
{
    [CreateAssetMenu(fileName = "TestDeck", menuName = "PlayingCard/TestDeck")]
    /// <summary>
    /// Represents a testing deck that is composed of individual player decks.
    /// This allows for controlled deck setup for testing purposes.
    /// </summary>
    public class TestDeckSO : DeckSO
    {
        [SerializeField] private DeckSO _player1Deck;
        [SerializeField] private DeckSO _player2Deck;
        [SerializeField] private DeckSO _player3Deck;
        [SerializeField] private DeckSO _player4Deck;

        /// <summary>
        /// Sets up the test deck by clearing any existing cards and adding
        /// the cards from each player's predefined deck.
        /// </summary>
        public DeckSO SetupTestDeck()
        {
            // Clear the current deck to prepare for test setup
            Cards.Clear();

            // Add cards from player 1's deck
            AddCardsFromDeck(_player1Deck);

            // Add cards from player 2's deck
            AddCardsFromDeck(_player2Deck);

            // Add cards from player 3's deck
            AddCardsFromDeck(_player3Deck);

            // Add cards from player 4's deck
            AddCardsFromDeck(_player4Deck);

            return this;
        }

        /// <summary>
        /// Adds cards from a given player deck into the test deck.
        /// </summary>
        /// <param name="playerDeck">The deck to add cards from.</param>
        private void AddCardsFromDeck(DeckSO playerDeck)
        {
            foreach (CardSO card in playerDeck.Cards)
            {
                Cards.Add(card);
            }
        }
    }
}

