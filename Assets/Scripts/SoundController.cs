using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    
    public AudioClip BaseMusic;
    public AudioClip BalloonSpawn;
    public AudioClip BalloonPop;
    public AudioClip TakeDamage;
    public AudioClip WrongHeight;

    private AudioSource audioSource;
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = BaseMusic;
        audioSource.loop = true;
        audioSource.volume = 0.1f;
        audioSource.Play();
    }

    public void PlaySpawnSound()
    {
        audioSource.PlayOneShot(BalloonSpawn, 0.2f);
    }
    public void PlayPopSound()
    {
        audioSource.PlayOneShot(BalloonPop, 0.2f);
    }
    public void PlayTakeDamage()
    {
        audioSource.PlayOneShot(TakeDamage, 0.3f);
    }
    public void PlayWrongHeight()
    {
        audioSource.PlayOneShot(WrongHeight, 2f);
    }
    
}
