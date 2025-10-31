using UnityEngine;

public class FireballCollect : MonoBehaviour
{
    [SerializeField] private AudioClip fireballSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.hasFireball = true;
                Debug.Log("Ai colectat sfera de foc! 🔥");

                if (fireballSound != null)
                    AudioHelper.PlayClipAtPoint(fireballSound, transform.position);

                // poți adăuga un efect vizual aici, dacă vrei
                Destroy(gameObject); // sfera dispare după colectare
            }
        }
    }
}
