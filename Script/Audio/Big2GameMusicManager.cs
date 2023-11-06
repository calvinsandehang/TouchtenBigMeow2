using System.Collections;
using UnityEngine;

namespace Big2Meow.Audio 
{
    /// <summary>
    /// Manages music playback for the Big2 game.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class Big2GameMusicManager : MonoBehaviour
    {
        public static Big2GameMusicManager Instance { get; private set; }

        [SerializeField] private AudioSource audioSource;
        [Range(0f, 1f)] public float volume = 1.0f; // Exposed volume control
        public float fadeOutTime = 2.0f;

        #region Monobehaviour
        private void Awake()
        {
            // Singleton pattern: Ensure there is only one instance of Big2GameMusicManager.
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                // If an instance already exists, destroy this GameObject.
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = volume; // Bind the volume from the inspector to the audio source.
        }

        private void Update()
        {
            // Check if the volume has been modified in the inspector.
            if (audioSource.volume != volume)
            {
                audioSource.volume = volume;
            }
        }
        #endregion

        #region Player Music Methods
        /// <summary>
        /// Plays a music clip.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        public void PlayMusicClip(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        /// <summary>
        /// Plays the next music clip from an array of music clips.
        /// </summary>
        /// <param name="musicClips">The array of audio clips to cycle through.</param>
        /// <param name="currentClipIndex">The index of the current clip.</param>
        public void PlayNextClip(AudioClip[] musicClips, ref int currentClipIndex)
        {
            currentClipIndex = (currentClipIndex + 1) % musicClips.Length;
            PlayMusicClip(musicClips[currentClipIndex]);
        }

        /// <summary>
        /// Fades out the currently playing music.
        /// </summary>
        /// <returns>An IEnumerator for the fading process.</returns>
        public IEnumerator FadeOut()
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeOutTime;
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
        #endregion
    }
}

