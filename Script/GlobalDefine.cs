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
        None, HighCard, Pair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush
    }

    public enum Rank
    {
        Three = 0, Four = 1, Five = 2, Six = 3, Seven = 4, Eight = 5, Nine = 6, Ten = 7,
        Jack = 8, Queen = 9, King = 10, Ace = 11, Two = 12
    }
}
