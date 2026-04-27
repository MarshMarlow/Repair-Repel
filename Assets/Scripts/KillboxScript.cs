using UnityEngine;

public class KillboxScript : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        ReturnToSpawn respawn = other.GetComponent<ReturnToSpawn>();
        if (respawn != null)
        {
            respawn.Respawn();
        }
    }
}