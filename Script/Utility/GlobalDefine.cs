using Big2Meow.DeckNCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class containing global enumerations used in the game.
/// </summary>
public static class GlobalDefine
{
    /// <summary>
    /// Enumerates the four suits in a deck of cards, ordered from low to high.
    /// </summary>
    public enum Suit
    {
        Diamonds, Clubs, Hearts, Spades // Ordering suits from low to high
    }

    /// <summary>
    /// Enumerates the hand ranks in poker.
    /// </summary>
    public enum HandRank
    {
        None, HighCard, Pair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush
    }

    /// <summary>
    /// Enumerates the ranks of playing cards, with Ace as the highest rank.
    /// </summary>
    public enum Rank
    {
        Three = 1, Four = 2, Five = 3, Six = 4, Seven = 5, Eight = 6, Nine = 7, Ten = 8,
        Jack = 9, Queen = 10, King = 11, Ace = 12, Two = 13
    }

    /// <summary>
    /// Enumerates different criteria for sorting cards.
    /// </summary>
    public enum SortCriteria
    {
        Rank,
        Suit,
        BestHand
    }

    /// <summary>
    /// Enumerates different hand types in the game.
    /// </summary>
    public enum HandType
    {
        None,
        Single,
        Pair,
        ThreeOfAKind,
        FiveCards
    }

    /// <summary>
    /// Enumerates different player types.
    /// </summary>
    public enum PlayerType
    {
        Human,
        AI
    }

    /// <summary>
    /// Enumerates different button types in the game.
    /// </summary>
    public enum ButtonType
    {
        SubmitCard,
        SkipTurn,
    }

    /// <summary>
    /// Delegate for handling card interactions.
    /// </summary>
    /// <param name="card">The card model.</param>
    public delegate void OnCardInteract(CardModel card);

    /// <summary>
    /// Enumerates different avatar types for players.
    /// </summary>
    public enum AvatarType
    {
        BlackCat,
        BlueCat,
        OrangeCat,
        WhiteCat
    }

    /// <summary>
    /// Represents the possible states of a selectable card.
    /// </summary>
    public enum CardState
    {
        Selected,
        Deselected,
    }

    /// <summary>
    /// Represents the possible deck.
    /// </summary>
    public enum DeckType
    {
        Normal,
        Test,
    }
}
