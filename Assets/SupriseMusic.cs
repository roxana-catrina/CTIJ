using UnityEngine;

public class SurpriseMusic : MonoBehaviour
{
    public AudioClip surpriseClip;
    private AudioSource globalMusic;

    void Start()
    {
        // Găsește AudioSource-ul global (cel care a supraviețuit scenelor)
        globalMusic = FindObjectOfType<AudioSource>();

        if (globalMusic == null)
        {
            Debug.LogError("No global AudioSource found in scene!");
        }
    }

    public void PlaySurprise()
    {
        if (globalMusic == null)
            return;

        globalMusic.Stop();              // oprește muzica de fundal
        globalMusic.clip = surpriseClip; // pune noul clip
        globalMusic.Play();              // pornește muzica surpriză
    }
}
