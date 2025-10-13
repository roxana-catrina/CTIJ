using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int coinsCollected = 0;
    public int health = 3;  // Starting health
    public TextMeshProUGUI textCoin;
    public TextMeshProUGUI textHealth;  // Reference to health UI text

    void Awake()
    {
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
            // Handle game over here
            Debug.Log("Game Over!");
        }
    }

    private void Update()
    {
        textCoin.text = "Coins: " + coinsCollected.ToString();
        if (textHealth != null)
        {
            textHealth.text = "Health: " + health.ToString();
        }
    }
}
