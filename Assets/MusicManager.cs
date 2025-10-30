using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

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
    }
}
