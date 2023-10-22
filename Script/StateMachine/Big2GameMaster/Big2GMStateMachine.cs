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
    P0Turn,
    P1Turn,
    P2Turn,
    P3Turn
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
    [SerializeField]
    private List<PlayerUIComponent> _playerUIComponent;

    private DeckModel deckModel;
    
    public event Action OnDealerFinishDealingCards;
    public event Action<Big2PlayerHand> OnRoundWinnerDeclared;
    public static event Action OnRoundHasConcluded;

    private bool firstRound = true;
    private int currentPlayerIndex = 0;
    public static bool DetermineWhoGoFirst = true;
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

    private void Start()
    {
        CurrentState = States[GMState.OpenGame];
        CurrentState.EnterState();

        SubscribeEvent();
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

        //Debug.Log("Game master has initialized.");
    }

    private void DetermineWhoPlayFirst()
    {
        foreach (var player in playerHands) 
        {
            if (player.CheckHavingThreeOfDiamonds())
            {
                currentPlayerIndex = playerHands.IndexOf(player);
                SetTurn(currentPlayerIndex);
                Debug.Log("The player that goes first is Player " + (currentPlayerIndex) );
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
        //Debug.Log("GM called SetTurn for Player " + (currentPlayerIndex));
        switch (currentPlayerIndex) 
        {
            case 0: // player turn
                Player0Turn();
                break;
            case 1:
                Player1Turn();
                break;
            case 2:
                Player2Turn();
                break;
            case 3:
                Player3Turn();
                break;
        }
    }

    public void NextTurn() 
    {
        // Increment currentPlayerIndex counterclockwise.
        Debug.Log("GM called Next Turn");
        DetermineWhoGoFirst = false;

        currentPlayerIndex = (currentPlayerIndex + 3) % 4;

        ResetSkippingPlayers(); // as long as there is someone that counter the card on the table, the skipping list is reset

        SetTurn(currentPlayerIndex);
    }

    public void EndRound(Big2PlayerHand roundWinner)
    {
        // Update the currentPlayerIndex for the next round.
        currentPlayerIndex = playerHands.IndexOf(roundWinner);

        // Notify that the round winner has been declared.
        OnRoundWinnerDeclared?.Invoke(roundWinner);
    }

    private List<Big2PlayerHand> skippingPlayers = new List<Big2PlayerHand>();
    
    private void SkipTurn(Big2PlayerHand skippingPlayer)
    {
        Debug.Log("SkipTurn");
        Big2PlayerHand lastNonSkippingPlayer = null;
        skippingPlayers.Add(skippingPlayer);

        for (int i = 0; i < skippingPlayers.Count; i++)
        {
            Debug.Log("Skipping players: " + string.Join(", ", skippingPlayers[i].PlayerID));
        }

        // Check if all other players have skipped
        if (skippingPlayers.Count == playerHands.Count - 1)
        {
            // Find the last non-skipping player
            foreach (var player in playerHands)
            {
                if (!skippingPlayers.Contains(player))
                {
                    lastNonSkippingPlayer = player;
                    Debug.Log(" lastNonSkippingPlayer : " + lastNonSkippingPlayer.PlayerID);
                    break;
                }
            }
        }
        else
        {
            currentPlayerIndex = (currentPlayerIndex + 3) % 4;
            SetTurn(currentPlayerIndex);
        }

        if (lastNonSkippingPlayer != null)
        {
            StartCoroutine(ExecuteNextRound(lastNonSkippingPlayer));

        }
    }

    private IEnumerator ExecuteNextRound(Big2PlayerHand lastNonSkippingPlayer)
    {
        OnRoundHasConcluded?.Invoke();
        yield return new WaitForSeconds(3f);

        int lastNonSkippingPlayerIndex = playerHands.IndexOf(lastNonSkippingPlayer);
        currentPlayerIndex = lastNonSkippingPlayerIndex;
        SetTurn(lastNonSkippingPlayerIndex);
        Debug.Log(" SetTurn(lastNonSkippingPlayerIndex)");
    }

    public void ResetSkippingPlayers() 
    {
        Debug.Log("ResetSkippingPlayers() ");
        skippingPlayers.Clear();
    }

    #endregion

    #region Initialization
    protected override void ParameterInitialization()
    {

    }

    protected override void StateInitialization()
    {
        //Debug.Log("GM initialize states");

        States[GMState.AskPlayer] = new Big2GMStateAskPlayer(GMState.AskPlayer, this);
        States[GMState.OpenGame] = new Big2GMStateOpenGame(GMState.OpenGame, this);
        States[GMState.CloseGame] = new Big2GMStateCloseGame(GMState.CloseGame, this);
        States[GMState.P0Turn] = new Big2GMStateP0Turn(GMState.P0Turn, this);
        States[GMState.P1Turn] = new Big2GMStateP1Turn(GMState.P1Turn, this);
        States[GMState.P2Turn] = new Big2GMStateP2Turn(GMState.P2Turn, this);
        States[GMState.P3Turn] = new Big2GMStateP3Turn(GMState.P3Turn, this);
        
    }

    private void InitializePlayer()
    {
        // create hands for each player
        for (int i = 0; i < _playerNumber; i++)
        {
            GameObject playerHandObject = Instantiate(_playerPrefab);
            Big2PlayerHand playerHand = playerHandObject.GetComponent<Big2PlayerHand>();
            Big2PlayerUIManager playerUIManager = playerHandObject.GetComponent<Big2PlayerUIManager>();

            if (i == 0)
            {
                playerHand.PlayerType = PlayerType.Human;       
            }
            else
                playerHand.PlayerType = PlayerType.AI;

            // initializae ID
            playerHand.InitializePlayerID(i);
            // initialize UI Elements
            playerUIManager.SetupSkipNotificationButton(_playerUIComponent[i].SkipNotification);

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

  

    private void Player0Turn() 
    {
        NextState = States[GMState.P0Turn];
        Debug.Log("NextState : " + NextState);
    }

    private void Player1Turn() 
    {
        NextState = States[GMState.P1Turn];
        Debug.Log("NextState : " + NextState);
    }

    private void Player2Turn() 
    {
        NextState = States[GMState.P2Turn];
        Debug.Log("NextState : " + NextState);
    }

    private void Player3Turn()
    {
        NextState = States[GMState.P3Turn];
        Debug.Log("NextState : " + NextState);
    }

    private void OnOpenGame() 
    {
        TransitionToState(GMState.OpenGame);
    }

    public void OrderPlayerToPlay(int playerID)
    {
        Big2PlayerStateMachine playerStateMachine = playerHands[playerID].GetComponent<Big2PlayerStateMachine>();
        playerStateMachine.MakePlayerPlay();
        //Debug.Log("GM OrderPlayer " + (playerID)+" to play");
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
        Big2SimpleAI.OnAIFinishTurnGlobal += NextTurn;
        Big2SimpleAI.OnAISkipTurn += SkipTurn;
        Big2PlayerSkipTurnHandler.OnPlayerSkipTurn += SkipTurn;
        Big2CardSubmissionCheck.OnPlayerFinishTurnGlobal += NextTurn;
    }

    public void UnsubscribeEvent()
    {
        Big2SimpleAI.OnAIFinishTurnGlobal -= NextTurn;
        Big2SimpleAI.OnAISkipTurn -= SkipTurn;
        Big2PlayerSkipTurnHandler.OnPlayerSkipTurn -= SkipTurn;
        Big2CardSubmissionCheck.OnPlayerFinishTurnGlobal -= NextTurn;
    }

    #endregion

    private void OnDisable()
    {
        UnsubscribeEvent();
    }

}
