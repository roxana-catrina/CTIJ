using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void RestartGame()
    {
        if (CoinManager.instance != null)
        {
            CoinManager.instance.health = 3;
            CoinManager.instance.coinsCollected = 0;
        }

        // Înregistrează callback-ul
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Level 1");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level 1")
        {
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            if (player != null)
            {
                player.ResetAppearance();
            }

            // Deregistrăm callback-ul pentru a nu-l apela de mai multe ori
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
