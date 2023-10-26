using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public class CardModel
{
    public Suit CardSuit { get;}
    public Rank CardRank { get;}
    public Sprite CardSprite { get;}
    public Sprite BacksideSprite { get; }

    //constructor
    public CardModel (CardSO cardSO)
    {
        CardSuit = cardSO.Suit;
        CardRank = cardSO.Rank;
        CardSprite = cardSO.CardSprite;
        BacksideSprite = cardSO.BacksideSprite;
    }

    public override string ToString()
    {
        return CardRank.ToString() + " of " + CardSuit.ToString(); // Adjust this format as necessary.
    }

    // Overriding Equals to compare based on CardSuit and CardRank
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
