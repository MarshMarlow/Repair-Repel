using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSounds : MonoBehaviour {
    public float minDelay = 12f;
    public float maxDelay = 23f;

    public float playChance = 0.7f; // chance to play audio
    public List<AudioClip> ambienceSounds; 
    public Transform soundSpawns;
    public bool isPaused = false;
    private List<Transform> soundSpawnPoints = new List<Transform>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in soundSpawns) soundSpawnPoints.Add(t);
        StartCoroutine(PlayRandomSounds());
    }

    void Reset() {
        isPaused = false;
    }

    public void setPaused (bool paused) {
        isPaused = paused;
    }

    private IEnumerator PlayRandomSounds() {
        while (true) {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            if (ambienceSounds.Count == 0 || soundSpawnPoints.Count == 0) {
                continue;
            }

            if (Random.value <= playChance && !isPaused) {
                // pick an audio clip to play and the place to play the audio
                AudioClip clip = ambienceSounds[5];//ambienceSounds[Random.Range(0, ambienceSounds.Count)];
                
                Transform obj = soundSpawnPoints[Random.Range(0, soundSpawnPoints.Count)];
                
                AudioSource audiosource = obj.GetComponent<AudioSource>();

                if (audiosource != null) {
                    audiosource.spatialBlend = 1;
                    audiosource.volume = Random.Range(0.55f, 1.0f);
                    audiosource.pitch = Random.Range(0.9f, 1.1f);
                    audiosource.PlayOneShot(clip);
                    
                    // pause the audio when the game is paused
                    if (audiosource.isPlaying && isPaused) {
                        audiosource.Pause();
                    }

                    // unpause the audio when the game is unpaused
                    if (!isPaused) {
                        audiosource.UnPause();
                    }
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
