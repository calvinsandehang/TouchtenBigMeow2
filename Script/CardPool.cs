using System.Collections.Generic;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    // The card prefab to be instantiated
    [SerializeField]
    private GameObject _cardPrefab;
    [SerializeField]
    private int _totalCard = 52;
   // List of all card instances, both active and inactive
   private List<GameObject> cardPool = new List<GameObject>();

    void Start()
    {
        // Optionally, preload a certain number of cards
        for (int i = 0; i < _totalCard; i++)
        {
            // Instantiate and deactivate immediately, then add to the pool
            var card = Instantiate(_cardPrefab);
            card.SetActive(false);
            cardPool.Add(card);
        }
    }

    // Get a card from the pool or instantiate a new one if necessary
    public GameObject GetCard()
    {
        // Search for an inactive card in the pool
        foreach (var card in cardPool)
        {
            if (!card.activeInHierarchy)
            {
                card.SetActive(true);
                return card;
            }
        }

        // If no inactive card is available, instantiate a new one, add it to the pool, and return it
        var newCard = Instantiate(_cardPrefab);
        cardPool.Add(newCard);
        return newCard;
    }

    // Return a card to the pool
    public void ReturnCard(GameObject card)
    {
        if (cardPool.Contains(card))
        {
            card.SetActive(false);
        }
        else
        {
            Debug.LogError("Trying to return a card that doesn't belong to the pool.");
        }
    }
}
