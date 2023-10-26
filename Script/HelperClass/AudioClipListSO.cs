using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipList", menuName = "Audio/Audio Clip List", order = 1)]
public class AudioClipListSO : ScriptableObject
{
    public List<AudioClip> audioClips;

    // Get a random clip from the list
    public AudioClip GetRandomClip()
    {
        if (audioClips.Count == 0) return null;

        int randomIndex = Random.Range(0, audioClips.Count);
        return audioClips[randomIndex];
    }
}
