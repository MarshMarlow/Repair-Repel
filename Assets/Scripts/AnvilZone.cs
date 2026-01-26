// Tracks if weapon is on anvil
// Used to determine whether or not hammer hits should be registered

using UnityEngine;

public class AnvilZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WeaponDurability weapon))
        {
            weapon.SetOnAnvil(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out WeaponDurability weapon))
        {
            weapon.SetOnAnvil(false);
        }
    }
}
