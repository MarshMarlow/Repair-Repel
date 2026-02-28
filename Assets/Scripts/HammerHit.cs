// Class that tracks if a weapon is being hit by the hammer while it is on the anvil
// If hit by hammer while on anvil, increase durability 

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HammerHit : MonoBehaviour
{

    [SerializeField] private int repairPerHit = 5;

    [SerializeField] private bool requireAttackInput = false;

    public AudioSource audioSource;
    public AudioClip hammer_hit1;
    public AudioClip hammer_hit2;

    private bool IsAttackingNow()
    {
        if (!requireAttackInput) return true;

        return Input.GetMouseButton(0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsAttackingNow())
            return;

        WeaponDurability weapon = collision.collider.GetComponentInParent<WeaponDurability>();
        if (weapon == null) return;

        if (!weapon.IsOnAnvil)
            return;

        // play anvil sound
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        if (Random.value <= 0.5) {
            audioSource.PlayOneShot(hammer_hit1);    
        }
        else {
            audioSource.PlayOneShot(hammer_hit2);
        }
        weapon.Repair(repairPerHit);
    }
}
