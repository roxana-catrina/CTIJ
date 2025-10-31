using UnityEngine;
using System.Collections;

public class PortalActivate : MonoBehaviour
{
    public GameObject fireballInPortal;
    public GameObject portalLightEntry;
    private bool activated = false;

    [Header("Portal Sound")]
    [SerializeField] private AudioClip portalOpenSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null && inventory.hasFireball)
            {
                StartCoroutine(ActivatePortal());
                activated = true;
            }
        }
    }

    private IEnumerator ActivatePortal()
    {
        if (portalOpenSound != null)
            AudioHelper.PlayClipAtPoint(portalOpenSound, transform.position);
            
        if (fireballInPortal != null)
        {
            fireballInPortal.SetActive(true);
            yield return new WaitForSeconds(0.5f); // 🔹 doar 0.2 secunde (foarte scurt)
            fireballInPortal.SetActive(false);
        }

        if (portalLightEntry != null)
            portalLightEntry.SetActive(true);

        Debug.Log("Portalul de intrare a fost activat!");
    }
}
