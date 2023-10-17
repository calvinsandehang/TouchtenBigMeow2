using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handle giving out card to the player
public class Dealer : MonoBehaviour
{
    public static Dealer Instance;

    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private DeckSO _deck;

    [Header("Rule")]
    [SerializeField]
    private int _playerNumber = 2;

    [SerializeField]
    private int _playerHandCount = 13;

    // a deck consist of 52 cards
    public List<CardModel> deck = new List<CardModel>();

    // Store player hands
    public List<PlayerHand> PlayerHands { get; private set; }

    private DeckModel deckModel;
    
    public event Action OnDealerFinishDealingCards;

    #region Initialization
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
    private void Start()
    {
        StartCoroutine(InitializeDealer());
    }

    private IEnumerator InitializeDealer()
    {
        DeckInitialization();
        yield return new WaitForEndOfFrame();
        DeckShuffle();
        Debug.Log("Dealer initialized.");
        IsInitialized = true;
    }
    #endregion

    public List<PlayerHand> DealCards() 
    {
        if (PlayerHands != null) 
        {
            Debug.LogWarning("Cards have been dealt");
            return null;
        }

        PlayerHands = new List<PlayerHand>();

        // create hands for each player
        for (int i = 0; i < _playerNumber; i++)
        {
            GameObject playerHandObject = Instantiate(_playerPrefab);
            PlayerHand playerHand = playerHandObject.GetComponent<PlayerHand>();
            PlayerHands.Add(playerHand);
        }        

        // Deal 13 cards to each player
        for (int i = 0; i <+_playerHandCount; i++)
        {
            foreach (PlayerHand playerHand in PlayerHands) 
            {
                CardModel card = deckModel.DrawCard();

                if (card != null)
                {
                    playerHand.AddCard(card);
                }
                else
                {
                    Debug.LogError("Not enough card on the deck");
                    return null;
                }
            }

        }
        OnDealerFinishDealingCards?.Invoke();
       
        return PlayerHands;
    }


    #region Debugging
    // This is a property for display in the inspector. It generates an array of string descriptions of the cards.
    public string[] DeckDebugView
    {
        get
        {
            string[] cardDescriptions = new string[deck.Count];
            for (int i = 0; i < deck.Count; i++)
            {
                cardDescriptions[i] = deck[i].ToString(); // Assuming you have a ToString method in CardModel that describes the card.
            }
            return cardDescriptions;
        }
    }


    #endregion

    #region Helper
    private void DeckInitialization()
    {
        deckModel = new DeckModel(_deck);
    }

    // Method to shuffle the deck
    private void DeckShuffle()
    {
        deckModel.Shuffle();
    }


    public bool IsInitialized;


    public List<CardModel> GetRemainingDeck()
    {
        return deckModel.GetCurrentDeck();
    }

    #endregion


}
