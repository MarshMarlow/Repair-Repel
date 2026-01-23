using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip[] grassFootsteps;
    public AudioClip[] stoneFootsteps;
    
    private bool onStone = false; // whether the bandit is walking on grass or stone
    
    
    // plays an audio sound for grass walk
    public void PlayGrassFootstep() {
        if (grassFootsteps.Length == 0) return;
        
        int index = Random.Range (0, grassFootsteps.Length);
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(grassFootsteps[index]);
    }

    // plays an audio sound for stone walk
    public void PlayStoneFootstep() {
        if (stoneFootsteps.Length == 0) return;
        
        int index = Random.Range (0, stoneFootsteps.Length);
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(stoneFootsteps[index]);
    }

    public void setOnStone(bool stone) {
        onStone = stone;
    }

    public void PlayFootstep() {
        // if walking on stone path
        if (onStone) {
            PlayStoneFootstep();
        }
        // if walking on grass
        else {
            PlayGrassFootstep();
        }
    }
}
