using UnityEngine;

public class LampRecharge : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private float interactDistance = 3.5f;
    [SerializeField] private LayerMask interactLayerMask; // workbench layer
    [SerializeField] private EquipLantern equipLantern;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse1)) return;

        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
            out RaycastHit hit, interactDistance, interactLayerMask))
        {
            // If we clicked a workbench, recharge equipped lantern, if lantern equipped
            var lantern = equipLantern.GetEquippedLanternFlashlight();
            if (lantern == null) return;

            lantern.RechargeFull();
        }
    }
}
