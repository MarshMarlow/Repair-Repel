using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public static PlayerAudio Instance;
    public AudioSource audioSourceAmbience;
    public AudioSource audioSource;
    private bool isPaused = false;

    public void setPaused(bool paused) {
        isPaused = paused;
        if (paused) {
            audioSourceAmbience.Pause();
            audioSource.Pause();
        }
        else {
            audioSourceAmbience.UnPause();
            audioSource.UnPause();
        }
    }
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
