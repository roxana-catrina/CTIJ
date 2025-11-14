using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Over Settings")]
    public AudioClip gameOverSound;       // sunetul care va fi redat
    public float soundDelayTime = 2f;     // timpul până se aude sunetul (2 secunde)
    
    private AudioSource audioSource;

    void Awake()
    {
        // Obține sau adaugă componenta AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        // Verifică dacă este scena de Game Over
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            StartCoroutine(PlayGameOverSound());
        }
    }

    IEnumerator PlayGameOverSound()
    {
        // Așteaptă 2 secunde
        yield return new WaitForSeconds(soundDelayTime);

        // Redă sunetul
        if (gameOverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
    }

    public void RestartGame()
    {
        if (CoinManager.instance != null)
        {
            CoinManager.instance.health = 3;
            CoinManager.instance.coinsCollected = 0;
            CoinManager.instance.item1 = CoinManager.instance.item1restart;
            CoinManager.instance.item2 = CoinManager.instance.item2restart;
        }

        // Obține nivelul curent salvat
        string levelToLoad = CoinManager.instance != null && !string.IsNullOrEmpty(CoinManager.instance.currentLevel) 
            ? CoinManager.instance.currentLevel 
            : "Level 1";

        // Înregistrează callback-ul
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(levelToLoad);
    }

    internal void SetActive(bool v)
    {
        throw new NotImplementedException();
    }

    [Obsolete]
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verifică dacă este un nivel de joc (Level 1 sau Level 2)
        if (scene.name.StartsWith("Level"))
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
