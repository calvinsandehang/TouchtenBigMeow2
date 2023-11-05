using Big2Meow.DeckNCard;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

namespace Big2Meow.Gameplay 
{
    /// <summary>
    /// Manages the cards on the table and provides information about the current hand.
    /// </summary>
    public class Big2TableManager : SubjectTable, ISubscriber
    {
        /// <summary>
        /// The singleton instance of the table manager.
        /// </summary>
        public static Big2TableManager Instance;

        /// <summary>
        /// The type of hand on the table.
        /// </summary>
        [ShowInInspector]
        public HandType TableHandType { get; private set; }

        /// <summary>
        /// The rank of the hand on the table.
        /// </summary>
        [ShowInInspector]
        public HandRank TableHandRank { get; private set; }

        /// <summary>
        /// The list of cards currently on the table.
        /// </summary>
        [ShowInInspector]
        public List<CardModel> TableCards { get; private set; }

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

            ParameterInitialization();
        }

        private void ParameterInitialization()
        {
            TableHandType = HandType.None;
            TableHandRank = HandRank.None;
            TableCards = new List<CardModel>();

            SubscribeEvent();
        }

        /// <summary>
        /// Retrieves information about the cards on the table.
        /// </summary>
        /// <returns>A <see cref="CardInfo"/> object representing the table state.</returns>
        public CardInfo TableLookUp()
        {
            CardInfo tableInfo = new CardInfo(TableHandType, TableHandRank, new List<CardModel>(TableCards));
            return tableInfo;
        }

        /// <summary>
        /// Updates the cards on the table with the provided card information.
        /// </summary>
        /// <param name="cardInfo">The card information to update the table with.</param>
        public void UpdateTableCards(CardInfo cardInfo)
        {
            TableHandType = cardInfo.HandType;
            TableHandRank = cardInfo.HandRank;
            TableCards.Clear();
            TableCards.AddRange(cardInfo.CardComposition);
            NotifyObserverAssigningCard(cardInfo);
            NotifyTableState(TableHandType, TableHandRank);
        }

        /// <summary>
        /// Clears the table, resetting it to an empty state.
        /// </summary>
        private void CleanTable()
        {
            TableHandType = HandType.None;
            TableHandRank = HandRank.None;
            TableCards = new List<CardModel>();

            CardInfo tableInfo = new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
            NotifyObserverAssigningCard(tableInfo);
            NotifyTableState(TableHandType, TableHandRank);
        }

        /// <summary>
        /// Subscribes to relevant events for cleaning the table.
        /// </summary>
        public void SubscribeEvent()
        {
            Big2GlobalEvent.SubscribeRoundHasEnded(CleanTable);
            Big2GlobalEvent.SubscribeGameHasEnded(CleanTable);
            Big2GlobalEvent.SubscribeSubmitCard(UpdateTableCards);
        }

        /// <summary>
        /// Unsubscribes from events to prevent memory leaks.
        /// </summary>
        public void UnsubscribeEvent()
        {
            Big2GlobalEvent.UnsubscribeRoundHasEnded(CleanTable);
            Big2GlobalEvent.UnsubscribeGameHasEnded(CleanTable);
            Big2GlobalEvent.UnsubscribeSubmitCard(UpdateTableCards);
        }
    }
}

