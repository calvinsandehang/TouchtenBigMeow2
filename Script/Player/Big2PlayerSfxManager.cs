using Big2Meow.FSM;
using Big2Meow.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

namespace Big2Meow.Player
{
    /// <summary>
    /// Manages sound effects for a Big2 player.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class Big2PlayerSfxManager : MonoBehaviour, ISubscriber
    {
        [SerializeField] private AudioClipListSO _winningClips;
        [SerializeField] private AudioClipListSO _losingClips;
        [SerializeField] private AudioClipListSO _playingClips;

        private AudioSource audioSource;

        private Big2PlayerStateMachine playerSM;
        private Big2PlayerHand playerHand;

        #region Monobehaviour
        private void Awake()
        {
            // Get the AudioSource component attached to this GameObject
            audioSource = GetComponent<AudioSource>();

            // Get references to other components
            playerSM = GetComponent<Big2PlayerStateMachine>();
            playerHand = GetComponent<Big2PlayerHand>();
        }

        private void Start()
        {
            // Subscribe to events only for human players
            if (playerHand.PlayerType == PlayerType.Human)
                SubscribeEvent();
        }

        private void OnDisable()
        {
            // Ensure event unsubscription when the component is disabled
            UnsubscribeEvent();
        }
        #endregion

        #region Play Audio Clip
        /// <summary>
        /// Plays a random winning audio clip.
        /// </summary>
        public void PlayRandomWinningClip()
        {
            Debug.Log("Winning audio has been played");
            PlayRandomClipFromList(_winningClips);
        }

        /// <summary>
        /// Plays a random losing audio clip.
        /// </summary>
        public void PlayRandomLosingClip()
        {
            Debug.Log("Losing audio has been played");
            PlayRandomClipFromList(_losingClips);
        }

        /// <summary>
        /// Plays a random playing audio clip.
        /// </summary>
        public void PlayRandomPlayingClip()
        {
            PlayRandomClipFromList(_playingClips);
        }

        // Play a random audio clip from the provided list
        private void PlayRandomClipFromList(AudioClipListSO clipList)
        {
            AudioClip clip = clipList.GetRandomClip();
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
        #endregion
        #region Subscribe Event
        /// <summary>
        /// Subscribe to relevant events.
        /// </summary>
        public void SubscribeEvent()
        {
            playerSM.OnPlayerIsWinning += PlayRandomWinningClip;
            playerSM.OnPlayerIsLosing += PlayRandomLosingClip;
        }

        /// <summary>
        /// Unsubscribe from relevant events.
        /// </summary>
        public void UnsubscribeEvent()
        {
            playerSM.OnPlayerIsWinning -= PlayRandomWinningClip;
            playerSM.OnPlayerIsLosing -= PlayRandomLosingClip;
        }

        #endregion


    }
}


