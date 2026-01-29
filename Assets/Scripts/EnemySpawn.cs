using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawn : MonoBehaviour
{
    private bool canSummon = false;
    public GameObject bandit; // the bandit prefab
    public GameObject golem; // the golem prefab
    public GameObject ghost; // the ghost prefab
    public GameObject tomato; //the tomato prefab

    public Transform banditSpawnParent; // the parent holding the bandit spawns
    public Transform golemSpawnParent; // the parent holding the golem spawns
    public Transform ghostSpawnParent; // the parent holding the ghost spawns
    
    public List<Transform> bandit_spawnPoints = new List<Transform>(); // all the bandit spawns
    public List<Transform> golem_spawnPoints = new List<Transform>(); // all the golem spawns
    public List<Transform> ghost_spawnPoints = new List<Transform>(); // all the ghost spawns
    
    private List<float> golem_inUse_time = new List<float>(); // records the time of when it was last summoned
    
    public float spawnInterval = 0.35f; // how long it takes per spawning an enemy
    
    public float spawnBanditInterval = 0.35f;
    public float spawnGolemInterval = 0.3f;
    public float spawnGhostInterval = 0.2f;
    
    public float waveDelay = 30f; // how long between waves
    
    public int currentWave = 0;
    private bool isSpawning = false;
    
    // spawn weights
    public int bandit_weight = 65;
    public int golem_weight = 28;
    public int ghost_weight = 7;

    public int golemUnlockWave = 2;
    public int ghostUnlockWave = 5;
    
    // text on the UI
    public TMP_Text waveText;
    
    // audio to start wave
    public AudioClip wave_audio;
    public AudioSource audiosource;
    
    public void Start() {
        // add the spawn points of each enemy into the respective lists
        foreach (Transform t in banditSpawnParent) bandit_spawnPoints.Add(t);
        foreach (Transform t in golemSpawnParent) golem_spawnPoints.Add(t);
        foreach (Transform t in ghostSpawnParent) ghost_spawnPoints.Add(t);
        
        currentWave = 0;
        isSpawning = false;

        // initializes the list of the golem in use times to 0
        for (int i = 0; i < golem_spawnPoints.Count; i++) {
            golem_inUse_time.Add(0);
        }
        // start the summon wave
        StartSummon();
    }
    
    // resets all variables
    public void Reset() {
        currentWave = 0;
        isSpawning = false;
        canSummon = false;
        
        StopAllCoroutines();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            Destroy(enemy);
        }
        
        for (int i = 0; i < golem_spawnPoints.Count; i++) {
            golem_inUse_time[i] = 0;
        }
    }
    
    public void StartSummon() {
        StartCoroutine(StartNextWave());
    }

    
    // update the UI text
    void Update() {
        waveText.text = "WAVE " + currentWave;
    }
    
    private IEnumerator StartNextWave() {
        if (isSpawning) yield break;
        isSpawning = true;

        currentWave++;
        Debug.Log("Starting wave " + currentWave);
        audiosource.PlayOneShot(wave_audio);
        yield return new WaitForSeconds(9); // how long it takes for the war horn
        

        int enemiesToSpawn = 5 + currentWave * 3;

        // spawns each enemy at an interval
        for (int i = 0; i < enemiesToSpawn; i++) {
            int enemyType = SpawnWeightedEnemy();
            
            if (enemyType == 1) yield return new WaitForSeconds(spawnBanditInterval);
            else if (enemyType == 2) yield return new WaitForSeconds(spawnGolemInterval);
            else if (enemyType == 3) yield return new WaitForSeconds(spawnGhostInterval);
            //yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
        
        yield return new WaitForSeconds(waveDelay);
        StartCoroutine(StartNextWave());
    }

    // returns the enemy type (1: bandit, 2: golem, 3: ghost)
    private int SpawnWeightedEnemy() {
        // gets the total unlocked weight through the current wave
        int totalWeight = bandit_weight;

        if (currentWave >= golemUnlockWave) {
            totalWeight += golem_weight;
        }
        
        if (currentWave >= ghostUnlockWave) {
            totalWeight += ghost_weight;
        }

        // get a random number to determine which enemy to spawn, then spawn it
        int roll = Random.Range(0, totalWeight);
        
        if (roll < bandit_weight) {
            SpawnEnemy(bandit, bandit_spawnPoints);
            return 1;
        }
        else if (roll < bandit_weight + golem_weight && currentWave >= golemUnlockWave) {
            SpawnEnemy(golem, golem_spawnPoints);
            return 2;
        }
        else if (currentWave >= ghostUnlockWave) {
            SpawnEnemy(ghost, ghost_spawnPoints);
            return 3;
        }
        return 4;
    }
    
    private void SpawnEnemy(GameObject prefab, List<Transform> spawnpoints)
    {
        if (spawnpoints.Count == 0) return;

        // if golem spawn, need to check if a golem is already on the spawn point before spawning
        if (prefab == golem) {
            int maxAttempts = 19;
            for (int i = 0; i < maxAttempts; i++) {
                int random_number = Random.Range(0, spawnpoints.Count);
                // the 11.8f is LIGHTS FADE IN + LIGHTS FADE OUT + THROW DELAY + AFTER THROW DELAY
                if (golem_inUse_time[random_number] + 7.8f <= Time.time) {
                    Transform spawnPoint = spawnpoints[random_number];
                    Instantiate (golem, spawnPoint.position, spawnPoint.rotation);
                    
                    golem_inUse_time[random_number] = Time.time;
                    return;
                }
            }
        }
        else {  
            Transform spawnPoint = spawnpoints[Random.Range(0, spawnpoints.Count)];
            GameObject enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            
            // have the speed be a little random
            if (prefab == bandit) {
                float speed = enemy.GetComponent<EnemyController>().agent.speed;
                enemy.GetComponent<EnemyController>().agent.speed = speed + Random.Range (-speed/8, speed/3);
            }
            else if (prefab == ghost) {
                float speed = enemy.GetComponent<GhostController>().agent.speed;
                enemy.GetComponent<GhostController>().agent.speed = speed + Random.Range (0, speed/3);
            }
        }
    }
}
