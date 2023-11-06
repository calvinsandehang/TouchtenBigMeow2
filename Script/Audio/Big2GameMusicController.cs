using Big2Meow.Player;
using System.Collections;
using UnityEngine;

namespace Big2Meow.Audio
{
    /// <summary>
    /// Manages the music transitions between normal and rush modes for the Big2 game.
    /// </summary>
    public class Big2GameMusicController : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _normalMusicClips;
        [SerializeField] private AudioClip[] _rushMusicClips;
        [SerializeField] private float _rushMusicDelay = 3.0f;

        private int currentNormalClipIndex;
        private int currentRushClipIndex;
        private bool isRushMode;
        private Coroutine rushMusicCoroutine;
        private AudioSource audioSource;

        /// <summary>
        /// Initializes the music controller by assigning the audio source and starting the music playback.
        /// </summary>
        private void Start()
        {
            audioSource = Big2GameMusicManager.Instance.GetComponent<AudioSource>();
            PlayInitialMusic();
            SubscribeToEvents();
        }

        /// <summary>
        /// Ensures proper cleanup of events and coroutines upon deactivation of the controller.
        /// </summary>
        private void OnDisable()
        {
            UnsubscribeFromEvents();
            if (rushMusicCoroutine != null)
            {
                StopCoroutine(rushMusicCoroutine);
            }
        }

        /// <summary>
        /// Plays the first music clip and starts the coroutine to monitor its completion.
        /// </summary>
        private void PlayInitialMusic()
        {
            currentNormalClipIndex = Random.Range(0, _normalMusicClips.Length);
            Big2GameMusicManager.Instance.PlayMusicClip(_normalMusicClips[currentNormalClipIndex]);
            StartCoroutine(WaitForMusicToEnd(_normalMusicClips[currentNormalClipIndex].length));
        }

        /// <summary>
        /// Waits for the given duration and then triggers the next music clip to be played.
        /// </summary>
        /// <param name="duration">The duration to wait for before playing the next music clip.</param>
        private IEnumerator WaitForMusicToEnd(float duration)
        {
            yield return new WaitForSeconds(duration);
            PlayNextClip();
        }

        /// <summary>
        /// Plays the next clip based on the current mode (normal or rush).
        /// </summary>
        private void PlayNextClip()
        {
            if (isRushMode)
            {
                Big2GameMusicManager.Instance.PlayNextClip(_rushMusicClips, ref currentRushClipIndex);
            }
            else
            {
                Big2GameMusicManager.Instance.PlayNextClip(_normalMusicClips, ref currentNormalClipIndex);
            }

            // Restart the coroutine with the new clip's length
            StartCoroutine(WaitForMusicToEnd(audioSource.clip.length));
        }

        /// <summary>
        /// Starts normal music playback. Stops any ongoing rush music coroutine.
        /// </summary>
        /// <param name="playerHand">The player's hand, not currently used in this context.</param>
        private void StartNormalMusic(Big2PlayerHand playerHand = null)
        {
            isRushMode = false;

            if (rushMusicCoroutine != null)
            {
                StopCoroutine(rushMusicCoroutine);
                rushMusicCoroutine = null;
            }

            if (_normalMusicClips.Length > 0)
            {
                currentNormalClipIndex = Random.Range(0, _normalMusicClips.Length);
                Big2GameMusicManager.Instance.PlayMusicClip(_normalMusicClips[currentNormalClipIndex]);
            }

        }

        /// <summary>
        /// Initiates the transition to rush music with a delay.
        /// </summary>
        public void StartRushMusic()
        {
            if (!isRushMode)
            {
                isRushMode = true;
                if (rushMusicCoroutine != null)
                {
                    StopCoroutine(rushMusicCoroutine);
                }
                rushMusicCoroutine = StartCoroutine(StartRushMusicAfterDelay());
            }
        }

        /// <summary>
        /// Coroutine to delay the start of rush music.
        /// </summary>
        /// <returns>Yield instruction that waits for the specified delay before starting rush music.</returns>
        private IEnumerator StartRushMusicAfterDelay()
        {
            yield return new WaitForSeconds(_rushMusicDelay);

            if (isRushMode)
            {
                if (!isRushMode)
                    Debug.LogWarning("ByPassingBollean gate misteryously");

                currentRushClipIndex = Random.Range(0, _rushMusicClips.Length);
                Big2GameMusicManager.Instance.PlayMusicClip(_rushMusicClips[currentRushClipIndex]);
            }
        }

        /// <summary>
        /// Subscribes to the relevant events for music control.
        /// </summary>
        private void SubscribeToEvents()
        {
            // Subscribe to events that should trigger music changes
            Big2GlobalEvent.SubscribePlayerCardLessThanSix(StartRushMusic);
            Big2GlobalEvent.SubscribePlayerDropLastCard(StartNormalMusic);
        }

        /// <summary>
        /// Unsubscribes from the events when the music controller is no longer active.
        /// </summary>
        private void UnsubscribeFromEvents()
        {
            // Unsubscribe from the events to prevent unwanted behavior when the object is not active
            Big2GlobalEvent.UnsubscribePlayerCardLessThanSix(StartRushMusic);
            Big2GlobalEvent.UnsubscribePlayerDropLastCard(StartNormalMusic);
        }
    }
}
