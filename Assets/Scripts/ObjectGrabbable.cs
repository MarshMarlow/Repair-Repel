using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    public bool autoOrient = true;
    private Transform playerTransform;
    
    public void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        objectRigidbody = GetComponent<Rigidbody>();
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        
        // if auto orient, will stand upright and will rotate based on vr controller
        if (autoOrient) {
            // for vr controls
            //Vector3 currentEuler = objectGrabPointTransform.rotation.eulerAngles;
            //transform.rotation = Quaternion.Euler(0f, currentEuler.y, 0f);
            
            // for current first person mouse controls
            Vector3 currentEuler = playerTransform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, currentEuler.y, 0f);
            //transform.rotation = Quaternion.identity;
        }
        objectRigidbody.useGravity = false;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPos = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPos);
            
            // rotates the object currently based on where the player is looking - can change it to objectGrabPointTransform rotation once we integrate.
            Vector3 currentEuler = playerTransform.rotation.eulerAngles;
            if(gameObject.CompareTag("Shield"))
            {
                transform.rotation = Quaternion.Euler(90f, currentEuler.y, 0f);
            } else {
                transform.rotation = Quaternion.Euler(0f, currentEuler.y, 0f);
            }
        }
    }

    // return whether the object is currently being held
    public bool isHeld() {
        return objectGrabPointTransform != null;
    }
}
