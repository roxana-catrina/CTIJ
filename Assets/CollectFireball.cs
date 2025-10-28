using UnityEngine;

public class FireballCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.hasFireball = true;
                Debug.Log("Ai colectat sfera de foc! 🔥");

                // poți adăuga un efect vizual aici, dacă vrei
                Destroy(gameObject); // sfera dispare după colectare
            }
        }
    }
}
