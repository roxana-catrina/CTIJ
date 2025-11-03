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

                // Ascunde bila din UI
                HideFireballUI();

                // poți adăuga un efect vizual aici, dacă vrei
                Destroy(gameObject); // sfera dispare după colectare
            }
        }
    }

    private void HideFireballUI()
    {
        // Caută bila din UI și ascunde-o
        GameObject fireballUI = GameObject.Find("FireballUI"); // Înlocuiește cu numele exact al GameObject-ului tău
        if (fireballUI != null)
        {
            fireballUI.SetActive(false);
            Debug.Log("Bila din UI a fost ascunsă!");
        }
        else
        {
            Debug.LogWarning("FireballUI nu a fost găsit! Verifică numele GameObject-ului din UI.");
        }
    }
}
