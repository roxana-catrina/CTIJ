using UnityEngine;
using UnityEngine.InputSystem;

public class SpearThrow : MonoBehaviour
{
    public GameObject spearPrefab;
    public float spearSpeed = 10f;
    public float spearOffset = 0.5f;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ThrowSpear();
        }
    }

    void ThrowSpear()
    {
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy == null)
        {
            Debug.LogWarning("Nu s-a găsit niciun inamic cu tag-ul 'Enemy'!");
            return;
        }

        Debug.Log("Inamic găsit: " + nearestEnemy.name);

        // Calculează direcția către inamic
        Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;
        Debug.Log("Direcția către inamic: " + direction);

        // Calculează poziția de spawn
        Vector2 spawnPosition = (Vector2)transform.position + direction * spearOffset;

        // Creează sulița
        GameObject spear = Instantiate(spearPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Sulița a fost creată la poziția: " + spawnPosition);

        // Rotește sulița în direcția inamicului
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        spear.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Verifică și adaugă velocity
        Rigidbody2D rb = spear.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("PROBLEMA: Spear-ul nu are Rigidbody2D! Adaugă Rigidbody2D la prefab-ul Spear!");
            rb = spear.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }

        rb.linearVelocity = direction * spearSpeed;
        Debug.Log("Velocity setat la: " + rb.linearVelocity);

        // Asigură-te că nu este kinematic
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            Debug.LogWarning("Nu există obiecte cu tag-ul 'Enemy' în scenă!");
            return null;
        }

        Debug.Log("Număr de inamici găsiți: " + enemies.Length);

        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }
}