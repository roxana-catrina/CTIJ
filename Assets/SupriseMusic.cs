using UnityEngine;

public class SurpriseMusic : MonoBehaviour
{
    public AudioClip surpriseClip;

    private AudioSource globalMusic;
    private AudioClip originalClip;
    private bool surprisePlaying = false;

    void Start()
    {
        // Găsește AudioSource-ul global care a pornit în Main Menu
        globalMusic = FindObjectOfType<AudioSource>();

        if (globalMusic == null)
        {
            Debug.LogError("No global AudioSource found!");
            return;
        }

        // Salvează muzica originală (cea care cânta în Main Menu)
        originalClip = globalMusic.clip;
    }

    public void ToggleSurprise()
    {
        if (globalMusic == null) return;

        if (!surprisePlaying)
        {
            // ❗ PORNEȘTE muzica surpriză
            globalMusic.Stop();
            globalMusic.clip = surpriseClip;
            globalMusic.Play();

            surprisePlaying = true;
        }
        else
        {
            // ❗ Oprește muzica surpriză și revine la clipul original
            globalMusic.Stop();
            globalMusic.clip = originalClip;
            // dacă vrei să re-pornească muzica originală, decomentează linia:
            // globalMusic.Play();

            surprisePlaying = false;
        }
    }
}
