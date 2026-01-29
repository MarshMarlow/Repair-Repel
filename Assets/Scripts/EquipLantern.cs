// Class that allows the equipping of a helmet lantern to a head mount point using the key 'Q'
// Also includes a feature to drop the helmet lantern on a table using 'Q' (The table it is currently on)

using UnityEngine;

public class EquipLantern : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private float equipDistance = 3.5f;
    [SerializeField] private LayerMask lanternLayerMask;

    [SerializeField] private Transform headMountPoint; // mounting point

    [SerializeField] private LayerMask tableLayerMask;      // interactable layer
    [SerializeField] private float dropCheckDistance = 3.0f;
    [SerializeField] private Transform tableDropPoint;      // assigned to table_3


    [SerializeField] private KeyCode equipKey = KeyCode.Q;

    private ObjectGrabbable equippedLantern;

    private void Update()
    {
        if (Input.GetKeyDown(equipKey))
        {
            if (equippedLantern == null)
                TryEquipFromLook();
            else
                TryDropOnTable();
        }
    }

    private void TryEquipFromLook() // equips the lantern on the head mount
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
            out RaycastHit hit, equipDistance, lanternLayerMask))
        {
            ObjectGrabbable grabbable = hit.transform.GetComponentInParent<ObjectGrabbable>();
            if (grabbable == null) return;

            if (grabbable.GetComponent<LanternFlashlight>() == null) return;

            equippedLantern = grabbable;
            equippedLantern.Grab(headMountPoint);
        }
    }

    private void TryDropOnTable()
    {
        if (equippedLantern == null) return;

        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
            out RaycastHit hit, dropCheckDistance, tableLayerMask))
        {
            LanternFlashlight lf = equippedLantern.GetComponent<LanternFlashlight>();
            if (lf != null)
            {
                lf.SetOn(false);
            }

            equippedLantern.Drop();
            // snaps to mount point on table
            Transform lanternTransform = equippedLantern.transform;
            lanternTransform.position = tableDropPoint.position;
            lanternTransform.rotation = tableDropPoint.rotation;

            equippedLantern = null;
        }
        else
        {
            Debug.Log("Not looking at table, cannot drop lantern.");
        }
    }

    public LanternFlashlight GetEquippedLanternFlashlight()
    {
        if (equippedLantern == null) return null;
        return equippedLantern.GetComponent<LanternFlashlight>();
    }

    public bool HasEquippedLantern() => equippedLantern != null;
}
