using UnityEngine;

[RequireComponent(typeof(ObjectGrabbable))]
public class WeaponDurability : MonoBehaviour
{
    [SerializeField] private int maxDurability = 10;
    [SerializeField] private int currentDurability = 10;

    public bool IsOnAnvil { get; private set; }

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
        if (IsOnAnvil && grabbable != null && grabbable.isHeld())
        {
            IsOnAnvil = false;
        }
    }

    public void SetOnAnvil(bool onAnvil)
    {
        if (onAnvil && grabbable != null && grabbable.isHeld())
            return;

        IsOnAnvil = onAnvil;
    }

    public void Repair(int amount)
    {
        currentDurability = Mathf.Clamp(currentDurability + amount, 0, maxDurability);
    }

    public void Damage(int amount)
    {
        currentDurability = Mathf.Clamp(currentDurability - amount, 0, maxDurability);
    }

    public bool IsBroken()
    {
        return currentDurability <= 0;
    }

    public bool IsFull()
    {
        return currentDurability >= maxDurability;
    }

    public bool CanBlock()
    {
        return !IsBroken();
    }
}