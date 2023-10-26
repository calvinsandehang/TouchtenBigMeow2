using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2PlayerVoiceEffect : MonoBehaviour, IObserverTable
{
    public AudioClip[] voiceClips; // Array to store your audio clips
    public float audioVolume = 1.0f; // Volume for audio playback, can be set from the inspector
    private AudioSource audioSource; // The audio source component
    private Big2TableManager big2TableManager;

    void Start()
    {
        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = audioVolume; // Set the volume

        big2TableManager = Big2TableManager.Instance;
        AddSelfToSubjectList();
    }

    public void AddSelfToSubjectList()
    {
        big2TableManager.AddObserver(this);
    }

    public void OnNotifyAssigningCard(CardInfo cardInfo)
    {
        // Play a randomly chosen audio clip here
        if (voiceClips.Length > 0)
        {
            int randomIndex = Random.Range(0, voiceClips.Length);
            audioSource.clip = voiceClips[randomIndex];
            audioSource.Play();
        }
    }

    public void OnNotifyTableState(GlobalDefine.HandType cardState, GlobalDefine.HandRank tableRank)
    {
        // Do nothing
    }

    public void RemoveSelfToSubjectList()
    {
        big2TableManager.RemoveObserver(this);
    }

    private void OnDisable()
    {
        RemoveSelfToSubjectList();
    }
}
