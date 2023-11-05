using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.Audio
{
    /// <summary>
    /// Manages audio SFX playback for the Big2 game.
    /// </summary>
    public class Big2SfxManager : MonoBehaviour
    {
        /// <summary>
        /// Gets the singleton instance of the Big2SfxManager.
        /// </summary>
        public static Big2SfxManager Instance { get; private set; }

        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            // Singleton pattern: Ensure there is only one instance of Big2SfxManager.
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                // If an instance already exists, destroy this GameObject.
                Destroy(gameObject);
                return;
            }

            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Plays an audio clip.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        public void PlayClip(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        /// <summary>
        /// Plays an audio clip with a specified volume.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="volume">The volume level for the audio.</param>
        public void PlayClipWithVolume(AudioClip clip, float volume)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip, volume);
            }
        }

        private void OnDestroy()
        {
            // Clear the instance when this GameObject is destroyed.
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }

}
