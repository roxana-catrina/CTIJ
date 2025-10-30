using UnityEngine;

public class Coin : MonoBehaviour
{
    CoinManager cm;
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.instance.AddCoin(); // Adaugă o monedă la scor
            AudioHelper.PlayClipAtPoint(collectSound, transform.position);
            Destroy(gameObject);

        }
    }
}