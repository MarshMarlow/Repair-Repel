using UnityEngine;

public class EquipLantern : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private float equipDistance = 3.5f;
    [SerializeField] private LayerMask lanternLayerMask; // put lanterns on this layer (or reuse Pickup)

    [Header("Mount Point")]
    [SerializeField] private Transform headMountPoint; // empty object under camera

    [Header("Input")]
    [SerializeField] private KeyCode equipKey = KeyCode.Q;

    private ObjectGrabbable equippedLantern;

    private void Update()
    {
        if (Input.GetKeyDown(equipKey))
        {
            if (equippedLantern == null)
                TryEquipFromLook();
            else
                DropEquipped();
        }
    }

    private void TryEquipFromLook()
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
            out RaycastHit hit, equipDistance, lanternLayerMask))
        {
            // If ray hits a child collider, find ObjectGrabbable on parent
            ObjectGrabbable grabbable = hit.transform.GetComponentInParent<ObjectGrabbable>();
            if (grabbable == null) return;

            // Only allow equipping objects that are actually lanterns
            if (grabbable.GetComponent<LanternFlashlight>() == null) return;

            equippedLantern = grabbable;
            equippedLantern.Grab(headMountPoint);
        }
    }

    private void DropEquipped()
    {
        if (equippedLantern == null) return;

        equippedLantern.Drop();
        equippedLantern = null;
    }

    public LanternFlashlight GetEquippedLanternFlashlight()
    {
        if (equippedLantern == null) return null;
        return equippedLantern.GetComponent<LanternFlashlight>();
    }

    public bool HasEquippedLantern() => equippedLantern != null;
}
