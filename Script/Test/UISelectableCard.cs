using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required namespace for handling UI events
using System;
using static GlobalDefine;

public class UISelectableCard : MonoBehaviour, IPointerClickHandler // Implementing interface
{
    private Image cardImage; // The UI Image component where the card's image will be displayed

    public Color initialColor = Color.white; // Initial color for the card
    public Color selectedColor = Color.green; // Color when the card is selected

    private bool isSelected = false; // Flag to check if the card is selected or not
    private CardModel cardModel;

   
    public event OnCardInteract OnSelectCard;
    public event OnCardInteract OnDeselectCard;


    // Start is called before the first frame update
    void Awake()
    {
        cardImage = GetComponent<Image>();
    }

    public void Initialize(CardModel card)
    {
        cardModel = card;
        Sprite cardSprite = cardModel.CardSprite;

        DisplayCard(cardSprite);
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

    public void DisplayCard(Sprite cardSprite)
    {
        cardImage.sprite = cardSprite;
        cardImage.color = initialColor;

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
        cardImage.color = selectedColor;
        CardEvaluator.Instance.RegisterCard(cardModel);
    }

    public void DeselectCard()
    {
        cardImage.color = initialColor;
        CardEvaluator.Instance.DeregisterCard(cardModel);
    }
}
