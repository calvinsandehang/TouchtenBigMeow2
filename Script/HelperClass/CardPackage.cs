using System;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

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
