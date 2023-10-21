using System.Collections.Generic;
using static GlobalDefine;

public class CardInfo
{
    public HandType HandType { get; }
    public HandRank HandRank { get; }
    public List<CardModel> CardComposition { get; }

    public CardInfo(HandType newState, HandRank newHandRank, List<CardModel> submittedCards)
    {
        HandType = newState;
        HandRank = newHandRank;
        CardComposition = submittedCards;
    }
}