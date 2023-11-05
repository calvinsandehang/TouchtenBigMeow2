using System;
using UnityEngine;
using static GlobalDefine;

namespace Big2Meow.DeckNCard
{
    /// <summary>
    /// Represents a playing card model with suit, rank, and sprites.
    /// </summary>
    public class CardModel
    {
        /// <summary>
        /// The suit of the card.
        /// </summary>
        public Suit CardSuit { get; }

        /// <summary>
        /// The rank of the card.
        /// </summary>
        public Rank CardRank { get; }

        /// <summary>
        /// The front-facing sprite of the card.
        /// </summary>
        public Sprite CardSprite { get; }

        /// <summary>
        /// The backside sprite of the card.
        /// </summary>
        public Sprite BacksideSprite { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardModel"/> class.
        /// </summary>
        /// <param name="cardSO">The CardSO containing card data.</param>
        public CardModel(CardSO cardSO)
        {
            CardSuit = cardSO.Suit;
            CardRank = cardSO.Rank;
            CardSprite = cardSO.CardSprite;
            BacksideSprite = cardSO.BacksideSprite;
        }

        /// <summary>
        /// Returns a string representation of the card.
        /// </summary>
        /// <returns>A string describing the card's rank and suit.</returns>
        public override string ToString()
        {
            return $"{CardRank} of {CardSuit}";
        }

        /// <summary>
        /// Determines whether the current card is equal to another card.
        /// </summary>
        /// <param name="obj">The object to compare with the current card.</param>
        /// <returns>True if the cards are equal; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                CardModel card = (CardModel)obj;
                return (CardSuit == card.CardSuit) && (CardRank == card.CardRank);
            }
        }

        /// <summary>
        /// Returns a hash code for the current card.
        /// </summary>
        /// <returns>A hash code based on the card's suit and rank.</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine in this case
            {
                int hash = 17; // Some prime number
                hash = hash * 23 + CardSuit.GetHashCode(); // Combine with Suit's hash code
                hash = hash * 23 + CardRank.GetHashCode(); // Combine with Rank's hash code
                return hash;
            }
        }
    }
}

