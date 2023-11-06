using Big2Meow.Player;
using UnityEngine;

namespace Big2Meow.Audio 
{
    public class Big2GameSfxController : MonoBehaviour, ISubscriber
    {
        [SerializeField] private AudioClip _warningSoundClip; // The warning sound when card below 6

        private bool isActive = false;

        private void Start()
        {
            SubscribeEvent();
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }

        /// <summary>
        /// Plays the warning sound clip using the Big2SfxManager.
        /// </summary>
        public void PlayWarningSound()
        {
            if (!isActive) 
            {

                isActive = true;
                // Ensures the SFX manager is available before attempting to play the clip.
                if (Big2SfxManager.Instance != null)
                {
                    Big2SfxManager.Instance.PlayClip(_warningSoundClip);
                }
            }
           
        }

        private void ResetState(Big2PlayerHand player) 
        {
            isActive = false;
        }
      

        public void SubscribeEvent()
        {
            Big2GlobalEvent.SubscribePlayerCardLessThanSix(PlayWarningSound);
            Big2GlobalEvent.SubscribePlayerDropLastCard(ResetState);
        }

        public void UnsubscribeEvent()
        {
            Big2GlobalEvent.UnsubscribePlayerCardLessThanSix(PlayWarningSound);
            Big2GlobalEvent.UnsubscribePlayerDropLastCard(ResetState);
        }
    }
}

