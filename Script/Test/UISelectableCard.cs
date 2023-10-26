using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required namespace for handling UI events
using System;
using static GlobalDefine;

public enum CardState
{
    Selected,
    Deselected,
}

public class UISelectableCard : SubjectCard, IPointerClickHandler // Implementing interface
{
    
    private Image cardImage; // The UI Image component where the card's image will be displayed

    public Color initialColor = Color.white; // Initial color for the card
    public Color selectedColor = Color.green; // Color when the card is selected

    private bool isSelected = false; // Flag to check if the card is selected or not
    private CardModel cardModel;

    [SerializeField] 
    private AudioClip cardDeselectedSound;
    [SerializeField]
    private AudioClip cardSelectSound;

    private AudioSource audioSource;
   
    public CardModel GetCardModel() 
    {
        return cardModel;
    }
    private void OnDisable()
    {
        isSelected = false;
    }
    void Awake()
    {
        cardImage = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            gameObject.AddComponent<AudioSource>();
    }

    public void Initialize(CardModel card, PlayerType playerType)
    {
        cardModel = card;
        Sprite cardSprite;
        if (playerType == PlayerType.Human)
        {
            cardSprite = cardModel.CardSprite;
        }
        else
        {
            cardSprite = cardModel.BacksideSprite;
        }
       

        DisplayCard(cardSprite, playerType);
    }


    public void DisplayCard(CardSO cardSO)
    {
        cardImage.sprite = cardSO.CardSprite;
        cardImage.color = initialColor;

        if (cardImage.sprite == null)
        {
            Debug.LogWarning("Card sprite is missing for: " + cardSO.name);
        }
    }

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

    public void OnPointerClick(PointerEventData eventData)
    {
        // This method is called when the card is clicked
        if (isSelected)
        {
            DeselectCard();
        }
        else
        {
            SelectCard();
        }

        isSelected = !isSelected; // Toggle the selection state
    }

    public void SelectCard()
    {
        if (cardSelectSound != null)
        {
            audioSource.clip = cardSelectSound;
            audioSource.Play();
        }

        cardImage.color = selectedColor;
        NotifyObserver(CardState.Selected);

        // this many instances refer to one instance => Singleton
        CardEvaluator.Instance.RegisterCard(cardModel);
    }

    public void DeselectCard()
    {
        if (cardDeselectedSound != null)
        {
            audioSource.clip = cardDeselectedSound;
            audioSource.Play();
        }

        cardImage.color = initialColor;
        NotifyObserver(CardState.Deselected);

        // this many instances refer to one instance => Singleton
        //Debug.Log("DeselectCard()");
        CardEvaluator.Instance.DeregisterCard(cardModel);
    }   
}
