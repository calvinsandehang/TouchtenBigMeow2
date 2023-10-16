using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public class CardModel
{
    public Suit CardSuit { get;}
    public Rank CardRank { get;}
    public Sprite CardSprite { get;}

    //constructor
    public CardModel (CardSO cardSO)
    {
        CardSuit = cardSO.Suit;
        CardRank = cardSO.Rank;
        CardSprite = cardSO.CardSprite;
    }

    public override string ToString()
    {
        return CardRank.ToString() + " of " + CardSuit.ToString(); // Adjust this format as necessary.
    }
}
