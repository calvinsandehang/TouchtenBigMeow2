using UnityEngine;

namespace Big2Meow.Audio
{
    /// <summary>
    /// Controls the main menu music and transitions between different tracks.
    /// </summary>
    public class Big2MainMenuMusicController : MonoBehaviour
    {
        [SerializeField] private AudioClip[] normalMusicClips;
        private int currentNormalClipIndex;

        private void Start()
        {
            PlayRandomNormalMusicClip();
        }

        private void Update()
        {
            if (Big2GameMusicManager.Instance == null)
            {
                Debug.LogWarning("Load from Splash Screen sceen or enable DevMode to initialize the backend");
                return;
            }

            // Check if the music has stopped playing, and if so, play the next clip.
            if (!Big2GameMusicManager.Instance.GetComponent<AudioSource>().isPlaying)
            {
                PlayNextNormalMusicClip();
            }
        }

        /// <summary>
        /// Plays a randomly selected normal music clip.
        /// </summary>
        private void PlayRandomNormalMusicClip()
        {
            if (normalMusicClips.Length > 0)
            {
                currentNormalClipIndex = Random.Range(0, normalMusicClips.Length);
                Big2GameMusicManager.Instance.PlayMusicClip(normalMusicClips[currentNormalClipIndex]);
            }
        }

        /// <summary>
        /// Plays the next normal music clip in the sequence.
        /// </summary>
        private void PlayNextNormalMusicClip()
        {
            Big2GameMusicManager.Instance.PlayNextClip(normalMusicClips, ref currentNormalClipIndex);
        }
    }
}
   
