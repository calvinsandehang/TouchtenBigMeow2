using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerSubmissionButton : MonoBehaviour
{
    private Button submitButton;
    private Image buttonImage;
    private Big2CardSubmissionCheck submissionCheck;

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
 
    private void OnDisable()
    {
        // Unsubscribe from the events to prevent memory leaks
        submissionCheck.AllowedToSubmitCard -= OnAllowedToSubmitCard;
        submissionCheck.NotAllowedToSubmitCard -= OnNotAllowedToSubmitCard;
    }
    public void InitializeButton(Big2CardSubmissionCheck big2CardSubmissionCheck) 
    {
        submissionCheck = big2CardSubmissionCheck;

        // Subscribe to the events
        submissionCheck.AllowedToSubmitCard += OnAllowedToSubmitCard;
        submissionCheck.NotAllowedToSubmitCard += OnNotAllowedToSubmitCard;
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
}
