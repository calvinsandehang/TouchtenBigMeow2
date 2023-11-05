using Big2Meow.DeckNCard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.DeckNCard
{
    /// <summary>
    /// Represents AI-specific card information.
    /// </summary>
    [Serializable]
    public class AiCardInfo
    {
        /// <summary>
        /// Gets or sets the list of card packages associated with AI card information.
        /// </summary>
        public List<CardPackage> CardPackages = new List<CardPackage>();

        /// <summary>
        /// Adds a card package to the AI's card information.
        /// </summary>
        /// <param name="cardPackage">The card package to add.</param>
        public void AddCardPackage(CardPackage cardPackage)
        {
            CardPackages.Add(cardPackage);
        }

        /// <summary>
        /// Removes a card package from the AI's card information.
        /// </summary>
        /// <param name="cardPackage">The card package to remove.</param>
        public void RemoveCardPackage(CardPackage cardPackage)
        {
            CardPackages.RemoveAll(package => package == cardPackage);
        }

        /// <summary>
        /// Clears all card packages from the AI's card information.
        /// </summary>
        public void ClearCardPackages()
        {
            CardPackages.Clear();
        }
    }

}
