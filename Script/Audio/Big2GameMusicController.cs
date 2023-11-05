using Big2Meow.Gameplay;
using Big2Meow.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.Audio 
{
    /// <summary>
    /// Controls the game's music and transitions between normal and rush modes.
    /// </summary>
    public class Big2GameMusicController : MonoBehaviour
    {
        public AudioClip[] _normalMusicClips;
        public AudioClip[] _rushMusicClips;
        private int currentNormalClipIndex;
        private int currentRushClipIndex;
        private bool isRushMode;

        #region Monobehaviour
        private void Start()
        {
            isRushMode = false;
            StartNormalMusic(null);
            SubscribeEvent();
        }

        private void OnDisable()
        {
            UnsubscribeEvent();
        }

        private void Update()
        {
            // Check if the current music clip has finished playing and switch to the next one.
            if (!Big2GameMusicManager.Instance.GetComponent<AudioSource>().isPlaying)
            {
                if (isRushMode)
                {
                    Big2GameMusicManager.Instance.PlayNextClip(_rushMusicClips, ref currentRushClipIndex);
                }
                else
                {
                    Big2GameMusicManager.Instance.PlayNextClip(_normalMusicClips, ref currentNormalClipIndex);
                }
            }

            // Start rush music when the 'R' key is pressed.
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartRushMusic();
            }
        }
        #endregion

        #region Start Music methods
        private void StartNormalMusic(Big2PlayerHand playerHand)
        {
            isRushMode = false;
            if (_normalMusicClips.Length > 0)
            {
                currentNormalClipIndex = Random.Range(0, _normalMusicClips.Length);
                Big2GameMusicManager.Instance.PlayMusicClip(_normalMusicClips[currentNormalClipIndex]);
            }
        }

        /// <summary>
        /// Starts the rush mode music.
        /// </summary>
        public void StartRushMusic()
        {
            if (isRushMode) return;

            isRushMode = true;
            if (_rushMusicClips.Length > 0)
            {
                currentRushClipIndex = Random.Range(0, _rushMusicClips.Length);
                Big2GameMusicManager.Instance.PlayMusicClip(_rushMusicClips[currentRushClipIndex]);
            }
        }
        #endregion

        #region Subscrive Event
        private void SubscribeEvent()
        {
            Big2GlobalEvent.SubscribePlayerCardLessThanSix(StartRushMusic);
            Big2GlobalEvent.SubscribePlayerDropLastCard(StartNormalMusic);
        }

        private void UnsubscribeEvent()
        {
            Big2GlobalEvent.UnsubscribePlayerCardLessThanSix(StartRushMusic);
            Big2GlobalEvent.UnsubscribePlayerDropLastCard(StartNormalMusic);
        }
        #endregion


    }
}

