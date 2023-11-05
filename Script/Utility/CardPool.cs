using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a pool of card objects.
/// </summary>
public class CardPool : MonoBehaviour
{
    /// <summary>
    /// The card prefab to be instantiated.
    /// </summary>
    [SerializeField]
    private GameObject _cardPrefab;

    /// <summary>
    /// The total number of cards in the pool.
    /// </summary>
    [SerializeField]
    private int _totalCard = 52;

    /// <summary>
    /// List of all card instances, both active and inactive.
    /// </summary>
    private List<GameObject> _cardPool = new List<GameObject>();

    /// <summary>
    /// Initializes the card pool by preloading cards.
    /// </summary>
    private void Awake()
    {
        PreloadCards(_totalCard);
    }

    /// <summary>
    /// Preloads a specified number of cards into the pool.
    /// </summary>
    /// <param name="count">The number of cards to preload.</param>
    private void PreloadCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject card = Instantiate(_cardPrefab);
            _cardPool.Add(card);
            DeactivateCard(card);
            
        }
    }
  
    /// <summary>
    /// Gets a card from the pool or instantiates a new one if necessary.
    /// </summary>
    /// <returns>The retrieved card GameObject.</returns>
    public GameObject GetCard()
    {
        foreach (var card in _cardPool)
        {
            if (!card.activeInHierarchy)
            {
                ActivateCard(card);
                return card;
            }
        }

        // If no inactive card is available, instantiate a new one, add it to the pool, and return it
        Debug.Log("Instantiate new Card");
        GameObject newCard = Instantiate(_cardPrefab);
        _cardPool.Add(newCard);
        return newCard;
    }




    /// <summary>
    /// Deactivates a card and returns it to the pool.
    /// </summary>
    /// <param name="card">The card GameObject to return.</param>
    public void ReturnCard(GameObject card)
    {
        if (_cardPool.Contains(card))
        {
            DeactivateCard(card);
        }
        else
        {
            Debug.LogError("Trying to return a card that doesn't belong to the pool.");
        }
    }

    /// <summary>
    /// Deactivates a card.
    /// </summary>
    /// <param name="card">The card GameObject to deactivate.</param>
    private void DeactivateCard(GameObject card)
    {
        card.SetActive(false);
    }

    /// <summary>
    /// Activates a card.
    /// </summary>
    /// <param name="card">The card GameObject to activate.</param>
    private void ActivateCard(GameObject card)
    {
        card.SetActive(true);
    }
}
