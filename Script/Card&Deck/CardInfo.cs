using System.Collections.Generic;
using static GlobalDefine;

namespace Big2Meow.DeckNCard 
{
    /// <summary>
    /// Represents information about a player's hand in the card game.
    /// </summary>
    public class CardInfo
    {
        /// <summary>
        /// Gets the hand type.
        /// </summary>
        public HandType HandType { get; }

        /// <summary>
        /// Gets the hand rank.
        /// </summary>
        public HandRank HandRank { get; }

        /// <summary>
        /// Gets the composition of cards in the hand.
        /// </summary>
        public List<CardModel> CardComposition { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardInfo"/> class.
        /// </summary>
        /// <param name="newHandType">The hand type.</param>
        /// <param name="newHandRank">The hand rank.</param>
        /// <param name="submittedCards">The composition of cards in the hand.</param>
        public CardInfo(HandType newHandType, HandRank newHandRank, List<CardModel> submittedCards)
        {
            HandType = newHandType;
            HandRank = newHandRank;
            CardComposition = submittedCards;
        }
    }

}
