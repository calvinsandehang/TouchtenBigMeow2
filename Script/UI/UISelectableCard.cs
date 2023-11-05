using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using static GlobalDefine;
using Big2Meow.DeckNCard;
using Big2Meow.Gameplay;
using Big2Meow.Player;

namespace Big2Meow.UI
{
    /// <summary>
    /// A script to handle the behavior of a selectable card in the UI.
    /// </summary>
    public class UISelectableCard : MonoBehaviour, IPointerClickHandler
    {
        private Image cardImage;
        public Color initialColor = Color.white;
        public Color selectedColor = Color.green;
        private bool isSelected = false;
        private CardModel cardModel;

        [SerializeField] private AudioClip cardDeselectedSound;
        [SerializeField] private AudioClip cardSelectSound;
        private AudioSource audioSource;

        private void Awake()
        {
            cardImage = GetComponent<Image>();
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                gameObject.AddComponent<AudioSource>();
            }
        }
        private void OnDisable()
        {
            isSelected = false;
        }

        /// <summary>
        /// Initializes the UI card with the specified CardModel and player type.
        /// </summary>
        /// <param name="card">The CardModel to display.</param>
        /// <param name="playerType">The player type associated with the card.</param>
        public void Initialize(CardModel card, PlayerType playerType)
        {
            cardModel = card;
            Sprite cardSprite = (playerType == PlayerType.Human) ? cardModel.CardSprite : cardModel.BacksideSprite;
            DisplayCard(cardSprite, playerType);
        }

        /// <summary>
        /// Displays the card based on a CardSO.
        /// </summary>
        /// <param name="cardSO">The CardSO containing card information.</param>
        public void DisplayCard(CardSO cardSO)
        {
            cardImage.sprite = cardSO.CardSprite;
            cardImage.color = initialColor;

            if (cardImage.sprite == null)
            {
                Debug.LogWarning("Card sprite is missing for: " + cardSO.name);
            }
        }

        /// <summary>
        /// Displays the card with a specified sprite and player type.
        /// </summary>
        /// <param name="cardSprite">The sprite to display.</param>
        /// <param name="playerType">The player type associated with the card.</param>
        public void DisplayCard(Sprite cardSprite, PlayerType playerType)
        {
            cardImage.sprite = cardSprite;
            cardImage.color = initialColor;

            if (playerType == PlayerType.Human)
            {
                cardImage.raycastTarget = true;
            }
            else
            {
                cardImage.raycastTarget = false;
            }

            if (cardImage.sprite == null)
            {
                Debug.LogWarning("Card sprite is missing for: " + cardSprite.name);
            }
        }

        /// <summary>
        /// Handles the pointer click event on the card.
        /// </summary>
        /// <param name="eventData">The pointer event data.</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (isSelected)
            {
                DeselectCard();
            }
            else
            {
                SelectCard();
            }

            isSelected = !isSelected;
        }

        /// <summary>
        /// Selects the card, changing its appearance and notifying observers.
        /// </summary>
        public void SelectCard()
        {
            if (cardSelectSound != null)
            {
                audioSource.clip = cardSelectSound;
                audioSource.Play();
            }

            cardImage.color = selectedColor;
            Big2PlayerCardEvaluator.Instance.RegisterCard(cardModel);
        }

        /// <summary>
        /// Deselects the card, reverting its appearance and notifying observers.
        /// </summary>
        public void DeselectCard()
        {
            if (cardDeselectedSound != null)
            {
                audioSource.clip = cardDeselectedSound;
                audioSource.Play();
            }

            cardImage.color = initialColor;
            Big2PlayerCardEvaluator.Instance.DeregisterCard(cardModel);
        }

        /// <summary>
        /// Gets the associated CardModel of this UI card.
        /// </summary>
        /// <returns>The CardModel.</returns>
        public CardModel GetCardModel()
        {
            return cardModel;
        }

    }
}
   
