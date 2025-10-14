using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RestartGame()
    {
        if(CoinManager.instance != null)
    {
            CoinManager.instance.health = 3;
            CoinManager.instance.coinsCollected = 0;
        }

        SceneManager.LoadScene("Level 1");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
