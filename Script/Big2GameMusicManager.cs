using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Big2GameMusicManager : MonoBehaviour, ISubscriber
{
    public AudioClip[] normalMusicClips;
    public AudioClip[] rushMusicClips;
    private AudioSource audioSource;
    private int currentNormalClipIndex;
    private int currentRushClipIndex;
    private bool isRushMode;
    public float fadeOutTime = 2.0f; // Time to fade out in seconds

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        isRushMode = false;

        PlayNormalMusic(null);
        SubscribeEvent();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (isRushMode)
            {
                PlayNextRushClip();
            }
            else
            {
                PlayNextNormalClip();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRushMusic();
        }

        // Check if the music is about to end and start fading out
        if (audioSource.isPlaying && audioSource.time > audioSource.clip.length - fadeOutTime)
        {
            StartCoroutine(FadeOut());
        }
    }

    private void PlayNormalMusic(Big2PlayerHand playerHand) 
    {
        isRushMode = false ;

        if (normalMusicClips.Length > 0)
        {
            currentNormalClipIndex = Random.Range(0, normalMusicClips.Length);
            PlayMusicClip(normalMusicClips, currentNormalClipIndex);
        }
    }
    public void PlayMusicClip(AudioClip[] musicClips, int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < musicClips.Length)
        {
            audioSource.clip = musicClips[clipIndex];
            audioSource.volume = 1f; // Reset the volume
            audioSource.Play();
        }
    }

    public void PlayNextNormalClip()
    {
        currentNormalClipIndex = (currentNormalClipIndex + 1) % normalMusicClips.Length;
        PlayMusicClip(normalMusicClips, currentNormalClipIndex);
    }

    public void StartRushMusic()
    {
        if (isRushMode) return;

        isRushMode = true;
        if (rushMusicClips.Length > 0)
        {
            currentRushClipIndex = Random.Range(0, rushMusicClips.Length);
            PlayMusicClip(rushMusicClips, currentRushClipIndex);
        }
    }

    public void PlayNextRushClip()
    {
        currentRushClipIndex = (currentRushClipIndex + 1) % rushMusicClips.Length;
        PlayMusicClip(rushMusicClips, currentRushClipIndex);
    }

    System.Collections.IEnumerator FadeOut()
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

    public void SubscribeEvent()
    {
        Big2PlayerHand.OnPlayerCardLessThanSix += StartRushMusic;
        Big2PlayerHand.OnPlayerLastCardIsDropped += PlayNormalMusic;
    }

    public void UnsubscribeEvent()
    {
        Big2PlayerHand.OnPlayerCardLessThanSix -= StartRushMusic;
        Big2PlayerHand.OnPlayerLastCardIsDropped -= PlayNormalMusic;
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }
}
