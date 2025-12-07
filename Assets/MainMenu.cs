#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Story");
    }

    public void ReplayGame()
    {
        // Oprește muzica surpriză și repornește muzica de fundal
        SurpriseMusic surpriseMusic = FindFirstObjectByType<SurpriseMusic>();
        if (surpriseMusic != null)
        {
            surpriseMusic.StopSurpriseAndRestoreOriginal();
        }

        // Oprește toate sunetele din scena curentă (inclusiv melodia din FinalScene)
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in allAudioSources)
        {
            // Oprește doar audio-urile care nu sunt MusicManager (muzica de fundal)
            if (audio.GetComponent<MusicManager>() == null)
            {
                audio.Stop();
            }
        }

        // Repornește muzica de fundal din MusicManager
        MusicManager musicManager = FindFirstObjectByType<MusicManager>();
        if (musicManager != null)
        {
            AudioSource bgMusic = musicManager.GetComponent<AudioSource>();
            if (bgMusic != null && !bgMusic.isPlaying)
            {
                bgMusic.Play();
            }
        }

        // Resetează datele jocului
        if (CoinManager.instance != null)
        {
            CoinManager.instance.health = 3;
            CoinManager.instance.coinsCollected = 0;
            CoinManager.instance.item1 = 0;
            CoinManager.instance.item2 = 0;
        }

        // Încarcă prima scenă a jocului
        SceneManager.LoadScene("Story");
    }
    
     public void QuitGame()
    {
        Debug.Log("Quit button pressed!"); // doar pentru test în editor
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}