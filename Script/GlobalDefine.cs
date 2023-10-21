using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalDefine 
{
    // In case the suit has rank system we order them as per poker rule where Spade has the higher rank
    public enum Suit
    {
        Diamonds, Clubs, Hearts, Spades // Ordering suits from low to high
    }

    public enum HandRank
    {
        None, HighCard, Pair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush
    }

    public enum Rank
    {
        Three = 1, Four = 2, Five = 3, Six = 4, Seven = 5, Eight = 6, Nine = 7, Ten = 8,
        Jack = 9, Queen = 10, King = 11, Ace = 12, Two = 13
    }

    public enum SortCriteria
    {
        Rank,
        Suit,
        BestHand
    }

    public enum HandType 
    {
        None,
        Single,
        Pair,
        ThreeOfAKind,
        FiveCards
    }

    public enum PlayerType 
    {
        Human,
        AI
    }

    public enum ButtonType
    {
        SubmitCard,
        SkipTurn,
    }

    public delegate void OnCardInteract(CardModel card);

    
}
