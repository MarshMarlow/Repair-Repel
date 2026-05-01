using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public float arrowSpeed;
    public float fireRate; //seconds per arrow, ex. fireRate = 2 means 1 arrow / 2 seconds, fireRate = .5 means 1 arrow / .5 seconds

    public Transform arrowSpawnTransform;
    public GameObject arrowPrefab;
    private float timer;
    private ObjectGrabbable objectGrabbable;
    // public GameObject crosshairUI; //REMOVE FOR VR

    private bool playedReloadSound = true;
    private WeaponDurability weaponDurability;

    
    public AudioSource audioSource;
    public AudioClip shootingSound;
    public AudioClip reloadSound;
    
    void Awake()
    {
        objectGrabbable = GetComponent<ObjectGrabbable>();
        weaponDurability = GetComponent<WeaponDurability>();
    }
    void Update()
    {
        // crosshairUI.SetActive(objectGrabbable.isHeld());
        if (timer > 0)
        {
            timer -= Time.deltaTime / fireRate;
        }

        if (timer <= reloadSound.length + 10 && !playedReloadSound) {
            audioSource.volume = 0.1f;
            audioSource.PlayOneShot(reloadSound);
            playedReloadSound = true;
        }

        if(((objectGrabbable.isHeldByLeft() && OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        || (objectGrabbable.isHeldByRight() && OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)))
        && timer <= 0 && weaponDurability.CurrentDurability > 0) {
                audioSource.volume = 0.3f;
                audioSource.PlayOneShot(shootingSound);
                Shoot();
                weaponDurability.Damage(1);
                playedReloadSound = false;
            }
        }

    void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnTransform.position, Quaternion.LookRotation(arrowSpawnTransform.forward));
        // arrow.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * arrowSpeed;
        arrow.GetComponent<Rigidbody>().velocity = arrowSpawnTransform.forward * arrowSpeed;  // THIS SHOULD BE THE VR ONE

        timer = 1;
    }
}
