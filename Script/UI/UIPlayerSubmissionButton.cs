using Big2Meow.Gameplay;
using Big2Meow.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Big2Meow.UI 
{
    /// <summary>
    /// Represents a UI button for player card submission.
    /// </summary>
    public class UIPlayerSubmissionButton : MonoBehaviour, ISubscriber
    {
        private Button submitButton;
        private Image buttonImage;
        private Big2CardSubmissionCheck submissionCheck;

        [SerializeField] private Color _allowedColor = Color.white;
        [SerializeField] private Color _notAllowedColor = Color.gray;

        #region Monobehaviour
        private void Awake()
        {
            buttonImage = GetComponent<Image>();
            submitButton = GetComponent<Button>();
            OnNotAllowedToSubmitCard();
        }

        private void OnDisable()
        {
            if (submissionCheck != null)
            {
                UnsubscribeEvent();
            }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the submit card button with a submission check handler.
        /// </summary>
        /// <param name="big2CardSubmissionCheck">The card submission check handler.</param>
        public void InitializeButton(Big2CardSubmissionCheck big2CardSubmissionCheck)
        {
            submissionCheck = big2CardSubmissionCheck;
            SubscribeEvent();
        }
        #endregion

        #region Submit card methods
        private void OnAllowedToSubmitCard()
        {
            SetButtonInteractable();
        }

        private void OnNotAllowedToSubmitCard()
        {
            SetButtonNonInteractable();
        }
        #endregion

        #region Handle Button
        /// <summary>
        /// Set the button interactable and update the image color to allowedColor.
        /// </summary>
        private void SetButtonInteractable()
        {
            submitButton.interactable = true;
            buttonImage.color = _allowedColor;
        }

        /// <summary>
        /// Set the button not interactable and update the image color to notAllowedColor.
        /// </summary>
        private void SetButtonNonInteractable()
        {
            submitButton.interactable = false;
            buttonImage.color = _notAllowedColor;
        }
        #endregion

        #region Subscribe Event
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
        #endregion
    }
}

