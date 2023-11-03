using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A ScriptableObject that holds a list of AudioClips.
/// </summary>
[CreateAssetMenu(fileName = "AudioClipList", menuName = "Audio/Audio Clip List", order = 1)]
public class AudioClipListSO : ScriptableObject
{
    /// <summary>
    /// The list of AudioClips.
    /// </summary>
    public List<AudioClip> audioClips;

    /// <summary>
    /// Gets a random AudioClip from the list.
    /// </summary>
    /// <returns>A random AudioClip or null if the list is empty.</returns>
    public AudioClip GetRandomClip()
    {
        if (audioClips.Count == 0) return null;

        int randomIndex = Random.Range(0, audioClips.Count);
        return audioClips[randomIndex];
    }
}
