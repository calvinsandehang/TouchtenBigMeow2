using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

[CreateAssetMenu(fileName = "New Card", menuName = "PlayingCard/Card")]
public class CardSO : ScriptableObject
{
    [SerializeField]
    private Suit _suit;
    [SerializeField]
    private Rank _rank;
    [SerializeField]
    private Sprite _cardSprite;
    
    public Suit Suit => _suit;
    public Rank Rank => _rank;
    public Sprite CardSprite => _cardSprite;

    public CardModel CreateNewCardModel()
    {
        return new CardModel(this);
    }
}
    
