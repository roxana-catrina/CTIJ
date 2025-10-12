using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddCoin(); // Adaugă o monedă la scor
            Destroy(gameObject); // Distruge moneda
        }
    }
}