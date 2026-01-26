// Class that allows the recharging of a the helmet lantern when placed on the table

using UnityEngine;

public class LampRecharge : MonoBehaviour
{
    [SerializeField] private float rechargeSecondsPerSecond = 3f;

    private LanternFlashlight currentLantern;

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

        currentLantern.RechargeSeconds(rechargeSecondsPerSecond * Time.deltaTime);
    }
}
