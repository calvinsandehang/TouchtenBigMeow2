using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.DeckNCard 
{
    /// <summary>
    /// Represents a deck of playing cards as a ScriptableObject.
    /// </summary>
    [CreateAssetMenu(fileName = "Deck", menuName = "PlayingCard/NormalDeck")]
    public class DeckSO : ScriptableObject
    {
        [SerializeField]
        private List<CardSO> _cards = new List<CardSO>();

        /// <summary>
        /// Gets the list of cards in the deck as a read-only collection.
        /// </summary>    
        public List<CardSO> Cards => _cards;

    }
}

