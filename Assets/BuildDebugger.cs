using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script de debugging pentru a verifica de ce playerul nu apare în build.
/// Atașează acest script pe un GameObject gol în scenă (ex: "BuildDebugger")
/// </summary>
public class BuildDebugger : MonoBehaviour
{
    void Start()
    {
        Debug.Log("========== BUILD DEBUGGER START ==========");
        Debug.Log("Scene name: " + SceneManager.GetActiveScene().name);
        Debug.Log("Build GUID: " + Application.buildGUID);
        Debug.Log("Is Editor: " + Application.isEditor);
        
        // Caută toate obiectele cu tag "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("Objects with 'Player' tag found: " + players.Length);
        
        foreach (GameObject player in players)
        {
            Debug.Log("  - Player object: " + player.name + " at position: " + player.transform.position);
            
            // Verifică componentele
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            
            Debug.Log("    PlayerMovement: " + (pm != null ? "✅ Found" : "❌ Missing"));
            Debug.Log("    SpriteRenderer: " + (sr != null ? "✅ Found (enabled: " + sr.enabled + ", sprite: " + (sr.sprite != null ? sr.sprite.name : "null") + ")" : "❌ Missing"));
            Debug.Log("    Rigidbody2D: " + (rb != null ? "✅ Found" : "❌ Missing"));
            Debug.Log("    GameObject active: " + player.activeInHierarchy);
            Debug.Log("    Layer: " + LayerMask.LayerToName(player.layer));
        }
        
        // Caută PlayerMovement direct
        PlayerMovement[] allPlayerMovements = FindObjectsOfType<PlayerMovement>();
        Debug.Log("PlayerMovement components found: " + allPlayerMovements.Length);
        
        foreach (PlayerMovement pm in allPlayerMovements)
        {
            Debug.Log("  - PlayerMovement on: " + pm.gameObject.name);
        }
        
        // Verifică camera
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            Debug.Log("Main Camera found: " + mainCam.name + " at position: " + mainCam.transform.position);
        }
        else
        {
            Debug.LogError("❌ Main Camera NOT FOUND!");
        }
        
        Debug.Log("========== BUILD DEBUGGER END ==========");
    }
}
