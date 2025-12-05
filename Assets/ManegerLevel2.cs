using UnityEngine;

public class ManegerLevel2 : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startPoint;
    public GameObject spear; // Corectat din GameManager în GameObject

    void Awake()
    {
        // Caută StartPoint dacă nu e asignat
        if (startPoint == null)
        {
            GameObject sp = GameObject.Find("StartPoint");
            if (sp != null) startPoint = sp.transform;
        }

        // Verifică dacă playerul există deja
        if (FindAnyObjectByType<PlayerMovement>() == null)
        {
            if (playerPrefab != null)
            {
                // Instanțiază playerul la StartPoint dacă există, altfel la (0,0,0)
                Vector3 spawnPos = startPoint != null ? startPoint.position : Vector3.zero;
                Instantiate(playerPrefab, spawnPos, Quaternion.identity);
                Debug.Log("[ManegerLevel2] Player instantiated via script.");
            }
            else
            {
                Debug.LogError("[ManegerLevel2] CRITICAL: Player Prefab is NOT assigned in Inspector! Please assign it.");
            }
        }

        if (spear != null)
        {
            spear.SetActive(false);
        }
    }
}
