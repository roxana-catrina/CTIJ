using UnityEngine;
using UnityEngine.SceneManagement; // pentru schimbarea scenei

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // verificăm dacă obiectul care a intrat în trigger este player-ul
        if (other.CompareTag("Player"))
        {
            // trecem la scena Level 2
            SceneManager.LoadScene("Level 2");
        }
    }
}

