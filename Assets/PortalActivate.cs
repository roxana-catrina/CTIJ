using UnityEngine;

public class PortalActivate : MonoBehaviour
{
    public GameObject fireballInPortal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                if (inventory.hasFireball)
                {
                    Debug.Log("Portal activat! 🔥");
                    if (fireballInPortal != null)
                    {
                        fireballInPortal.SetActive(true); // 🔹 sfera apare în portal
                    }
                }
                else
                {
                    Debug.Log("Nu ai sfera de foc încă! 🔒");
                }
            }
        }
    }
}
