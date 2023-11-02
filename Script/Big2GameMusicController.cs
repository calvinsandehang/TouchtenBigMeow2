using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the game's music and transitions between normal and rush modes.
/// </summary>
public class Big2GameMusicController : MonoBehaviour
{
    public AudioClip[] normalMusicClips;
    public AudioClip[] rushMusicClips;
    private int currentNormalClipIndex;
    private int currentRushClipIndex;
    private bool isRushMode;

    private void Start()
    {
        isRushMode = false;
        StartNormalMusic(null);
        SubscribeEvent();
    }

    private void Update()
    {
        // Check if the current music clip has finished playing and switch to the next one.
        if (!Big2GameMusicManager.Instance.GetComponent<AudioSource>().isPlaying)
        {
            if (isRushMode)
            {
                Big2GameMusicManager.Instance.PlayNextClip(rushMusicClips, ref currentRushClipIndex);
            }
            else
            {
                Big2GameMusicManager.Instance.PlayNextClip(normalMusicClips, ref currentNormalClipIndex);
            }
        }

        // Start rush music when the 'R' key is pressed.
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRushMusic();
        }
    }

    private void StartNormalMusic(Big2PlayerHand playerHand)
    {
        isRushMode = false;
        if (normalMusicClips.Length > 0)
        {
            currentNormalClipIndex = Random.Range(0, normalMusicClips.Length);
            Big2GameMusicManager.Instance.PlayMusicClip(normalMusicClips[currentNormalClipIndex]);
        }
    }

    /// <summary>
    /// Starts the rush mode music.
    /// </summary>
    public void StartRushMusic()
    {
        if (isRushMode) return;

        isRushMode = true;
        if (rushMusicClips.Length > 0)
        {
            currentRushClipIndex = Random.Range(0, rushMusicClips.Length);
            Big2GameMusicManager.Instance.PlayMusicClip(rushMusicClips[currentRushClipIndex]);
        }
    }

    private void SubscribeEvent()
    {
        // Subscribe to events related to card gameplay.
        Big2PlayerHand.OnPlayerCardLessThanSix += StartRushMusic;
        Big2PlayerHand.OnPlayerLastCardIsDropped += StartNormalMusic;
    }

    private void UnsubscribeEvent()
    {
        // Unsubscribe from events to prevent memory leaks.
        Big2PlayerHand.OnPlayerCardLessThanSix -= StartRushMusic;
        Big2PlayerHand.OnPlayerLastCardIsDropped -= StartNormalMusic;
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }
}
