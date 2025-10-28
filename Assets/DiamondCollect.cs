using UnityEngine;

public class DiamondCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.hasDiamond = true;
                Debug.Log("Ai colectat diamantul!");
                Destroy(gameObject); // dispare diamantul
            }
        }
    }
}
