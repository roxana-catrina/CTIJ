using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // Singleton
    public int coinsCollected = 0; // Numărul de monede colectate
    public TextMeshProUGUI textCoin; // Referință la UI Text pentru afișarea monedelor
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
    private void Update()
    {
        textCoin.text = coinsCollected.ToString();
    }
}
