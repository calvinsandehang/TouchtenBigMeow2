using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private Image cardImage; // The UI Image component where the card's image will be displayed

    public Color initialColor = Color.white; // Initial color for the card
    public Color selectedColor = Color.green; // Color when the card is selected

    // Start is called before the first frame update
    void Awake()
    {
        cardImage = GetComponent<Image>();
    }

    public void DisplayCard(CardSO cardSO)
    {
        // Assign the sprite of the card to the UI Image component
        cardImage.sprite = cardSO.CardSprite;

        // Set the initial color
        cardImage.color = initialColor;

        // Optional: If the sprite is null, you can log a warning
        if (cardImage.sprite == null)
        {
            Debug.LogWarning("Card sprite is missing for: " + cardSO.name);
        }
    }

    public void DisplayCard(Sprite cardImage)
    {
        // Assign the sprite of the card to the UI Image component
        this.cardImage.sprite = cardImage;

        // Set the initial color
        this.cardImage.color = initialColor;

        // Optional: If the sprite is null, you can log a warning
        if (this.cardImage.sprite == null)
        {
            Debug.LogWarning("Card sprite is missing for: " + cardImage.name);
        }
    }

    public void SelectCard()
    {
        // Change the color to the selected color when the card is selected
        cardImage.color = selectedColor;
    }

    public void DeselectCard()
    {
        // Change the color back to the initial color when the card is deselected
        cardImage.color = initialColor;
    }
}
