using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public Transform teleportDestination; // unde se teleportează
    public GameObject linkedPortalLight;  // lumina din portalul destinație

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // teleportează instant playerul
            other.transform.position = teleportDestination.position;

            // asigură-te că lumina de la destinație e aprinsă
            if (linkedPortalLight != null)
                linkedPortalLight.SetActive(true);

            Debug.Log("Player teleportat!");
        }
    }
}
