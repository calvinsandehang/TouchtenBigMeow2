using Big2Meow.DeckNCard;
using Big2Meow.Gameplay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

namespace Big2Meow.UI
{
    /// <summary>
    /// Represents a player's hand of cards.
    /// </summary>
    public class PlayerCard
    {
        public int PlayerID;
        public List<GameObject> CardsObjectsInPlayerHand = new List<GameObject>();
        public List<CardModel> CardModelsInPlayerHand = new List<CardModel>();
    }

    /// <summary>
    /// Manages the display and sorting of player hands.
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    [RequireComponent(typeof(Big2CardSorter))]
    [RequireComponent(typeof(CardPool))]
    public class UIPlayerHandManager : MonoBehaviour, ISubscriber
    {
        public static UIPlayerHandManager Instance;

        [SerializeField] private DeckSO deckSO;
        [SerializeField] private Transform cardParent;
        [SerializeField] private List<GameObject> _playerCardsParent;

        private List<PlayerCard> PlayerCards = new List<PlayerCard>();
        private SortCriteria currentSortCriteria;

        private Big2CardSorter cardSorter;
        private CardPool cardPool;


        #region MonoBehaviour    

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(Instance);

            PlayerCardInitialization();
            ParameterInitialization();
        }

        private void Start()
        {
            SubscribeEvent();
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the parameters.
        /// </summary>
        private void ParameterInitialization()
        {
            currentSortCriteria = SortCriteria.Rank;
            cardSorter = GetComponent<Big2CardSorter>();
            cardPool = GetComponent<CardPool>();
        }

        /// <summary>
        /// Initializes the player cards list.
        /// </summary>
        private void PlayerCardInitialization()
        {
            for (int i = 0; i < 4; i++)
                PlayerCards.Add(new PlayerCard());
        }
        #endregion

        #region Display and Sorting methods
        /// <summary>
        /// Displays the cards in a player's hand.
        /// </summary>
        public void DisplayCards(List<CardModel> cards, int playerID, PlayerType playerType)
        {
            //Debug.Log($"Display Card, Player {playerID}");

            PlayerCards[playerID].CardModelsInPlayerHand.Clear();
            PlayerCards[playerID].CardModelsInPlayerHand = cards.ToList();

            // Clear out the old cards.
            while (PlayerCards[playerID].CardsObjectsInPlayerHand.Count > 0)
            {
                var card = PlayerCards[playerID].CardsObjectsInPlayerHand[0];
                PlayerCards[playerID].CardsObjectsInPlayerHand.RemoveAt(0);
                cardPool.ReturnCard(card);
            }

            // Generate the new cards from the pool
            foreach (var cardModel in PlayerCards[playerID].CardModelsInPlayerHand)
            {
                GameObject cardGO = cardPool.GetCard();
                cardGO.transform.SetParent(_playerCardsParent[playerID].transform, false);
                cardGO.transform.localRotation = Quaternion.identity;  // Reset rotation to 0,0,0
                UISelectableCard selectableCard = cardGO.GetComponent<UISelectableCard>();
                selectableCard.Initialize(cardModel, playerType);  // Adjust this if necessary to match CardModel structure.
                PlayerCards[playerID].CardsObjectsInPlayerHand.Add(cardGO);
            }

            //Debug.Log(_playerCardsParent[playerID].transform);

            SortPlayerHand(currentSortCriteria, playerID, playerType);
        }

        /// <summary>
        /// Sorts a player's hand based on the specified criteria.
        /// </summary>
        public void SortPlayerHand(SortCriteria criteria, int playerID, PlayerType playerType)
        {
            switch (criteria)
            {
                case SortCriteria.Rank:
                    currentSortCriteria = SortCriteria.Rank;
                    cardSorter.SortPlayerHandByRank(PlayerCards[playerID].CardsObjectsInPlayerHand, playerType);
                    break;
                case SortCriteria.Suit:
                    currentSortCriteria = SortCriteria.Suit;
                    cardSorter.SortPlayerHandBySuit(PlayerCards[playerID].CardsObjectsInPlayerHand, playerType);
                    break;
                case SortCriteria.BestHand:
                    currentSortCriteria = SortCriteria.BestHand;
                    cardSorter.SortPlayerHandByBestHand(PlayerCards[playerID].CardsObjectsInPlayerHand, cardPool, _playerCardsParent[playerID].transform, playerType);
                    break;
            }
        }
        #endregion

        #region Subscribe Event
        public void SubscribeEvent()
        {
            Big2GlobalEvent.SubscribeSortCard(SortPlayerHand);
        }

        public void UnsubscribeEvent()
        {
            Big2GlobalEvent.UnsubscribeSortCard(SortPlayerHand);
        }
        #endregion
    }
}
   
