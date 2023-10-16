using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerCardAssigner : MonoBehaviour
{
    public static UIPlayerCardAssigner Instance;

    [SerializeField]
    private DeckSO deckSO;

    [SerializeField]
    private Transform cardParent;


    [SerializeField]
    private CardPool cardPool;

    private List<GameObject> activeCards = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }    

    public void DisplayCardsReceived(CardModel[] cards)
    {
        // Clear out the old cards.
        while (activeCards.Count > 0)
        {
            var card = activeCards[0];
            activeCards.RemoveAt(0);
            cardPool.ReturnToPool(card);
        }

        // Generate the new cards.
        foreach (var cardModel in cards)
        {
            GameObject cardGO = cardPool.Get();
            cardGO.transform.SetParent(cardParent, false);
            CardDisplay cardDisplay = cardGO.GetComponent<CardDisplay>();
            cardDisplay.DisplayCard(cardModel.CardSprite); // Adjust this if necessary to match CardModel structure.
            activeCards.Add(cardGO);
        }
    }
}
