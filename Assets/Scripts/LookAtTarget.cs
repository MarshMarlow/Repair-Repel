using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour {
    public Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("DoorTarget").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            60 * Time.deltaTime
        );
    }
}
