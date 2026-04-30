// Class that allows the equipping of a helmet lantern to a head mount point using the key 'Q'
// Also includes a feature to drop the helmet lantern on a table using 'Q' (The table it is currently on)

using UnityEngine;

public class EquipLantern : MonoBehaviour
{
    [SerializeField] private Transform leftControllerTransform;
    [SerializeField] private Transform rightControllerTransform;
    [SerializeField] private float equipDistance = 3.5f;
    [SerializeField] private LayerMask lanternLayerMask;

    [SerializeField] private Transform headMountPoint; // mounting point

    [SerializeField] private LayerMask tableLayerMask;      // interactable layer
    [SerializeField] private float dropCheckDistance = 3.0f;
    [SerializeField] private Transform tableDropPoint;      // assigned to table_3


    [SerializeField] private OVRInput.RawButton leftEquipButton = OVRInput.RawButton.Y;

    [SerializeField] private OVRInput.RawButton rightEquipButton = OVRInput.RawButton.B;

    private ObjectGrabbable equippedLantern;
    public AudioSource audioSource;
    public AudioClip put_on;
    public AudioClip put_off;
    

    private void Update()
    {
        if (OVRInput.GetDown(leftEquipButton))
        {
            if (equippedLantern == null)
                TryEquipFromLook(leftControllerTransform);
            else
                TryDropOnTable(leftControllerTransform);
        }
        else if(OVRInput.GetDown(rightEquipButton))
        {
            if (equippedLantern == null)
                TryEquipFromLook(rightControllerTransform);
            else
                TryDropOnTable(rightControllerTransform);
        }
    }

    private void TryEquipFromLook(Transform controller) // equips the lantern on the head mount
    {
        if (Physics.Raycast(controller.position, controller.forward,
            out RaycastHit hit, equipDistance, lanternLayerMask))
        {
            ObjectGrabbable grabbable = hit.transform.GetComponentInParent<ObjectGrabbable>();
            if (grabbable == null) return;

            if (grabbable.GetComponent<LanternFlashlight>() == null) return;

            equippedLantern = grabbable;
            audioSource.PlayOneShot(put_on);
            equippedLantern.Grab();
        } else
        {
            Debug.Log("RAY MISS");
        }
    }

    private void TryDropOnTable(Transform controller)
    {
        if (equippedLantern == null) return;

        if (Physics.Raycast(controller.position, controller.forward,
            out RaycastHit hit, dropCheckDistance, tableLayerMask))
        {
            LanternFlashlight lf = equippedLantern.GetComponent<LanternFlashlight>();
            if (lf != null)
            {
                lf.SetOn(false);
            }

            audioSource.PlayOneShot(put_off);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;        
        Gizmos.DrawRay(leftControllerTransform.position, leftControllerTransform.forward * equipDistance);
        Gizmos.DrawRay(rightControllerTransform.position, rightControllerTransform.forward * equipDistance);
    }
}
