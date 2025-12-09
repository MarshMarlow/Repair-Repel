using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    public FootstepController footstepController;
    
    public Transform target; // target for nav mesh agent
    [SerializeField] private Animator m_Animator; // animator
    public Color damageColor; // change mat colour when damaged
    
    public int maxHealth = 1; // max health
    public int currentHealth; // current health
    
    public float deathDelay = 1.2f; // the delay between playing animation, then destroying the gameobject
    private bool dead = false; 
    private CapsuleCollider capsule; // for turning off after death
    
    private bool canMove = false; // for death and pause
    
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Material mat;
    [HideInInspector]
    public Color m_Color;
    

    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        m_Animator = GetComponentInChildren<Animator>();
        target = GameObject.FindWithTag("DoorTarget").transform;
        //agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        mat = GetComponentInChildren<Renderer>().material;
        m_Color = mat.color;
        currentHealth = maxHealth;
        agent.SetDestination(target.position);
    }

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        //if (canMove && !dead && agent.enabled) {
        //    footstepController
        //}
    }
    
    public bool isDead() {
        return dead;
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

    public IEnumerator TakeDamage(int damage) {
        if (dead) yield break;

        // if not dead, is moving, and can move, update colour to damaged colour
        if (agent.enabled && !dead && canMove) {
            agent.isStopped = true;
            mat.DOColor(damageColor, 0.1f);
            yield return new WaitForSeconds(0.1f);

            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;

            if (currentHealth - damage > 0) agent.ResetPath();
            mat.DOColor(m_Color, 0.1f);
        }
        
        if (currentHealth - damage < 0) currentHealth = 0;
        else currentHealth -= damage;
        
        // if dead, play death animation then destroy gameobject
        if (currentHealth <= 0 && !dead) {
            agent.isStopped = true;
            agent.enabled = false;
            dead = true;
            m_Animator.SetBool("isDead", dead);
            capsule.enabled = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            yield return new WaitForSeconds(deathDelay);
            Destroy(gameObject);
        }
        else if (!dead) {
            agent.isStopped = false;
        }
    }
}
