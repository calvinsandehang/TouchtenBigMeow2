using UnityEngine;
using UnityEngine.UI;

public class PlayerSubmissionUIButton : MonoBehaviour
{
    private Button submitButton;
    private Image buttonImage;

    [SerializeField]
    private Color allowedColor = Color.white;

    [SerializeField]
    private Color notAllowedColor = Color.gray;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        submitButton = GetComponent<Button>();
        OnNotAllowedToSubmitCard();
    }
    private void Start()
    {
        // Subscribe to the events
        CardSubmissionCheck.AllowedToSubmitCard += OnAllowedToSubmitCard;
        CardSubmissionCheck.NotAllowedToSubmitCard += OnNotAllowedToSubmitCard;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the events to prevent memory leaks
        CardSubmissionCheck.AllowedToSubmitCard -= OnAllowedToSubmitCard;
        CardSubmissionCheck.NotAllowedToSubmitCard -= OnNotAllowedToSubmitCard;
    }

    private void OnAllowedToSubmitCard()
    {
        // Set the button interactable and update the image color to allowedColor
        submitButton.interactable = true;
        buttonImage.color = allowedColor;
    }

    private void OnNotAllowedToSubmitCard()
    {
        // Set the button not interactable and update the image color to notAllowedColor
        submitButton.interactable = false;
        buttonImage.color = notAllowedColor;
    }

    public void OnSubmitButtonPressed() 
    {
        CardSubmissionCheck.Instance.OnSubmitCard();
    }
}
