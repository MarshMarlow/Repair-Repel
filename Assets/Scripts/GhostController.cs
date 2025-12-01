using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class GhostController : MonoBehaviour
{
    public Transform target; // target for nav mesh agent
    public Color damageColor; // change mat colour when damaged
    public float maxHealth = 1f;
    public float currentHealth;
    
    private bool canMove = true; // for death and pause
    private bool dead = false;
    
    public float deathDelay = 1.25f; // the delay between playing animation, then destroying the gameobject
    
    private CapsuleCollider capsule; // for turning off after death
    
    // for taking damage timers
    private bool isTakingContinuousDamage = false;
    private float damageTickRate = 0.5f; // damage every 0.5 seconds
    private float lastDamageTime = 0f;

    // particle to spawn in when dead
    public GameObject deathParticlePrefab;

    public GameObject enemyModel;
    
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Material[] mats;
    [HideInInspector]
    public Renderer[] renderers;
    [HideInInspector]
    public Color[] originalColours;
    [HideInInspector]
    public Color m_Color;
    
    // Start is called before the first frame update
    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        target = GameObject.FindWithTag("PlayerTarget").transform;
        agent = GetComponent<NavMeshAgent>();
        renderers = GetComponentsInChildren<Renderer>();
        List<Material> matList = new List<Material>();
        
        // Look towards player
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);

        // get all the mats in the ghost prefab
        foreach (Renderer renderer in renderers) {
            // don't include the lantern materials
            if (renderer.gameObject.name == "Latern2" || renderer.gameObject.name == "Candle2.001" ||
                renderer.gameObject.name == "Glass2" || renderer.gameObject.name == "red_flame_0 (1)") continue;
            matList.AddRange(renderer.materials);
        }
        
        mats = matList.ToArray();

        // get all the colours in the ghost prefab
        originalColours = new Color [mats.Length];
        for (int i = 0; i < mats.Length; i++) {
            originalColours[i] = mats[i].color;
        }
        
        currentHealth = maxHealth;
        agent.SetDestination(target.position);
    }
    
    
    // for pausing or opening up shop
    public void stopMoving() {
        if (!dead) {
            agent.enabled = false;
            agent.isStopped = true;
            canMove = false;
        }
    }

    // for pausing or opening up shop
    public void resumeMoving() {
        if (!dead) {
            agent.enabled = true;
            agent.isStopped = false;
            canMove = true;
        }
    }
    
    public bool isDead() {
        return dead;
    }

    public void ApplyContinuousDamage(int damage) {
        if (dead) return;

        // applies damage every 0.5s
        if (Time.time - lastDamageTime >= damageTickRate) {
            lastDamageTime = Time.time;
            TakeDamage(damage);
        }
        
        // applies the damage colour material
        if (!isTakingContinuousDamage) {
            isTakingContinuousDamage = true;
            ApplyDamageVisuals();
        }
    }
    
    public void StopContinuousDamage() {
        isTakingContinuousDamage = false;
        RemoveDamageVisuals();
    }

    // takes damage. If health is 0, kill it
    private void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
    
        if (currentHealth == 0 && !dead) {
            agent.isStopped = true;
            agent.enabled = false;
            dead = true;
            capsule.enabled = false;
            StartCoroutine(Die());
        }
    }

    // spawn a particle before dying
    private IEnumerator Die() {
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        GameObject particle = Instantiate(deathParticlePrefab, transform.position, rotation);
        enemyModel.SetActive(false);
        yield return new WaitForSeconds(deathDelay);
        Destroy(particle);
        Destroy(gameObject);
    }

    // when hit, change to damaged material colour and reduce speed
    private void ApplyDamageVisuals() {
        if (agent.enabled && canMove) {
            agent.speed = agent.speed - 0.3f;
            foreach (Material mat in mats) {
                mat.DOColor(damageColor, 0.15f);
            }
        }
    }
    
    // when not hit, change back to normal colour and change speed back to default
    private void RemoveDamageVisuals() {
        for (int i = 0; i < mats.Length; i++) {
            mats[i].DOColor(originalColours[i], 0.15f);
        }

        if (agent.enabled && !dead) {
            agent.speed = agent.speed + 0.3f;
        }
    }
    
    // when the ghost collides into player, game over
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerTarget")) {
            Debug.Log("Killed Player; Game Over");
            // GAME OVER
        }
    }
}
