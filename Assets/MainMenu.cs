using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        if (CoinManager.instance != null)
        {
            // Reset game state
            CoinManager.instance.health = 3;
            CoinManager.instance.coinsCollected = 0;
            
            // Clear text references before scene change
            CoinManager.instance.textCoin = null;
            CoinManager.instance.textHealth = null;

            SceneManager.LoadScene("Level 1");
        }
    }
}