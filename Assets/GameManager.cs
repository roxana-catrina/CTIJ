using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton
    public int coinsCollected = 0; // Numărul de monede colectate

    void Awake()
    {
        // Asigură-te că există o singură instanță
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin()
    {
        coinsCollected++;
        Debug.Log("Monede colectate: " + coinsCollected);
    }
}