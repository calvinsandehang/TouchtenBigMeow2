using Big2Meow.DeckNCard;
using Big2Meow.Gameplay;
using Big2Meow.Player;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;


namespace Big2Meow.FSM
{
    /// <summary>
    /// Enumeration representing different states of the game manager.  
    /// Changing the order of the enum WILL NOT break the system
    /// </summary>
    public enum GMState
    {
        AskPlayer,
        OpenGame,
        CloseGame,
        P0Turn,
        P1Turn,
        P2Turn,
        P3Turn,
        ForceRestartGame
    }

    /// <summary>
    /// Main game manager class that controls the flow of the Big2 game.
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    public class Big2GMStateMachine : StateManager<GMState>, ISubscriber
    {
        /// <summary>
        /// The singleton instance of the game manager.
        /// </summary>
        public static Big2GMStateMachine Instance;

        #region Prefabs and Deck

        [Header("Player Instance")]
        [Tooltip("Add player prefab")]
        [SerializeField] private GameObject _playerPrefab;
        [Tooltip("Where do you want the Player prefab to be instantiated")]
        [SerializeField] private Transform _playerParent;

        [Header("Deck")]
        [Tooltip("Test deck is not shuffled ")]
        [SerializeField] private DeckType _deckType;
        [SerializeField] private DeckSO _normalDeck;
        [SerializeField] private TestDeckSO _testDeck;
        public DeckSO NormalDeck => _normalDeck;
        public TestDeckSO TestDeck => _testDeck;
        public DeckType DeckType => _deckType;

        #endregion

        #region Game Rules

        [Header("Game Rules")]

        /// <summary>
        /// The number of players in the game (2 to 4).
        /// </summary>
        [SerializeField]
        [Range(2, 4)]
        private int _playerNumber = 4;
        public int PlayerNumber => _playerNumber;

        /// <summary>
        /// The total number of cards in hand per player.
        /// </summary>
        [SerializeField] private int _totalCardInHandPerPlayer = 13;
        public int TotalCardInHandPerplayer => _totalCardInHandPerPlayer;

        /// <summary>
        /// The delay after the end of the game before proceeding to the post-game phase.
        /// </summary>
        [SerializeField] private int _endGameDelay = 5;

        /// <summary>
        /// The delay after the post-game phase before starting a new game.
        /// </summary>
        [SerializeField] private int _postGameDelay = 5;
        public int PostGameDelay => _postGameDelay;

        #endregion

        #region Deck and Players
        public List<CardModel> deck = new List<CardModel>();
        public List<Big2PlayerHand> PlayerHands { get; private set; } = new List<Big2PlayerHand>();
        #endregion

        #region UI Components

        [Header("Player UI Components")]
        [SerializeField] private List<PlayerComponents> _playerUIComponent;
        public List<PlayerComponents> PlayerUIComponents => _playerUIComponent;

        private List<Big2PlayerHand> skippingPlayers = new List<Big2PlayerHand>();
        public DeckModel DeckModel { get; set; }

        #endregion

        #region Game State

        /// <summary>
        /// Indicates whether it's the first game.
        /// </summary>
        public bool FirstGame { get; set; } = true;

        /// <summary>
        /// The index of the current player.
        /// </summary>
        public int CurrentPlayerIndex { get; set; } = 0;

        /// <summary>
        /// Indicates whether it's time to determine who goes first.
        /// </summary>
        public static bool DetermineWhoGoFirst = true;

        /// <summary>
        /// Indicates whether the winner of the game has been determined.
        /// </summary>
        public static bool WinnerIsDetermined = false;

        /// <summary>
        /// Indicates whether the game has been initialized.
        /// </summary>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// The player who has won the game.
        /// </summary>
        public Big2PlayerHand WinnerPlayer { get; private set; }

        public Big2Rule Big2Rule { get; private set; }

        #endregion

        #region Monobehaviour
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
            SubscribeEvent();

            CurrentState = States[GMState.OpenGame];
            CurrentState.EnterState();
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }
        #endregion

        /// <summary>
        /// Instantiate a player object.
        /// </summary>
        /// <returns>The instantiated player object.</returns>
        public GameObject InstantiatePlayer()
        {
            GameObject playerHandObject = Instantiate(_playerPrefab, _playerParent);
            return playerHandObject;
        }

        #region Handle Turn
        // Determine who goes first based on Player who has three of diamonds
        public void SetTurn(int currentPlayerIndex)
        {
            if (currentPlayerIndex > 3)
            {
                Debug.LogError("Player index should not be more than 3 as there are only 4 players");
                return;
            }

            switch (currentPlayerIndex)
            {
                case 0: Player0Turn(); break;
                case 1: Player1Turn(); break;
                case 2: Player2Turn(); break;
                case 3: Player3Turn(); break;
            }
        }


        /// <summary>
        /// Handle the next turn in the game.
        /// </summary>
        /// <param name="playerHand">The player whose turn is next.</param>
        public void NextTurn(Big2PlayerHand playerHand)
        {
            // Increment currentPlayerIndex counterclockwise.
            Debug.Log("GM called Next Turn");
            DetermineWhoGoFirst = false;

            CurrentPlayerIndex = (CurrentPlayerIndex + 3) % 4;

            ResetSkippingPlayers(); // as long as there is someone that counter the card on the table, the skipping list is reset

            SetTurn(CurrentPlayerIndex);
        }

        /// <summary>
        /// Handles skipping a player's turn and checks if all other players have skipped.
        /// </summary>
        /// <param name="skippingPlayer">The player who is skipping their turn.</param>
        private void SkipTurn(Big2PlayerHand skippingPlayer)
        {
            Big2PlayerHand lastNonSkippingPlayer = null;
            skippingPlayers.Add(skippingPlayer);

            Debug.Log("skippingPlayers.Count =  " + skippingPlayers.Count);
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

        /// <summary>
        /// Executes the next round after players have skipped their turns.
        /// </summary>
        /// <param name="lastNonSkippingPlayer">The last player who did not skip their turn.</param>
        private IEnumerator ExecuteNextRound(Big2PlayerHand lastNonSkippingPlayer)
        {
            Big2GlobalEvent.BroadcastRoundHasEnded();
            yield return new WaitForSeconds(3f);

            int lastNonSkippingPlayerIndex = PlayerHands.IndexOf(lastNonSkippingPlayer);
            CurrentPlayerIndex = lastNonSkippingPlayerIndex;
            SetTurn(lastNonSkippingPlayerIndex);
        }


        /// <summary>
        /// Reset the list of players who have skipped their turn.
        /// </summary>
        public void ResetSkippingPlayers()
        {
            skippingPlayers.Clear();
        }

        /// <summary>
        /// Handle the end of the game.
        /// </summary>
        /// <param name="winningPlayer">The player who won the game.</param>
        private void EndGame(Big2PlayerHand winningPlayer)
        {
            WinnerPlayer = winningPlayer;
            WinnerIsDetermined = true;

            // Make the players win/lose
            foreach (var player in PlayerHands)
            {
                Big2PlayerStateMachine playerStateMachine = player.GetComponent<Big2PlayerStateMachine>();

                if (player == winningPlayer)
                    playerStateMachine.MakePlayerWin();
                else
                    playerStateMachine.MakePlayerLose();
            }

            StartCoroutine(DelayedAction(OnEndGame, _endGameDelay));

        }
        #endregion

        #region Initialization
        private void ParameterInitialization()
        {
            Big2Rule = new Big2Rule();
        }

        /// <summary>
        /// Initialize GM states
        /// </summary>
        private void StateInitialization()
        {
            States[GMState.AskPlayer] = new Big2GMStateAskPlayer(GMState.AskPlayer, this);
            States[GMState.OpenGame] = new Big2GMStateOpenGame(GMState.OpenGame, this);
            States[GMState.CloseGame] = new Big2GMStateCloseGame(GMState.CloseGame, this);
            States[GMState.ForceRestartGame] = new Big2GMStateForceRestartGame(GMState.ForceRestartGame, this);
            States[GMState.P0Turn] = new Big2GMStateP0Turn(GMState.P0Turn, this);
            States[GMState.P1Turn] = new Big2GMStateP1Turn(GMState.P1Turn, this);
            States[GMState.P2Turn] = new Big2GMStateP2Turn(GMState.P2Turn, this);
            States[GMState.P3Turn] = new Big2GMStateP3Turn(GMState.P3Turn, this);
        }
        #endregion

        #region State Transition

        /// <summary>
        /// Transition to the P0Turn state.
        /// </summary>
        private void Player0Turn()
        {
            RequestTransitionToState(GMState.P0Turn);
            Debug.Log("Requested Transition to : " + GMState.P0Turn);
        }

        /// <summary>
        /// Transition to the P1Turn state.
        /// </summary>
        private void Player1Turn()
        {
            RequestTransitionToState(GMState.P1Turn);
            Debug.Log("Requested Transition to : " + GMState.P1Turn);
        }

        /// <summary>
        /// Transition to the P2Turn state.
        /// </summary>
        private void Player2Turn()
        {
            RequestTransitionToState(GMState.P2Turn);
            Debug.Log("Requested Transition to : " + GMState.P2Turn);
        }

        /// <summary>
        /// Transition to the P3Turn state.
        /// </summary>
        private void Player3Turn()
        {
            RequestTransitionToState(GMState.P3Turn);
            Debug.Log("Requested Transition to : " + GMState.P3Turn);
        }

        /// <summary>
        /// Transition to the OpenGame state.
        /// </summary>
        public void OnOpenGame()
        {
            RequestTransitionToState(GMState.OpenGame);
            Debug.Log("Requested Transition to : " + GMState.OpenGame);
        }

        /// <summary>
        /// Transition to the CloseGame state.
        /// </summary>
        public void OnEndGame()
        {
            RequestTransitionToState(GMState.CloseGame);
            Debug.Log("Requested Transition to : " + GMState.CloseGame);
        }

        /// <summary>
        /// Transition to the AskPlayer state during post-game.
        /// </summary>
        public void AskPlayerInPostGame()
        {
            RequestTransitionToState(GMState.AskPlayer);
            Debug.Log("Requested Transition to : " + GMState.AskPlayer);
        }

        public void OnForceRestartGame()
        {
            RequestTransitionToState(GMState.ForceRestartGame);
            Debug.Log("Requested Transition to : " + GMState.ForceRestartGame);
        }
        public void OnQuadrapleTwo()
        {
            // and _endGameDelay is a predefined variable with the delay duration
            StartCoroutine(DelayedAction(OnForceRestartGame, _endGameDelay));
        }

        #endregion


        #region Order Player
        /// <summary>
        /// Order a player to play their turn based on their ID.
        /// </summary>
        /// <param name="playerID">The ID of the player to order to play.</param>
        public void OrderPlayerToPlay(int playerID)
        {
            Big2PlayerStateMachine playerStateMachine = PlayerHands[playerID].GetComponent<Big2PlayerStateMachine>();
            playerStateMachine.MakePlayerPlay();
        }

        /// <summary>
        /// Order all players to wait during post-game.
        /// </summary>
        public void OrderPlayerToWaitInPostGame()
        {
            foreach (var player in PlayerHands)
            {
                Big2PlayerStateMachine playerStateMachine = player.GetComponent<Big2PlayerStateMachine>();
                playerStateMachine.MakePlayerWait();
            }
        }
        #endregion

        #region Helper  

        /// <summary>
        /// Find the index of an element in a list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to search in.</param>
        /// <param name="element">The element to find.</param>
        /// <returns>The index of the element in the list, or -1 if not found.</returns>
        public int FindElementIndex<T>(List<T> list, T element)
        {
            return list.IndexOf(element);
        }

        /// <summary>
        /// Check if it's the first round of the game.
        /// </summary>
        /// <returns>True if it's the first round, false otherwise.</returns>
        public bool CheckGameInFirstRound()
        {
            return FirstGame;
        }


        /// <summary>
        /// Execute an action with a delayed wait.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="delay">The delay in seconds.</param>
        /// <returns>An IEnumerator for the delayed action.</returns>
        public IEnumerator DelayedAction(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        private void OnValidate()
        {
            if (_playerPrefab == null)
            {
                Debug.LogWarning("Player prefab is not assigned!", this);
            }

            if (_deckType == DeckType.Test)
            {
                Debug.LogWarning("Deck type is set to Test, which is not shuffled!", this);
            }
            else if (_deckType == DeckType.Normal) 
            {
                Debug.LogWarning("Deck type is set to Normal, change to Test Deck for testing purpose", this);
            }
        }


        #endregion

        #region Subscribe Event
        public void SubscribeEvent()
        {
            Big2GlobalEvent.SubscribePlayerFinishTurnGlobal(NextTurn);
            Big2GlobalEvent.SubscribeAIFinishTurnGlobal(NextTurn);
            Big2GlobalEvent.SubscribeAISkipTurnGlobal(SkipTurn);
            Big2GlobalEvent.SubscribePlayerSkipTurnGlobal(SkipTurn);
            Big2GlobalEvent.SubscribePlayerDropLastCard(EndGame);
            Big2GlobalEvent.SubscribeRestartGame(OnOpenGame);
            Big2GlobalEvent.SubscribeHavingQuadrupleTwo(OnQuadrapleTwo);
        }

        public void UnsubscribeEvent()
        {
            Big2GlobalEvent.UnsubscribePlayerFinishTurnGlobal(NextTurn);
            Big2GlobalEvent.UnsubscribeAIFinishTurnGlobal(NextTurn);
            Big2GlobalEvent.UnsubscribeAISkipTurnGlobal(SkipTurn);
            Big2GlobalEvent.UnsubscribePlayerSkipTurnGlobal(SkipTurn);
            Big2GlobalEvent.UnsubscribePlayerDropLastCard(EndGame);
            Big2GlobalEvent.SubscribeRestartGame(OnOpenGame);
            Big2GlobalEvent.UnsubscribeHavingQuadrupleTwo(OnQuadrapleTwo);
        }
        #endregion
    }
}



