using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerSubmissionButton : MonoBehaviour, ISubscriber
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
        if (submissionCheck != null)
        {
            UnsubscribeEvent();
        }
        
    }
    public void InitializeButton(Big2CardSubmissionCheck big2CardSubmissionCheck) 
    {
        submissionCheck = big2CardSubmissionCheck;
        SubscribeEvent();
    }

    private void OnAllowedToSubmitCard()
    {
        // Set the button interactable and update the image color to allowedColor
        //Debug.Log("AllowedToSubmitCard()");
        submitButton.interactable = true;
        buttonImage.color = allowedColor;
    }

    private void OnNotAllowedToSubmitCard()
    {
        // Set the button not interactable and update the image color to notAllowedColor
        //Debug.Log("NotAllowedToSubmitCard()");
        submitButton.interactable = false;
        buttonImage.color = notAllowedColor;
    }

    public void SubscribeEvent()
    {
        Big2GlobalEvent.SubscribeCardSubmissionAllowed(OnAllowedToSubmitCard);
        Big2GlobalEvent.SubscribeCardSubmissionNotAllowed(OnNotAllowedToSubmitCard);
    }

    public void UnsubscribeEvent()
    {
        Big2GlobalEvent.UnsubscribeCardSubmissionAllowed(OnAllowedToSubmitCard);
        Big2GlobalEvent.UnsubscribeCardSubmissionNotAllowed(OnNotAllowedToSubmitCard);
    }
}
