using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlayingHand : MonoBehaviour
{
    public CardModel[] Cards;
    protected int handSize;
    public virtual void Awake()
    {
        Cards = new CardModel[handSize];
    }

    public void SetCards(CardModel[] newCards)
    {
        if (handSize == 0)
        {
            Debug.LogError("Hand size is invalid");
            return; 
        }

        // Check if the newCards array matches the expected hand size.
        if (newCards.Length != handSize)
        {
            Debug.LogError("Incorrect number of cards for back hand");
            return;
        }

       // Clear the Cards array     
       Cards = new CardModel[handSize]; 

       Cards = newCards;

       UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < Cards.Length; i++) 
        {
            //Debug.Log("Card : " + Cards[i]);
        }
    }
}
