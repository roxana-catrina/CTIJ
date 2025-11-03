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
    public int maps = 0;
    public int potions = 0;
    public int coinsForBuy = 0;
    public TextMeshProUGUI textCoin; // Referință la UI Text pentru afișarea monedelor
    public TextMeshProUGUI textHealth; // Reference to health display
    public TextMeshProUGUI textMap;
    public TextMeshProUGUI textPotion;
    public GameObject healthUI; // Reference to health UI GameObject
    public bool item1Bought = false;
    public bool item2Bought = false;
    public string currentLevel; // Salvează nivelul curent

    public int item1 = 0;
    public int item2 = 0;
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
        coinsForBuy++;
    }

    public void AddMap()
    {
        maps++;
    }

    public void AddPotion()
    {
        potions++;
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
        
        // Salvează nivelul curent când se încarcă Level 1 sau Level 2
        if (scene.name == "Level 1" || scene.name == "Level 2")
        {
            currentLevel = scene.name; // Salvează nivelul curent
            
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

                // Activează UI-ul de viață
                if (healthUI != null)
                {
                    healthUI.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("PlayerMovement not found in scene 'Level 1'!");
            }
            coinsCollected = 0;
            health = 3;
            maps = 0;
            potions = 0;
        }
        else
        {
            // Dezactivează UI-ul de viață în alte scene
            if (healthUI != null)
            {
                healthUI.SetActive(false);
            }
        }
    }

    public System.Collections.IEnumerator ReassignUI()
    {
        // Așteaptă un frame pentru a te asigura că UI-ul s-a încărcat complet
        yield return null;

        GameObject coinObj = GameObject.Find("TextCoin");
        GameObject healthObj = GameObject.Find("TextHealth");
        GameObject mapObj = GameObject.Find("TextMap");
        GameObject potionObj = GameObject.Find("TextPotion");

        // Găsește și referința pentru healthUI dacă nu este setată
        if (healthUI == null)
        {
            healthUI = GameObject.Find("TextHealth"); // Înlocuiește cu numele corect al GameObject-ului tău
        }

        if (coinObj != null)
            textCoin = coinObj.GetComponent<TMPro.TextMeshProUGUI>();
        else
            textCoin = null;

        if (healthObj != null)
            textHealth = healthObj.GetComponent<TMPro.TextMeshProUGUI>();
        else
            textHealth = null;

        if (mapObj != null)
            textMap = mapObj.GetComponent<TMPro.TextMeshProUGUI>();
        else
            textMap = null;

        if (potionObj != null)
            textPotion = potionObj.GetComponent<TMPro.TextMeshProUGUI>();
        else
            textPotion = null;

        // Actualizează UI-ul imediat dacă a fost găsit
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (textCoin != null)
            textCoin.text =  coinsCollected.ToString();
        if (textHealth != null)
            textHealth.text =  health.ToString();

        if (textMap != null)
        {
            textMap.text = "Maps: " + maps+ "/1";
        }
        if (textPotion != null)
        {
            textPotion.text = potions.ToString();
        }
    }

    private void Update()
    {
        if (textCoin != null)
        {
            textCoin.text =  coinsCollected.ToString() ;
        }

        if (textHealth != null)
        {
            textHealth.text =  health.ToString();
        }


        if (textMap != null)
        {
            textMap.text = "Maps: " + maps.ToString() + "/1";
        }
        if (textPotion != null)
        {
            textPotion.text = potions.ToString() ;
        }
    }

    public bool BuyItem1(int cost)
    {
        if (coinsForBuy >= cost)
        {
            coinsForBuy -= cost;
            coinsCollected = coinsCollected - cost;
            item1++;

            return true;
        }
        return false;
    }
     public bool BuyItem2(int cost)
    {
        if (coinsForBuy >= cost)
        {
            coinsForBuy -= cost;
            coinsCollected = coinsCollected - cost;
            item2++;

            return true;
        }
        return false;
    }
}