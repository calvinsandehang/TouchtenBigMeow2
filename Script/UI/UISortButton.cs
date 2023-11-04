using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

namespace Big2Meow.UI
{
    /// <summary>
    /// Represents a button in the UI that allows sorting of cards based on different criteria.
    /// </summary>
    public class UISortButton : MonoBehaviour
    {
        private const string BestHandText = "Sort by Best Hand";
        private const string RankText = "Sort by Rank";
        private const string SuitText = "Sort by Suit";

        private Button sortButton;

        [SerializeField] private TextMeshProUGUI buttonText;

        private List<Action> methods = new List<Action>();

        private int currentIndex = -1;
        private int maxIndex = 3;

        private void Start()
        {
            sortButton = GetComponent<Button>();
            sortButton.onClick.AddListener(OnSortButtonPressed);

            methods.Add(SortByBestHand);
            methods.Add(SortByRank);
            methods.Add(SortBySuit);

            buttonText.text = BestHandText;
        }

        /// <summary>
        /// Sort by the best hand criteria.
        /// </summary>
        public void SortByBestHand()
        {
            Big2GlobalEvent.BroadcastSortCard(SortCriteria.BestHand, 0, PlayerType.Human);
            // Change text on the button
            buttonText.text = RankText;
        }

        /// <summary>
        /// Sort by the rank criteria.
        /// </summary>
        public void SortByRank()
        {
            Big2GlobalEvent.BroadcastSortCard(SortCriteria.Rank, 0, PlayerType.Human);
            // Change text on the button
            buttonText.text = SuitText;
        }

        /// <summary>
        /// Sort by the suit criteria.
        /// </summary>
        public void SortBySuit()
        {
            Big2GlobalEvent.BroadcastSortCard(SortCriteria.Suit, 0, PlayerType.Human);
            // Change text on the button
            buttonText.text = BestHandText;
        }

        /// <summary>
        /// Handles the button click event.
        /// </summary>
        public void OnSortButtonPressed()
        {
            currentIndex = IncrementValue(currentIndex);
            methods[currentIndex].Invoke();
        }

        /// <summary>
        /// Increments the value in a circular manner.
        /// </summary>
        /// <param name="currentIndex">The current index value.</param>
        /// <returns>The incremented index.</returns>
        public int IncrementValue(int currentIndex)
        {
            return currentIndex = (currentIndex + 1) % maxIndex;
        }
    }
}
   
