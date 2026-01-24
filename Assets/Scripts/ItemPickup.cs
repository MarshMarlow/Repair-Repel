using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private LayerMask interactLayerMask;


    private ObjectGrabbable objectGrabbable;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (objectGrabbable == null)
            {
                float pickupDistance = 3.5f;
                if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance, pickupLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }
                }
            } 
        } 
        else
        {
            if (objectGrabbable != null)
            {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }

        // right click to recharge lamp while holding it
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        float interactDistance = 3.5f;

        if (Physics.Raycast(playerCameraTransform.position,
                            playerCameraTransform.forward,
                            out RaycastHit hit,
                            interactDistance,
                            interactLayerMask))
        {
            if (objectGrabbable == null) return;

            LanternFlashlight lantern =
                objectGrabbable.GetComponent<LanternFlashlight>();

            if (lantern != null)
            {
                lantern.RechargeFull(); // call function to recharge the lantern
            }
        }
    }
}
