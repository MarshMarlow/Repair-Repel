using UnityEngine;

[RequireComponent(typeof(ObjectGrabbable))]
public class WeaponDurability : MonoBehaviour
{
    [SerializeField] private int maxDurability = 10;
    [SerializeField] private int currentDurability = 10;

    public bool IsOnAnvil { get; private set; } // tracks if weapon is on anvil

    private ObjectGrabbable grabbable;

    public int CurrentDurability => currentDurability;
    public int MaxDurability => maxDurability;

    private void Awake()
    {
        grabbable = GetComponent<ObjectGrabbable>();
        currentDurability = Mathf.Clamp(currentDurability, 0, maxDurability);
    }

    private void Update()
    {
        if (IsOnAnvil && grabbable != null && grabbable.isHeld()) //if picked up, then no longer on anvil
        {
            IsOnAnvil = false;
        }
    }

    public void SetOnAnvil(bool onAnvil)
    {
        if (onAnvil && grabbable != null && grabbable.isHeld()) // not on anvil
            return;

        IsOnAnvil = onAnvil; // on anvil
    }

    public void Repair(int amount)
    {
        currentDurability = Mathf.Clamp(currentDurability + amount, 0, maxDurability);
    }

    public void Damage(int amount)
    {
        currentDurability = Mathf.Clamp(currentDurability - amount, 0, maxDurability);
    }

    public bool IsBroken() => currentDurability <= 0;
    public bool IsFull() => currentDurability >= maxDurability;
}
