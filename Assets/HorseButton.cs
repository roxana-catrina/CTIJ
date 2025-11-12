using UnityEngine;

public class HorseButton : MonoBehaviour
{
    public AudioClip neighSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = neighSound;
    }

    public void PlayNeigh()
    {
        audioSource.Play();
    }
}
