using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;


namespace Big2Meow.DeckNCard
{
    /// <summary>
    /// Represents a playing card with suit, rank, card sprite, and backside sprite.
    /// </summary>
    [CreateAssetMenu(fileName = "New Card", menuName = "PlayingCard/Card")]
    public class CardSO : ScriptableObject
    {
        [SerializeField]
        private Suit _suit;

        /// <summary>
        /// Gets the suit of the card.
        /// </summary>
        public Suit Suit => _suit;

        [SerializeField]
        private Rank _rank;

        /// <summary>
        /// Gets the rank of the card.
        /// </summary>
        public Rank Rank => _rank;

        [SerializeField]
        private Sprite _cardSprite;

        /// <summary>
        /// Gets the sprite for the card face.
        /// </summary>
        public Sprite CardSprite => _cardSprite;

        [SerializeField]
        private Sprite _backsideSprite;

        /// <summary>
        /// Gets the sprite for the card backside.
        /// </summary>
        public Sprite BacksideSprite => _backsideSprite;

        /// <summary>
        /// Creates a new card model based on this card data.
        /// </summary>
        /// <returns>A new <see cref="CardModel"/> instance.</returns>
        public CardModel CreateNewCardModel()
        {
            return new CardModel(this);
        }
    }
}
   
