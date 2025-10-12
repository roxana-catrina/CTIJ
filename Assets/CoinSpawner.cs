using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // Prefab-ul monedei
    public float spawnInterval = 2f; // Intervalul de generare (secunde)
    public Vector2 spawnPosition; // Poziția unde apar monedele (stânga-sus)

    void Start()
    {
        // Începe generarea monedelor
        InvokeRepeating("SpawnCoin", 0f, spawnInterval);
    }

    void SpawnCoin()
    {
        // Generează o monedă la poziția specificată
        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    }
}