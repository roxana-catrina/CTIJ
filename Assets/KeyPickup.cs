using UnityEngine;
using UnityEngine.SceneManagement; // pentru schimbarea scenei

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // verificăm dacă obiectul care a intrat în trigger este player-ul
        if (other.CompareTag("Player"))
        {
            if(CoinManager.instance.maps==1 && CoinManager.instance.potions==1 && CoinManager.instance.coinsCollected==2)  
            // trecem la scena Level 2
            SceneManager.LoadScene("Level 2");
        }
    }
}

