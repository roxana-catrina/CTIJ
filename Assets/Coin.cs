using UnityEngine;

public class Coin : MonoBehaviour
{   CoinManager cm;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.instance.AddCoin(); // Adaugă o monedă la scor
            Destroy(gameObject); // Distruge moneda
        }
    }
}