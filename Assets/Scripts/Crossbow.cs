using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public float arrowSpeed;
    public float fireRate; //seconds per arrow, ex. fireRate = 2 means 1 arrow / 2 seconds, fireRate = .5 means 1 arrow / .5 seconds

    public Transform arrowSpawnTransform;
    public GameObject arrowPrefab;
    private float timer;
    private ObjectGrabbable objectGrabbable;
    public GameObject crosshairUI; //REMOVE FOR VR
    
    void Awake()
    {
        objectGrabbable = GetComponent<ObjectGrabbable>();
    }
    void Update()
    {
        crosshairUI.SetActive(objectGrabbable.isHeld());
        if (timer > 0)
        {
            timer -= Time.deltaTime / fireRate;
        }

        //if(Input.GetKeyDown(KeyCode.E)  && objectGrabbable.isHeld() && timer <= 0 ) //if on laptop and u cant right click u can use this one
        if (Input.GetMouseButtonDown(1) && objectGrabbable.isHeld() && timer <= 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnTransform.position, Quaternion.LookRotation(arrowSpawnTransform.forward));
        arrow.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * arrowSpeed;
        //arrow.GetComponent<Rigidbody>().velocity = arrowSpawnTransform.forward * arrowSpeed;  THIS SHOULD BE THE VR ONE

        timer = 1;
    }
}
