using UnityEngine;
/// <summary>
/// A class responsible for playing voice effects based on game events in a Big2 card game.
/// </summary>
public class Big2PlayerVoiceEffect : MonoBehaviour, IObserverTable
{
    /// <summary>
    /// Array to store audio clips for voice effects.
    /// </summary>
    public AudioClip[] VoiceClips;

    /// <summary>
    /// Volume for audio playback, adjustable from the inspector.
    /// </summary>
    public float AudioVolume = 1.0f;

    private AudioSource audioSource; // The audio source component
    private Big2TableManager big2TableManager;

    private void Start()
    {
        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = AudioVolume; // Set the volume

        big2TableManager = Big2TableManager.Instance;
        AddSelfToSubjectList();
    }

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
        if (VoiceClips.Length > 0)
        {
            int randomIndex = Random.Range(0, VoiceClips.Length);
            audioSource.clip = VoiceClips[randomIndex];
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

    private void OnDisable()
    {
        RemoveSelfFromSubjectList();
    }
}
