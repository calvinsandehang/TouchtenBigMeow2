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
/* Game : 
 * a game consist of multiple rounds
 * a player who manage to drop all of his cards first is the winner of a game
 * a game is initially start by player holding three of diamonds, but after that, the previous game winner start the game first
 * 
 * Round : 
 * A round consist of turns that made by player. 
 * A round is won by the player who drop the latest highest card
 * The next round start with the winner of the precious round
 * 
 * Turn : a turn is a state when a playing has a change to play/drop his card * 
 * 
 */


[DefaultExecutionOrder(-9999)]
public class Big2GMStateMachine : StateManager<GMState>, ISubscriber
{
    public static Big2GMStateMachine Instance;

    [SerializeField]
    private GameObject _playerPrefab;
    public GameObject PlayerPrefab => _playerPrefab;

    [SerializeField]
    private DeckSO _deck;

    public DeckSO Deck => _deck;

    [Header("Rule")]
    [SerializeField]
    [Range(2, 4)]
    private int _playerNumber = 4;
    public int PlayerNumber => _playerNumber;

    [SerializeField]
    private int _totalCardInHandPerPlayer = 13;
    public int TotalCardInHandPerplayer => _totalCardInHandPerPlayer;

    // a deck consist of 52 cards
    public List<CardModel> deck = new List<CardModel>();  
    public List<Big2PlayerHand> PlayerHands { get; private set; } = new List<Big2PlayerHand>();
    
    
    [SerializeField]
    private List<PlayerComponents> _playerUIComponent;
    public List<PlayerComponents> PlayerUIComponents => _playerUIComponent;
    public DeckModel DeckModel { get; set; }
    
    public event Action OnDealerFinishDealingCards;
    //public event Action<Big2PlayerHand> OnRoundWinnerDeclared;
    public static event Action OnRoundHasEnded; // Subs : Big2TableManager, UIBig2TableCards
    public static event Action OnGameHasEnded;
    public bool FirstGame { get; set; } = true;
    public int CurrentPlayerIndex { get; set; } = 0;
    public static bool DetermineWhoGoFirst = true;
    public static bool WinnerIsDetermined = false;
    public bool IsInitialized { get; set; }
    public Big2PlayerHand WinnerPlayer { get; private set; }
    

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

    public GameObject InstantiatePlayer() 
    {
        GameObject playerHandObject = Instantiate(PlayerPrefab);
        return playerHandObject;
    }

    #region Handle Turn
    // Determine who goes first based on Player who has three of diamonds
    public void SetTurn(int currentPlayerIndex) 
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

    public void NextTurn(Big2PlayerHand playerHand) 
    {
        // Increment currentPlayerIndex counterclockwise.
        Debug.Log("GM called Next Turn");
        DetermineWhoGoFirst = false;

        CurrentPlayerIndex = (CurrentPlayerIndex + 3) % 4;

        ResetSkippingPlayers(); // as long as there is someone that counter the card on the table, the skipping list is reset

        SetTurn(CurrentPlayerIndex);
    }

    private List<Big2PlayerHand> skippingPlayers = new List<Big2PlayerHand>();
    
    private void SkipTurn(Big2PlayerHand skippingPlayer)
    {
        //Debug.Log("SkipTurn");
        Big2PlayerHand lastNonSkippingPlayer = null;
        skippingPlayers.Add(skippingPlayer);

        
        for (int i = 0; i < skippingPlayers.Count; i++)
        {
            Debug.Log("Skipping players: " + string.Join(", ", skippingPlayers[i].PlayerID));
        }
        

        // Check if all other players have skipped
        if (skippingPlayers.Count == PlayerHands.Count - 1)
        {
            // Find the last non-skipping player
            foreach (var player in PlayerHands)
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
            CurrentPlayerIndex = (CurrentPlayerIndex + 3) % 4;
            SetTurn(CurrentPlayerIndex);
        }

        if (lastNonSkippingPlayer != null)
        {
            StartCoroutine(ExecuteNextRound(lastNonSkippingPlayer));
        }
    }

    private IEnumerator ExecuteNextRound(Big2PlayerHand lastNonSkippingPlayer)
    {
        OnRoundHasEnded?.Invoke();
        yield return new WaitForSeconds(3f);

        int lastNonSkippingPlayerIndex = PlayerHands.IndexOf(lastNonSkippingPlayer);
        CurrentPlayerIndex = lastNonSkippingPlayerIndex;
        SetTurn(lastNonSkippingPlayerIndex);
        //Debug.Log(" SetTurn(lastNonSkippingPlayerIndex)");
    }

    public void ResetSkippingPlayers() 
    {
        //Debug.Log("ResetSkippingPlayers() ");
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

    public void DealCards()
    {
        // Deal 13 cards to each player
        for (int i = 0; i < +_totalCardInHandPerPlayer; i++)
        {
            foreach (Big2PlayerHand playerHand in PlayerHands)
            {
                CardModel card = DeckModel.DrawCard();

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

    public void OnOpenGame() 
    {
        NextState = States[GMState.OpenGame];
        Debug.Log("NextState : " + NextState);
    }
   

    public void OrderPlayerToPlay(int playerID)
    {
        Big2PlayerStateMachine playerStateMachine = PlayerHands[playerID].GetComponent<Big2PlayerStateMachine>();
        playerStateMachine.MakePlayerPlay();
        //Debug.Log("GM OrderPlayer " + (playerID)+" to play");
    }

    public void StartGame() 
    {
        WinnerIsDetermined = false;
    }

    private void EndGame(Big2PlayerHand winningPlayer) 
    {
        WinnerPlayer = winningPlayer;
        NextState = States[GMState.CloseGame];

        // set global variable
        WinnerIsDetermined = true;

        // make the player win/lose
        foreach (var player in PlayerHands) 
        {
            Big2PlayerStateMachine playerStateMachine = player.GetComponent<Big2PlayerStateMachine>();

            if (player == winningPlayer)            
                playerStateMachine.MakePlayerWin();
            else            
                playerStateMachine.MakePlayerLose();            
        }        
    }

    private void OrderPlayerToWaitInPostGame() 
    {
        foreach (var player in PlayerHands)
        {
            Big2PlayerStateMachine playerStateMachine = player.GetComponent<Big2PlayerStateMachine>();
                       
            playerStateMachine.MakePlayerInPostGame();
        }
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
        return FirstGame;
    }
    #endregion


    #region Helper  

    public int FindElementIndex<T>(List<T> list, T element)
    {
        return list.IndexOf(element);
    }


    public List<CardModel> GetRemainingDeck()
    {
        return DeckModel.GetCurrentDeck();
    }

    public void SubscribeEvent()
    {
        Big2GlobalEvent.SubscribeAIFinishTurnGlobal(NextTurn);
        Big2GlobalEvent.SubscribeAISkipTurnGlobal(NextTurn);
        Big2GlobalEvent.SubscribePlayerSkipTurnGlobal(SkipTurn);
        Big2GlobalEvent.SubscribePlayerDropLastCard(NextTurn);
        Big2GlobalEvent.SubscribePlayerDropLastCard(EndGame);
    }
   

    public void UnsubscribeEvent()
    {
        Big2GlobalEvent.UnsubscribeAIFinishTurnGlobal(NextTurn);
        Big2GlobalEvent.UnsubscribeAISkipTurnGlobal(NextTurn);
        Big2GlobalEvent.UnsubscribePlayerSkipTurnGlobal(SkipTurn);
        Big2GlobalEvent.UnsubscribePlayerDropLastCard(NextTurn);
        Big2GlobalEvent.UnsubscribePlayerDropLastCard(EndGame);    
    }

    public void BroadcastCardHasBeenDealt()
    {
        OnDealerFinishDealingCards?.Invoke();
    }
    public void BroadcastGameHasEnded()
    {
        OnGameHasEnded?.Invoke();
    }


    #endregion

    private void OnDisable()
    {
        UnsubscribeEvent();
    }

}
