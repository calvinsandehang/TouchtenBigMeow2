using Big2Meow.DeckNCard;
using Big2Meow.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

namespace Big2Meow.UI
{
    #region Card Template Class
    /// <summary>
    /// Represents a pair card template containing two Image components.
    /// </summary>
    [Serializable]
    public class PairCardTemplate
    {
        public Image CardImage1;
        public Image CardImage2;
    }

    /// <summary>
    /// Represents a Three of a Kind card template containing three Image components.
    /// </summary>
    [Serializable]
    public class ThreeOfAKindCardTemplate
    {
        public Image CardImage1;
        public Image CardImage2;
        public Image CardImage3;
    }

    /// <summary>
    /// Represents a Five Card template containing five Image components.
    /// </summary>
    [Serializable]
    public class FiveCardTemplate
    {
        public Image CardImage1;
        public Image CardImage2;
        public Image CardImage3;
        public Image CardImage4;
        public Image CardImage5;
    }
    #endregion
    public class UIBig2TableCards : MonoBehaviour, IObserverTable, ISubscriber
    {
        [Header("Parent Game Object")]
        [SerializeField] private GameObject _singleTable;
        [SerializeField] private GameObject _pairTable;
        [SerializeField] private GameObject _threeOfKindTable;
        [SerializeField] private GameObject _fiveTable;
        [Header("Images under the parent")]
        [SerializeField] private Image[] _singleTableImages = new Image[4];
        [SerializeField] private PairCardTemplate[] _pairCardTemplate = new PairCardTemplate[4];
        [SerializeField] private ThreeOfAKindCardTemplate[] _threeOfKindCardTemplate = new ThreeOfAKindCardTemplate[4];
        [SerializeField] private FiveCardTemplate[] _fiveCardTemplate = new FiveCardTemplate[4];

        private CardModel[] singleTableCardModel = new CardModel[4];
        private List<CardModel> submittedTableCardModel = new List<CardModel>();

        Big2TableManager tableManager;
        HandType currentTableState;

        #region Monobehaviour
        private void Start()
        {
            AddSelfToSubjectList();
            InitializeImageTemplate();
            SubscribeEvent();

            for (int i = 0; i < 4; i++)
            {
                singleTableCardModel[i] = null; // fill the array with null
            }
            // temporary
            currentTableState = HandType.None;
        }

        private void OnDisable()
        {
            RemoveSelfFromSubjectList();
        }
        #endregion

        #region Table Observer

        /// <summary>
        /// Adds the current object as an observer to the table manager.
        /// </summary>
        public void AddSelfToSubjectList()
        {
            tableManager = Big2TableManager.Instance;
            tableManager.AddObserver(this);
        }

        /// <summary>
        /// Removes the current object as an observer from the table manager.
        /// </summary>
        public void RemoveSelfFromSubjectList()
        {
            tableManager.RemoveObserver(this);
        }

        /// <summary>
        /// Handles notifications about card assignments and updates the table accordingly.
        /// </summary>
        /// <param name="cardInfo">Information about the assigned cards.</param>
        public void OnNotifyAssigningCard(CardInfo cardInfo)
        {
            currentTableState = cardInfo.HandType;

            switch (currentTableState)
            {
                case HandType.None:
                    EnableCertainTableType(null);
                    break;
                case HandType.Single:
                    EnableCertainTableType(_singleTable);
                    HandleAssigningForSingleCard(cardInfo.CardComposition);
                    break;
                case HandType.Pair:
                    EnableCertainTableType(_pairTable);
                    HandleAssigningCardForPair(cardInfo.CardComposition);
                    break;
                case HandType.ThreeOfAKind:
                    EnableCertainTableType(_threeOfKindTable);
                    HandleAssigningForThreeOfKindCard(cardInfo.CardComposition);
                    break;
                case HandType.FiveCards:
                    EnableCertainTableType(_fiveTable);
                    HandleAssigningForFiveCard(cardInfo.CardComposition);
                    break;
            }
        }

        /// <summary>
        /// Handles notifications about changes in table state (hand type and rank).
        /// </summary>
        /// <param name="cardState">The current hand type on the table.</param>
        /// <param name="tableRank">The rank of the current hand on the table.</param>
        public void OnNotifyTableState(HandType cardState, HandRank tableRank)
        {
            // This method can be used to respond to changes in table state if needed.
            // Currently, it does nothing.
        }

        #endregion

        #region Assigning Card

        /// <summary>
        /// Handles assigning single cards to the table.
        /// </summary>
        /// <param name="cardModels">The list of card models to assign.</param>
        private void HandleAssigningForSingleCard(List<CardModel> cardModels)
        {
            submittedTableCardModel.AddRange(cardModels);

            // Trim the cardModels list to a maximum of 4 elements
            if (submittedTableCardModel.Count > 4)
            {
                submittedTableCardModel.RemoveRange(0, submittedTableCardModel.Count - 4);
            }

            for (int i = 0; i < _singleTableImages.Length; i++)
            {
                if (i < submittedTableCardModel.Count)
                {
                    singleTableCardModel[i] = submittedTableCardModel[i];
                }
                else
                {
                    singleTableCardModel[i] = null;
                }

                // Update the sprites for the table images
                if (singleTableCardModel[i] == null) continue; // skip assigning sprite if still null

                Sprite cardSprite = singleTableCardModel[i].CardSprite;
                _singleTableImages[i].sprite = cardSprite;

                Color imageColor = _singleTableImages[i].color;
                imageColor.a = 1f;
                _singleTableImages[i].color = imageColor;
            }
        }

        /// <summary>
        /// Handles assigning cards for pairs on the table.
        /// </summary>
        /// <param name="cardModels">The list of card models to assign.</param>
        private void HandleAssigningCardForPair(List<CardModel> cardModels)
        {
            submittedTableCardModel.AddRange(cardModels);

            // Limit the list, remove the first component when the count is more than 8
            if (submittedTableCardModel.Count > 8)
            {
                submittedTableCardModel.RemoveRange(0, submittedTableCardModel.Count - 8);
            }

            int j = 0;

            for (int i = 0; i < submittedTableCardModel.Count; i += 2)
            {
                Sprite cardSprite1 = submittedTableCardModel[i].CardSprite;
                Sprite cardSprite2 = submittedTableCardModel[i + 1].CardSprite;

                _pairCardTemplate[j].CardImage1.sprite = cardSprite1;
                _pairCardTemplate[j].CardImage2.sprite = cardSprite2;

                Color imageColor1 = Color.white;
                imageColor1.a = 1f;
                _pairCardTemplate[j].CardImage1.color = imageColor1;

                Color imageColor2 = Color.white;
                imageColor2.a = 1f;
                _pairCardTemplate[j].CardImage2.color = imageColor2;

                j++;
            }
        }

        /// <summary>
        /// Handles assigning cards for Three of a Kind on the table.
        /// </summary>
        /// <param name="cardModels">The list of card models to assign.</param>
        private void HandleAssigningForThreeOfKindCard(List<CardModel> cardModels)
        {
            submittedTableCardModel.AddRange(cardModels);

            // Limit the list to a maximum of 12 elements
            if (submittedTableCardModel.Count > 12)
            {
                submittedTableCardModel.RemoveRange(0, submittedTableCardModel.Count - 12);
            }

            int j = 0;

            for (int i = 0; i < submittedTableCardModel.Count; i += 3)
            {
                Sprite cardSprite1 = submittedTableCardModel[i].CardSprite;
                Sprite cardSprite2 = submittedTableCardModel[i + 1].CardSprite;
                Sprite cardSprite3 = submittedTableCardModel[i + 2].CardSprite;

                _threeOfKindCardTemplate[j].CardImage1.sprite = cardSprite1;
                _threeOfKindCardTemplate[j].CardImage2.sprite = cardSprite2;
                _threeOfKindCardTemplate[j].CardImage3.sprite = cardSprite3;

                Color imageColor1 = Color.white;
                Color imageColor2 = Color.white;
                Color imageColor3 = Color.white;
                imageColor1.a = 1f;
                imageColor2.a = 1f;
                imageColor3.a = 1f;
                _threeOfKindCardTemplate[j].CardImage1.color = imageColor1;
                _threeOfKindCardTemplate[j].CardImage2.color = imageColor2;
                _threeOfKindCardTemplate[j].CardImage3.color = imageColor3;

                j++;
            }
        }

        /// <summary>
        /// Handles assigning cards for Five Card on the table.
        /// </summary>
        /// <param name="cardModels">The list of card models to assign.</param>
        private void HandleAssigningForFiveCard(List<CardModel> cardModels)
        {
            submittedTableCardModel.AddRange(cardModels);

            // Limit the list to a maximum of 20 elements
            if (submittedTableCardModel.Count > 20)
            {
                submittedTableCardModel.RemoveRange(0, submittedTableCardModel.Count - 20);
            }

            int j = 0;

            for (int i = 0; i < submittedTableCardModel.Count; i += 5)
            {
                Sprite cardSprite1 = submittedTableCardModel[i].CardSprite;
                Sprite cardSprite2 = submittedTableCardModel[i + 1].CardSprite;
                Sprite cardSprite3 = submittedTableCardModel[i + 2].CardSprite;
                Sprite cardSprite4 = submittedTableCardModel[i + 3].CardSprite;
                Sprite cardSprite5 = submittedTableCardModel[i + 4].CardSprite;

                _fiveCardTemplate[j].CardImage1.sprite = cardSprite1;
                _fiveCardTemplate[j].CardImage2.sprite = cardSprite2;
                _fiveCardTemplate[j].CardImage3.sprite = cardSprite3;
                _fiveCardTemplate[j].CardImage4.sprite = cardSprite4;
                _fiveCardTemplate[j].CardImage5.sprite = cardSprite5;

                Color imageColor1 = Color.white;
                Color imageColor2 = Color.white;
                Color imageColor3 = Color.white;
                Color imageColor4 = Color.white;
                Color imageColor5 = Color.white;
                imageColor1.a = 1f;
                imageColor2.a = 1f;
                imageColor3.a = 1f;
                imageColor4.a = 1f;
                imageColor5.a = 1f;
                _fiveCardTemplate[j].CardImage1.color = imageColor1;
                _fiveCardTemplate[j].CardImage2.color = imageColor2;
                _fiveCardTemplate[j].CardImage3.color = imageColor3;
                _fiveCardTemplate[j].CardImage4.color = imageColor4;
                _fiveCardTemplate[j].CardImage5.color = imageColor5;

                j++;
            }
        }

        #endregion

        #region Helper

        /// <summary>
        /// Initializes the image template and activates the appropriate table type.
        /// </summary>
        private void InitializeImageTemplate()
        {
            ActivateAllTableObjects();
            ResetImageTemplate();
        }

        /// <summary>
        /// Clears the table UI by removing submitted card models and resetting the image template.
        /// </summary>
        private void ClearTableUI()
        {
            submittedTableCardModel.Clear();
            ResetImageTemplate();
        }

        /// <summary>
        /// Enables the specified table type and disables others.
        /// </summary>
        /// <param name="enabledGameObject">The GameObject to enable.</param>
        private void EnableCertainTableType(GameObject enabledGameObject)
        {
            DeactivateAllTableObjects();

            if (enabledGameObject != null)
                enabledGameObject.SetActive(true);
        }

        /// <summary>
        /// Resets the image template by setting alpha to 0 for various table images.
        /// </summary>
        private void ResetImageTemplate()
        {
            SetAlphaToZeroForSingleTableImages();
            SetAlphaToZeroForPairTableImages();
            SetAlphaToZeroForThreeOfAKindTableImages();
            SetAlphaToZeroForFiveCardTableImages();
        }

        /// <summary>
        /// Activates all table objects.
        /// </summary>
        private void ActivateAllTableObjects()
        {
            _singleTable.SetActive(true);
            _pairTable.SetActive(true);
            _threeOfKindTable.SetActive(true);
            _fiveTable.SetActive(true);
        }

        /// <summary>
        /// Deactivates all table objects.
        /// </summary>
        private void DeactivateAllTableObjects()
        {
            _singleTable.SetActive(false);
            _pairTable.SetActive(false);
            _threeOfKindTable.SetActive(false);
            _fiveTable.SetActive(false);
        }

        /// <summary>
        /// Sets alpha to 0 for single table images.
        /// </summary>
        private void SetAlphaToZeroForSingleTableImages()
        {
            for (int i = 0; i < _singleTableImages.Length; i++)
            {
                Color imageColor = _singleTableImages[i].color;
                imageColor.a = 0f;
                _singleTableImages[i].color = imageColor;
            }
        }

        /// <summary>
        /// Sets alpha to 0 for pair table images.
        /// </summary>
        private void SetAlphaToZeroForPairTableImages()
        {
            for (int i = 0; i < _pairCardTemplate.Length; i++)
            {
                PairCardTemplate pairTemplate = _pairCardTemplate[i];
                if (pairTemplate != null)
                {
                    pairTemplate.CardImage1.color = new Color(0f, 0f, 0f, 0f);
                    pairTemplate.CardImage2.color = new Color(0f, 0f, 0f, 0f);
                }
            }
        }

        /// <summary>
        /// Sets alpha to 0 for Three of a Kind table images.
        /// </summary>
        private void SetAlphaToZeroForThreeOfAKindTableImages()
        {
            for (int i = 0; i < _threeOfKindCardTemplate.Length; i++)
            {
                ThreeOfAKindCardTemplate threeOfKindTemplate = _threeOfKindCardTemplate[i];
                if (threeOfKindTemplate != null)
                {
                    threeOfKindTemplate.CardImage1.color = new Color(0f, 0f, 0f, 0f);
                    threeOfKindTemplate.CardImage2.color = new Color(0f, 0f, 0f, 0f);
                    threeOfKindTemplate.CardImage3.color = new Color(0f, 0f, 0f, 0f);
                }
            }
        }

        /// <summary>
        /// Sets alpha to 0 for Five Card table images.
        /// </summary>
        private void SetAlphaToZeroForFiveCardTableImages()
        {
            for (int i = 0; i < _fiveCardTemplate.Length; i++)
            {
                FiveCardTemplate fiveCardTemplate = _fiveCardTemplate[i];
                if (fiveCardTemplate != null)
                {
                    fiveCardTemplate.CardImage1.color = new Color(0f, 0f, 0f, 0f);
                    fiveCardTemplate.CardImage2.color = new Color(0f, 0f, 0f, 0f);
                    fiveCardTemplate.CardImage3.color = new Color(0f, 0f, 0f, 0f);
                    fiveCardTemplate.CardImage4.color = new Color(0f, 0f, 0f, 0f);
                    fiveCardTemplate.CardImage5.color = new Color(0f, 0f, 0f, 0f);
                }
            }
        }

        #endregion

        #region Subscribe Event
        public void SubscribeEvent()
        {
            Big2GlobalEvent.SubscribeGameHasEnded(ClearTableUI);
            Big2GlobalEvent.SubscribeRoundHasEnded(ClearTableUI);
        }

        public void UnsubscribeEvent()
        {
            Big2GlobalEvent.UnsubscribeGameHasEnded(ClearTableUI);
            Big2GlobalEvent.UnsubscribeRoundHasEnded(ClearTableUI);
        }
        #endregion
    }

}
