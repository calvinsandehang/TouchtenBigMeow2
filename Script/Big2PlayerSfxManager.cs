using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

[RequireComponent(typeof(AudioSource))]
public class Big2PlayerSfxManager : MonoBehaviour, ISubscriber
{
    [SerializeField] 
    private AudioClipListSO _winningClips;
    [SerializeField]
    private AudioClipListSO _losingClips;
    [SerializeField]
    private AudioClipListSO _playingClips;

    private AudioSource audioSource;

    private Big2PlayerStateMachine playerSM;
    private Big2PlayerHand playerHand;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        playerSM = GetComponent<Big2PlayerStateMachine>();
        playerHand = GetComponent<Big2PlayerHand>();
    }

    private void Start()
    {
        if(playerHand.PlayerType == PlayerType.Human)
            SubscribeEvent();
    }

    public void PlayRandomWinningClip()
    {
        Debug.Log("Winning audio has been played");
        PlayRandomClipFromList(_winningClips);
    }

    public void PlayRandomLosingClip()
    {
        Debug.Log("Losing audio has been played");
        PlayRandomClipFromList(_losingClips);
    }

    public void PlayRandomPlayingClip()
    {
        PlayRandomClipFromList(_playingClips);
    }

    private void PlayRandomClipFromList(AudioClipListSO clipList)
    {
        AudioClip clip = clipList.GetRandomClip();
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void SubscribeEvent()
    {
        Big2CustomEvent.OnPlayerIsWinning += PlayRandomWinningClip;
        playerSM.OnPlayerIsLosing += PlayRandomLosingClip;
    }

    public void UnsubscribeEvent()
    {
        playerSM.OnPlayerIsWinning -= PlayRandomWinningClip;
        playerSM.OnPlayerIsLosing -= PlayRandomLosingClip;
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }
}
