using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;


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
    [Range(2, 4)]
    private int _playerNumber = 4;


    [SerializeField]
    private int _playerHandCount = 13;

    // a deck consist of 52 cards
    public List<CardModel> deck = new List<CardModel>();  

    public List<Big2PlayerHand> playerHands { get; private set; } = new List<Big2PlayerHand>();

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

        StartCoroutine(InitializeDealer());
    }
    private void Start()
    {
        //StartCoroutine(InitializeDealer());
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

    public List<Big2PlayerHand> DealCards() 
    {
        Debug.Log("Deal Cards");
        if (playerHands.Count > 0) 
        {
            Debug.LogWarning("Cards have been dealt");
            return null;
        }
        // create hands for each player
        for (int i = 0; i < _playerNumber; i++)
        {
            GameObject playerHandObject = Instantiate(_playerPrefab);
            Big2PlayerHand playerHand = playerHandObject.GetComponent<Big2PlayerHand>();

            if (i == 0) {
                playerHand.PlayerType = PlayerType.Human; Debug.Log("HUmanPlayer");
            }
               
            
            else
                playerHand.PlayerType = PlayerType.AI;            

            playerHands.Add(playerHand);
        }     

        // Deal 13 cards to each player
        for (int i = 0; i <+_playerHandCount; i++)
        {
            foreach (Big2PlayerHand playerHand in playerHands) 
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
       
        return playerHands;
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
