// Class that allows the recharging of a the helmet lantern when placed on the table

using UnityEngine;

public class LampRecharge : MonoBehaviour
{
    [SerializeField] private float rechargeSecondsPerSecond = 3f;

    private LanternFlashlight currentLantern;
    public AudioSource audiosource;
    public AudioClip charge;
    public  float soundInterval = 0.1f;
    private float nextSoundTime = 0f;

    private void Start() {
        //soundInterval = charge.length;
        //Debug.Log(soundInterval);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out LanternFlashlight lantern))
        {
            currentLantern = lantern;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentLantern != null && other.GetComponent<LanternFlashlight>() == currentLantern)
        {
            currentLantern = null;
        }
    }

    private void Update()
    {
        if (currentLantern == null) return;

        if (currentLantern.getRemainingSeconds() < currentLantern.getMaxSeconds()) {
            if (Time.time >= nextSoundTime) {
                audiosource.PlayOneShot(charge);
                nextSoundTime = Time.time + soundInterval;
            }
        }
        else {
            nextSoundTime = 0f;
        }
        currentLantern.RechargeSeconds(rechargeSecondsPerSecond * Time.deltaTime);
    }
}
