using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrucifixAttack : MonoBehaviour
{
    public int damagePerSecond = 1;          
    public float attackRange = 20f;             // how far the cross can hit
    public float coneAngle = 10f;               // around 5–15 degrees
    public LayerMask ghostLayer;
    private Transform playerCam;
    private GhostController currentTarget;
    public ObjectGrabbable objectGrabbable;
    
    void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        objectGrabbable = GetComponent<ObjectGrabbable>();
    }
    
    void Update()
    {
        if (IsHeld() == false) {
            // stop continuous damage
            currentTarget = null;
            return;
        }

        // draws the cone rays of the crucifix attack
        Debug.DrawRay(transform.position, transform.forward * attackRange, Color.white);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, coneAngle, 0) * transform.forward * attackRange, Color.magenta);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -coneAngle, 0) * transform.forward * attackRange, Color.magenta);
        DoCrucifixAttack();
    }
    
    private bool IsHeld() {
        return objectGrabbable.isHeld();
    }

    private void DoCrucifixAttack() {
        Vector3 direction = transform.forward;
        direction.y = 0;
        direction.Normalize();
        
        GhostController bestCandidate = null;
        float bestDistance = 99999f;
        float cosThreshold = Mathf.Cos(coneAngle * Mathf.Deg2Rad); // convert to dot product threshold for comparing whether the ghost is within cone angle
        
        // find all ghosts within range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, ghostLayer);
        
        foreach (var collider in hitColliders) {
            GhostController enemy = collider.GetComponent<GhostController>();
            if (enemy == null || enemy.isDead()) continue; // if enemy doesn't exist or is dead, get next ghost

            // get the distance from crucifix to target
            Vector3 toTarget = collider.transform.position - transform.position;
            toTarget.y = 0;
            float dist = toTarget.magnitude;

            Vector3 normalized_dist = toTarget.normalized;
            float dot = Vector3.Dot(direction, normalized_dist);
            
            // if the target is outside of the cone
            if (dot < cosThreshold || dot <= 0f) continue;
            
            // if theres a shorter distance
            if (dist < bestDistance) {
                bestDistance = dist;
                bestCandidate = enemy;
            }
        }

        // if new target, stop the continuous damage on the current target then switch target
        if (bestCandidate != currentTarget) {
            if (currentTarget != null) {
                currentTarget.StopContinuousDamage();
            }

            currentTarget = bestCandidate;
        }

        // continuous damage to the target
        if (currentTarget != null) {
            currentTarget.ApplyContinuousDamage(1);
        }
    }
}
