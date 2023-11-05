using System;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

namespace Big2Meow.DeckNCard
{
    /// <summary>
    /// Represents a collection of cards in a package.
    /// </summary>
    public class CardPackage
    {
        /// <summary>
        /// Gets or sets the type of the card package.
        /// </summary>
        public HandType CardPackageType { get; set; }

        /// <summary>
        /// Gets or sets the rank of the card package.
        /// </summary>
        public HandRank CardPackageRank { get; set; }

        /// <summary>
        /// Gets or sets the content of the card package as a list of card models.
        /// </summary>
        public List<CardModel> CardPackageContent { get; set; } = new List<CardModel>();
              
        /// <summary>
        /// Resets the card package to its default state.
        /// </summary>
        public void Reset()
        {
            // Assuming default values for HandType and HandRank enums are the "empty" state
            CardPackageType = default(HandType);
            CardPackageRank = default(HandRank);
            CardPackageContent.Clear();
        }
    }
}
