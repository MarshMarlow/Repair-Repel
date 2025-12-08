using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip[] grassFootsteps;
    public AudioClip[] stoneFootsteps;
    
    
    // plays an audio sound for grass walk
    public void PlayGrassFootstep() {
        if (grassFootsteps.Length == 0) return;
        
        int index = Random.Range (0, grassFootsteps.Length);
        audioSource.PlayOneShot(grassFootsteps[index]);
    }

    // plays an audio sound for stone walk
    public void PlayStoneFootstep() {
        if (stoneFootsteps.Length == 0) return;
        
        int index = Random.Range (0, stoneFootsteps.Length);
        audioSource.PlayOneShot(stoneFootsteps[index]);
    }
}
