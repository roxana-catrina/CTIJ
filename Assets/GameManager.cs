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
    public int item1restart = 0;
    public int item2restart = 0;

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
            Debug.LogError($"[CoinManager] FATAL: Destroying duplicate instance on object: '{gameObject.name}'. IF THIS IS THE PLAYER, REMOVE THE COINMANAGER SCRIPT FROM IT!");
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

    [System.Obsolete]
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

    [System.Obsolete]
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [System.Obsolete]
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [System.Obsolete]
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"OnSceneLoaded called for scene: {scene.name}");
        
        // Salvează nivelul curent când se încarcă Level 1 sau Level 2
        if (scene.name == "Level 1" || scene.name == "Level 2")
        {
            currentLevel = scene.name; // Salvează nivelul curent
            
            // Resetează valorile ÎNAINTE de a reseta playerul
            coinsCollected = 0;
            health = 3;
            maps = 0;
            potions = 0; // Resetează potions la 0
            // NU resetăm item1 și item2 aici - rămân păstrate între niveluri
            
            Debug.Log("Potions reset to: " + potions);
            
            // VERIFICĂ IMEDIAT dacă playerul există în scenă
            PlayerMovement[] immediateCheck = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
            Debug.Log($"IMMEDIATE CHECK: Found {immediateCheck.Length} PlayerMovement objects in scene {scene.name}");
            foreach (PlayerMovement pm in immediateCheck)
            {
                Debug.Log($"  - PlayerMovement on GameObject: {pm.gameObject.name}, Active: {pm.gameObject.activeInHierarchy}");
            }
            
            // Folosim doar coroutine-ul pentru a aștepta ca Start() să se execute
            StartCoroutine(ResetPlayerAfterSceneLoad());

            // Activează UI-ul de viață
            if (healthUI != null)
            {
                healthUI.SetActive(true);
            }
        }
        else
        {
            // Dezactivează UI-ul de viață în alte scene
            if (healthUI != null)
            {
                healthUI.SetActive(false);
            }
        }
        
        // Actualizează UI-ul DUPĂ ce ai resetat valorile
        StartCoroutine(ReassignUI());
    }

    private System.Collections.IEnumerator ResetPlayerAfterSceneLoad()
    {
        Debug.Log("ResetPlayerAfterSceneLoad started, waiting for player...");
        
        // Așteaptă până găsești playerul (cu timeout)
        PlayerMovement player = null;
        float timeout = 2f;
        float elapsed = 0f;
        
        while (player == null && elapsed < timeout)
        {
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
            
            // Caută TOATE obiectele cu PlayerMovement
            PlayerMovement[] allPlayers = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
            Debug.Log($"Found {allPlayers.Length} PlayerMovement objects after {elapsed}s");
            
            // Găsește playerul care ARE SpriteRenderer (playerul adevărat)
            foreach (PlayerMovement pm in allPlayers)
            {
                Debug.Log($"Checking PlayerMovement on: {pm.gameObject.name}");
                SpriteRenderer sr = pm.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    player = pm;
                    Debug.Log("Found REAL player with SpriteRenderer: " + pm.gameObject.name);
                    break;
                }
            }
            
            // Dacă nu găsim unul cu SpriteRenderer, ia primul disponibil
            if (player == null && allPlayers.Length > 0)
            {
                player = allPlayers[0];
                Debug.LogWarning("No player with SpriteRenderer found, using first PlayerMovement: " + player.gameObject.name);
            }
        }
        
        if (player != null)
        {
            Debug.Log("Player found after scene load: " + player.gameObject.name);
            
            // Asigură-te că resetezi canAttack
            player.canAttack = false;
            
            // Apelează ResetAppearance DUPĂ ce Start() s-a executat
            player.ResetAppearance();
            
            // Setează camera să urmărească player-ul
            CinemachineCamera virtualCamera = FindAnyObjectByType<CinemachineCamera>();
            if (virtualCamera != null)
            {
                virtualCamera.Follow = player.transform;
                Debug.Log("Camera set to follow player: " + player.gameObject.name);
            }
            else
            {
                Debug.LogError("CinemachineVirtualCamera not found in scene!");
            }
            
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Debug.Log("Player SpriteRenderer enabled: " + sr.enabled);
            }
        }
        else
        {
            Debug.LogError($"PlayerMovement not found after {timeout}s timeout!");
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
            healthUI = GameObject.Find("TextHealth");
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

        // Actualizează UI-ul imediat cu valorile resetate
        UpdateUI();
        
        Debug.Log("UI reassigned - Potions displayed: " + (textPotion != null ? textPotion.text : "null"));
    }

    private void UpdateUI()
    {
        if (textCoin != null)
            textCoin.text = coinsCollected.ToString();
        if (textHealth != null)
            textHealth.text = health.ToString();
        if (textMap != null)
            textMap.text = "Maps: " + maps + "/1";
        if (textPotion != null)
        {
            textPotion.text = potions.ToString();
            Debug.Log("Potion UI updated to: " + potions);
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
        // Verifică dacă ai suficiente monede ȘI dacă nu ai cumpărat deja de 2 ori
        if (coinsForBuy >= cost && item1 < 2)
        {
            coinsForBuy -= cost;
            coinsCollected = coinsCollected - cost;
            item1++;
            item1restart = item1;
            
            if (item1 >= 2)
            {
                item1Bought = true; // Marchează ca fiind cumpărat complet după 2 achiziții
            }
            
            Debug.Log("Item 1 cumpărat! Total: " + item1 + "/2, CoinsForBuy: " + coinsForBuy);
            return true;
        }
        
        if (item1 >= 2)
        {
            Debug.Log("Ai cumpărat deja Item 1 de 2 ori (maxim)!");
        }
        else
        {
            Debug.Log("Nu ai suficiente monede pentru Item 1! Ai: " + coinsForBuy + ", Necesari: " + cost);
        }
        
        return false;
    }
    
    public bool BuyItem2(int cost)
    {
        // Verifică dacă ai suficiente monede ȘI dacă nu ai cumpărat deja de 2 ori
        if (coinsForBuy >= cost && item2 < 2)
        {
            coinsForBuy -= cost;
            coinsCollected = coinsCollected - cost;
            item2++;
            item2restart = item2;
            
            if (item2 >= 2)
            {
                item2Bought = true; // Marchează ca fiind cumpărat complet după 2 achiziții
            }
            
            Debug.Log("Item 2 cumpărat! Total: " + item2 + "/2, CoinsForBuy: " + coinsForBuy);
            return true;
        }
        
        if (item2 >= 2)
        {
            Debug.Log("Ai cumpărat deja Item 2 de 2 ori (maxim)!");
        }
        else
        {
            Debug.Log("Nu ai suficiente monede pentru Item 2! Ai: " + coinsForBuy + ", Necesari: " + cost);
        }
        
        return false;
    }
}