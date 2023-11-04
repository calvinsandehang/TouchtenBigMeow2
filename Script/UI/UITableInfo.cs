using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static GlobalDefine;

namespace Big2Meow.UI
{
    /// <summary>
    /// Manages the display of table information on the UI.
    /// </summary>
    public class UITableInfo : MonoBehaviour, IObserverTable
    {
        [SerializeField]
        private TextMeshProUGUI _tableText;

        private Big2TableManager tableManager;

        void Start()
        {
            tableManager = Big2TableManager.Instance;
            AddSelfToSubjectList();
        }

        private void OnDisable()
        {
            RemoveSelfFromSubjectList();
        }

        /// <summary>
        /// Called when notified about the table state, updates the displayed text.
        /// </summary>
        /// <param name="tableHandType">The hand type on the table.</param>
        /// <param name="tableRank">The hand rank on the table.</param>
        public void OnNotifyTableState(HandType tableHandType, HandRank tableRank)
        {
            if (tableHandType == HandType.None)
            {
                _tableText.text = "";
            }
            else
            {
                _tableText.text = tableHandType.ToString();
            }
        }

        /// <summary>
        /// Removes this UI element from the list of observers in the table manager.
        /// </summary>
        public void RemoveSelfFromSubjectList()
        {
            tableManager.RemoveObserver(this);
        }

        public void OnNotifyAssigningCard(CardInfo cardInfo)
        {
            // do nothing
        }

        /// <summary>
        /// Adds this UI element to the list of observers in the table manager.
        /// </summary>
        public void AddSelfToSubjectList()
        {
            tableManager.AddObserver(this);
        }
    }
}

