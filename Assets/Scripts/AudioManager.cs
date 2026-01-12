using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Header("----- Audio Source -----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    [Header("----- Audio Clip -----")]
    public AudioClip countdowntimer;
    public AudioClip countdowntimer2;
    public AudioClip gameplaymusic;
    public AudioClip votepickup;
    public AudioClip explosion;
    public AudioClip vitalvote;
    public AudioClip reputationdip;
    public AudioClip campaignsurge;
    public AudioClip slowroll;
    public AudioClip votingboost;
    public AudioClip frozenfury;

    private void Start()
    {
        musicSource.clip = gameplaymusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
