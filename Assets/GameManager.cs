using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // Singleton
    public int coinsCollected = 0; // Numărul de monede colectate
    public int health = 3; // Starting health
    public TextMeshProUGUI textCoin; // Referință la UI Text pentru afișarea monedelor
    public TextMeshProUGUI textHealth; // Reference to health display

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

    public void TakeDamage()
    {
        health--;
        Debug.Log("Health remaining: " + health);

        if (health <= 0)
        {
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            if (player != null)
            {
                player.ClearAppearance();
            }

            SceneManager.LoadScene("GameOver");
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(ReassignUI());
        if (scene.name == "Level 1")
        {
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            if (player != null)
            {
                if (player.startPoint == null)
                {
                    Debug.LogError("startPoint is not assigned in PlayerMovement component!");
                }
                player.ResetAppearance();

                // Setează camera să urmărească player-ul
                CinemachineCamera virtualCamera = FindObjectOfType<CinemachineCamera>();
                if (virtualCamera != null)
                {
                    virtualCamera.Follow = player.transform;
                    Debug.Log("Camera set to follow player: " + player.gameObject.name);
                }
                else
                {
                    Debug.LogError("CinemachineVirtualCamera not found in scene!");
                }
            }
            else
            {
                Debug.LogError("PlayerMovement not found in scene 'Level 1'!");
            }
            coinsCollected = 0; // Numărul de monede colectate
            health = 3; // Starting health
        }
    }

    private System.Collections.IEnumerator ReassignUI()
    {
        // Așteaptă un frame pentru a te asigura că UI-ul s-a încărcat complet
        yield return null;

        GameObject coinObj = GameObject.Find("TextCoin");
        GameObject healthObj = GameObject.Find("TextHealth");

        if (coinObj != null)
            textCoin = coinObj.GetComponent<TMPro.TextMeshProUGUI>();
        else
            textCoin = null;

        if (healthObj != null)
            textHealth = healthObj.GetComponent<TMPro.TextMeshProUGUI>();
        else
            textHealth = null;

        // Actualizează UI-ul imediat dacă a fost găsit
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (textCoin != null)
            textCoin.text = "Coins: " + coinsCollected;
        if (textHealth != null)
            textHealth.text = "Health: " + health;
    }

    private void Update()
    {
        if (textCoin != null)
        {
            textCoin.text = "Coins: " + coinsCollected.ToString();
        }

        if (textHealth != null)
        {
            textHealth.text = "Health: " + health.ToString();
        }
    }
}