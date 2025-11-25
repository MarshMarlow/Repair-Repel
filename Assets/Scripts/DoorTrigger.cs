using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    // when the bandit enemies run through the castle doors, decrease the castle hp
    private void OnTriggerEnter(Collider other) {
        // DECREASE CASTLE HP WHEN ENTERED 
        Destroy(other.gameObject);
    }
}
