using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyThrow : MonoBehaviour {
    [SerializeField] private Animator m_Animator; // animator
    public bool canThrow = false; // to start the throwing action
    public bool canMove = true; // whether it can continue its actions (for pausing or opening up an ingame menu)
    private bool isThrowing = false; // whether it is in the middle of throwing
    public float throwDelay = 0.8f; // the time to get to the peak of throwing (summon projectile)
    public float afterThrowDelay = 1.0f; // the time to wait after the peak
    public LightFades[] lightFades; // to adjust the lighting on the golem after the throw
    public Transform target; // the player target
    private Transform tomatoSpawn; //spawnpoint of tomato projectile
    public AudioClip throwSound;
    public AudioSource audioSource;
    public GameObject tomatoPrefab;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayThrowSound() {
        audioSource.PlayOneShot(throwSound);
    }
    
    void Start() {
        target = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
        lightFades = GetComponentsInChildren<LightFades>();
        m_Animator = GetComponentInChildren<Animator>();
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);
        tomatoSpawn = transform.GetChild(0);
    }
    
    void Update() {
        // throw when golem lights has been fully activated
        if (canMove && canThrow && !isThrowing) {
            StartCoroutine(ThrowProjectile());
            isThrowing = true;
        }
    }

    // throws projectile when animation reaches its peak
    private IEnumerator ThrowProjectile() {
        m_Animator.SetBool("isThrowing", true);
        audioSource.PlayOneShot(throwSound);
        yield return new WaitForSeconds(throwDelay);
        
        // SUMMON PROJECTILE
        Instantiate(tomatoPrefab, tomatoSpawn.position, tomatoSpawn.rotation);
        
        yield return new WaitForSeconds(afterThrowDelay);
        m_Animator.SetBool("isThrowing", false);
        foreach (LightFades lightFade in lightFades) {
            lightFade.StartCoroutine(lightFade.FadeOutLights());
        }
    }

    public void DestroyItself() {
        Destroy(this.gameObject);
    }
}
