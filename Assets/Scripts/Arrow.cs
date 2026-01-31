using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;
    public float lifeTime;
    private float gravityMultiplier = 0.3f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        lifeTime -= Time.deltaTime;
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.StartCoroutine(enemy.TakeDamage(damage));
        }

        Destroy(gameObject);
    }
}
