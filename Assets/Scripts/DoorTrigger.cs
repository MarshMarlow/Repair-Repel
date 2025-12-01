using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    CastleHealth castleHealth;

    void Start() {
        castleHealth = GetComponent<CastleHealth>();
    }
    
    // when the bandit enemies run through the castle doors, decrease the castle hp
    private void OnTriggerEnter(Collider other) {
        // DECREASE CASTLE HP WHEN ENTERED 
        castleHealth.StartCoroutine(castleHealth.Damage(1));
        Destroy(other.gameObject);
    }
}
