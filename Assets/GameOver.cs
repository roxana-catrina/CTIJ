using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RestartGame()
    { 
        if (CoinManager.instance != null)
        {
            CoinManager.instance.health = 3;
            CoinManager.instance.coinsCollected = 0;
        }

        SceneManager.LoadScene("Level 1");

        // Opțional: dacă vrei să apelezi ResetAppearance imediat după încărcare
        // poți folosi un callback OnSceneLoaded
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Găsește Playerul și resetează vizualul
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            PlayerMovement pm = playerObj.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.ResetAppearance();
            }
        }

        // Deconectează callback-ul ca să nu fie apelat de mai multe ori
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
