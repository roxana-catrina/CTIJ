using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // Singleton
    public int coinsCollected = 0; // Numărul de monede colectate
    public int health = 3; // Starting health
    public TextMeshProUGUI textCoin; // Referință la UI Text pentru afișarea monedelor
    public TextMeshProUGUI textHealth; // Reference to health display

    void Awake()
    {
        // Asigură-te că există o singură instanță
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin()
    {
        coinsCollected++;
        Debug.Log("Monede colectate: " + coinsCollected);
    }

    public void TakeDamage()
    {
        health--;
        Debug.Log("Health remaining: " + health);

        if (health <= 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void Update()
    {
        if (textCoin != null)
        {
            textCoin.text = "Coins: " + coinsCollected.ToString();
        }
        
        if (textHealth != null)
        {
            textHealth.text = "Health: " + health.ToString();
        }
    }
}
