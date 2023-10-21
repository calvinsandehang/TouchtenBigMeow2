using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public enum GMState 
{
    AskPlayer,
    OpenGame,
    CloseGame, 
    P1Turn,
    P2Turn,
    P3Turn,
    P4Turn
}
// Handle giving out card to the player
[DefaultExecutionOrder(-9999)]
public class Big2GMStateMachine : StateManager<GMState>, ISubscriber
{
    public static Big2GMStateMachine Instance;

    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private DeckSO _deck;

    [Header("Rule")]
    [SerializeField]
    [Range(2, 4)]
    private int _playerNumber = 4;


    [SerializeField]
    private int _totalCardInHandPerPlayer = 13;

    // a deck consist of 52 cards
    public List<CardModel> deck = new List<CardModel>();  

    public List<Big2PlayerHand> playerHands { get; private set; } = new List<Big2PlayerHand>();

    private DeckModel deckModel;
    
    public event Action OnDealerFinishDealingCards;
    public event Action<Big2PlayerHand> OnRoundWinnerDeclared;

    private bool firstRound = true;
    private int currentPlayerIndex = 0;

    public bool IsInitialized { get; private set; }

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

        StateInitialization();
        ParameterInitialization();
    }
  

    public void InitializeGame() 
    {
        InitializeDeck();
        DeckShuffle();
        if (!IsInitialized)
        {
            
            InitializePlayer();
        }
        DealCards();

        if (!IsInitialized)
        {
            IsInitialized = true;
            DetermineWhoPlayFirst();
        }

        if (!IsInitialized)        
            IsInitialized = true;

        Debug.Log("Game master has initialized.");
    }

    private void DetermineWhoPlayFirst()
    {
        foreach (var player in playerHands) 
        {
            if (player.CheckHavingThreeOfDiamonds())
            {
                currentPlayerIndex = playerHands.IndexOf(player);
                SetTurn(currentPlayerIndex);
                Debug.Log("The player that goes first is Player " + currentPlayerIndex );
                break;
            }
        }
    }

    #region Handle Turn
    // Determine who goes first based on Player who has three of diamonds
    private void SetTurn(int currentPlayerIndex) 
    {
        if (currentPlayerIndex > 3)
            Debug.LogError("player index should not be more than 3 as there are only 4 players");

        switch (currentPlayerIndex) 
        {
            case 0: // player turn
                Player1Turn();
                break;
            case 1:
                Player2Turn();
                break;
            case 2:
                Player3Turn();
                break;
            case 3:
                Player4Turn();
                break;
        }
    }

    public void NextTurn() 
    {
        // Increment currentPlayerIndex counterclockwise.
        currentPlayerIndex = (currentPlayerIndex + 3) % 4;

        SetTurn(currentPlayerIndex);
    }

    public void EndRound(Big2PlayerHand roundWinner)
    {
        // Update the currentPlayerIndex for the next round.
        currentPlayerIndex = playerHands.IndexOf(roundWinner);

        // Notify that the round winner has been declared.
        OnRoundWinnerDeclared?.Invoke(roundWinner);
    }
    #endregion

    #region Initialization
    protected override void ParameterInitialization()
    {

    }

    protected override void StateInitialization()
    {
        Debug.Log("GM initialize states");

        States[GMState.AskPlayer] = new Big2GMStateAskPlayer(GMState.AskPlayer, this);
        States[GMState.OpenGame] = new Big2GMStateOpenGame(GMState.OpenGame, this);
        States[GMState.CloseGame] = new Big2GMStateCloseGame(GMState.CloseGame, this);
        States[GMState.P1Turn] = new Big2GMStateP1Turn(GMState.P1Turn, this);
        States[GMState.P2Turn] = new Big2GMStateP2Turn(GMState.P2Turn, this);
        States[GMState.P3Turn] = new Big2GMStateP3Turn(GMState.P3Turn, this);
        States[GMState.P4Turn] = new Big2GMStateP4Turn(GMState.P4Turn, this);

        CurrentState = States[GMState.OpenGame];

        Debug.Log("Current state = " + CurrentState);
    }

    private void InitializePlayer()
    {
        // create hands for each player
        for (int i = 0; i < _playerNumber; i++)
        {
            GameObject playerHandObject = Instantiate(_playerPrefab);
            Big2PlayerHand playerHand = playerHandObject.GetComponent<Big2PlayerHand>();

            if (i == 0)
            {
                playerHand.PlayerType = PlayerType.Human; Debug.Log("HumanPlayer");               
            }
            else
                playerHand.PlayerType = PlayerType.AI;

            playerHand.InitializePlayerID(i);
            playerHands.Add(playerHand);
        }
    }

    public void DealCards()
    {
        // Deal 13 cards to each player
        for (int i = 0; i < +_totalCardInHandPerPlayer; i++)
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
                    return;
                }
            }
        }
        OnDealerFinishDealingCards?.Invoke();
                
    }
    #endregion

    #region Order
    private void Player1Turn() 
    {
        TransitionToState(GMState.P1Turn);
    }

    private void Player2Turn() 
    {
        TransitionToState(GMState.P2Turn);
    }

    private void Player3Turn() 
    {
        TransitionToState(GMState.P3Turn);
    }

    private void Player4Turn()
    {
        TransitionToState(GMState.P4Turn);
    }

    private void OnOpenGame() 
    {
        TransitionToState(GMState.OpenGame);
    }
    #endregion

    
    #endregion

   


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

    #region LookUp
    public bool CheckGameInFirstRound() 
    {
        return firstRound;
    }
    #endregion

    #region Setup 
    public void SetGameNotInFirstRound() 
    {
        firstRound = false;
    }
    #endregion

    #region Helper  

    private void InitializeDeck()
    {
        deckModel = new DeckModel(_deck);
    }

    // Method to shuffle the deck
    private void DeckShuffle()
    {
        deckModel.Shuffle();
    }

    public List<CardModel> GetRemainingDeck()
    {
        return deckModel.GetCurrentDeck();
    }

    public void SubscribeEvent()
    {

    }

    public void UnsubscribeEvent()
    {

    }

    #endregion


}
