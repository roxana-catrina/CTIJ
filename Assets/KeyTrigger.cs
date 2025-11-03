using UnityEngine;

public class KeyTrigger : MonoBehaviour
{
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject openDoor;
    [SerializeField] private AudioClip doorOpenSound;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null && inventory.hasDiamond && inventory.hasFireball)
            {
                // ✅ Toate obiectele colectate → se deschide poarta
                if (closedDoor != null && openDoor != null)
                {
                    closedDoor.SetActive(false);
                    openDoor.SetActive(true);

                    // 🔊 Sunet opțional de deschidere poartă
                    if (doorOpenSound != null)
                        AudioHelper.PlayClipAtPoint(doorOpenSound, transform.position);
                }

                // Distruge cheia (opțional)
                Destroy(gameObject);
            }
        }
    }
}
