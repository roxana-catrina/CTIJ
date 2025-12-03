using UnityEngine;

public class SurpriseMusic : MonoBehaviour
{
    public AudioClip surpriseClip;
    private AudioSource audioSource;
    private bool surprisePlaying = false;

    void Start()
    {
        // Crează propriul AudioSource pentru melodia surpriză
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = surpriseClip;
        audioSource.playOnAwake = false;
    }

    public void ToggleSurprise()
    {
        if (!surprisePlaying)
        {
            // Oprește muzica de fundal
            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.PauseMusic();
            }

            // Pornește melodia surpriză
            audioSource.Play();
            surprisePlaying = true;
        }
        else
        {
            // Oprește melodia surpriză
            audioSource.Stop();
            surprisePlaying = false;

            // Repornește muzica de fundal
            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.ResumeMusic();
            }
        }
    }
}
