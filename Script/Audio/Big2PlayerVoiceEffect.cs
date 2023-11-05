using Big2Meow.DeckNCard;
using Big2Meow.Gameplay;
using UnityEngine;

namespace Big2Meow.Audio
{
    /// <summary>
    /// A class responsible for playing voice effects based on game events in a Big2 card game.
    /// </summary>
    public class Big2PlayerVoiceEffect : MonoBehaviour, IObserverTable
    {
        /// <summary>
        /// Array to store audio clips for voice effects.
        /// </summary>
        [SerializeField] private AudioClip[] _voiceClips;

        /// <summary>
        /// Volume for audio playback, adjustable from the inspector.
        /// </summary>
        [SerializeField] private float _audioVolume = 1.0f;

        private AudioSource audioSource; // The audio source component
        private Big2TableManager big2TableManager;

        #region Monobehaviour
        private void Start()
        {
            // Initialize the audio source
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = _audioVolume; // Set the volume

            big2TableManager = Big2TableManager.Instance;
            AddSelfToSubjectList();
        }

        private void OnDisable()
        {
            RemoveSelfFromSubjectList();
        }
        #endregion

        /// <summary>
        /// Adds this instance to the list of observers in the game table manager.
        /// </summary>
        public void AddSelfToSubjectList()
        {
            big2TableManager.AddObserver(this);
        }

        /// <summary>
        /// Handles the notification of assigning cards and plays a randomly chosen audio clip.
        /// </summary>
        /// <param name="cardInfo">Information about the assigned cards.</param>
        public void OnNotifyAssigningCard(CardInfo cardInfo)
        {
            // Play a randomly chosen audio clip here
            if (_voiceClips.Length > 0)
            {
                int randomIndex = Random.Range(0, _voiceClips.Length);
                audioSource.clip = _voiceClips[randomIndex];
                audioSource.Play();
            }
        }

        /// <summary>
        /// Handles the notification of the game table state (not used in this class).
        /// </summary>
        /// <param name="cardState">The current game hand type.</param>
        /// <param name="tableRank">The rank of the current hand.</param>
        public void OnNotifyTableState(GlobalDefine.HandType cardState, GlobalDefine.HandRank tableRank)
        {
            // Do nothing
        }

        /// <summary>
        /// Removes this instance from the list of observers in the game table manager.
        /// </summary>
        public void RemoveSelfFromSubjectList()
        {
            big2TableManager.RemoveObserver(this);
        }

       
    }
}
   
