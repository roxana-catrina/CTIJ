using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    public static MusicManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        // verifică dacă există deja o instanță activă
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // distruge dublurile
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // păstrează-l între scene
        
        // Obține AudioSource-ul (ar trebui să fie deja atașat în Inspector)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("MusicManager: Nu s-a găsit AudioSource pe acest GameObject!");
        }
    }

    public void PauseMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }
}
