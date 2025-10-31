using UnityEngine;

public class DiamondCollect : MonoBehaviour
{
    [SerializeField] private AudioClip diamondSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("DiamondCollect: Collision detectată cu " + other.name + " (tag: " + other.tag + ")");
        
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.hasDiamond = true;
                Debug.Log("Ai colectat diamantul!");


                if (diamondSound != null)
                    AudioHelper.PlayClipAtPoint(diamondSound, transform.position);


                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("PlayerInventory component nu există pe " + other.name);
            }
        }
    }

    private void Start()
    {
        // Verificare la început
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
            Debug.LogError("Diamond nu are Collider2D!");
        else if (!col.isTrigger)
            Debug.LogWarning("Diamond Collider nu are Is Trigger bifat!");
    }
}