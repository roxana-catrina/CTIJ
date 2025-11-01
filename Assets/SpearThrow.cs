using UnityEngine;
using UnityEngine.InputSystem;

public class SpearThrow : MonoBehaviour
{
    public GameObject spearPrefab;
    public GameObject frozenBallPrefab;
    public float projectileSpeed = 10f;
    public float projectileOffset = 0.5f;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
        {
            ThrowProjectile();
        }
    }

    void ThrowProjectile()
    {
        // Verifică ce tip de proiectil să arunce bazat pe item1 și item2
        if (CoinManager.instance.item1 == 2)
        {
            // Doar spear
            LaunchProjectile(spearPrefab, "spear");
            CoinManager.instance.item1--;
        }
        else if (CoinManager.instance.item1 == 1 && CoinManager.instance.item2 == 1)
        {
            // Prima dată frozen ball, apoi spear
            LaunchProjectile(frozenBallPrefab, "frozen ball");
            CoinManager.instance.item2--;
        }
        else if (CoinManager.instance.item1 == 1 && CoinManager.instance.item2 == 0)
        {
            // Doar spear (când item2 este 0)
            LaunchProjectile(spearPrefab, "spear");
            CoinManager.instance.item1--;
        }
        else if (CoinManager.instance.item2 == 2)
        {
            // Doar frozen ball
            LaunchProjectile(frozenBallPrefab, "frozen ball");
            CoinManager.instance.item2--;
        }
        else if (CoinManager.instance.item2 == 1 && CoinManager.instance.item1 == 0)
        {
            // Doar frozen ball (când item1 este 0)
            LaunchProjectile(frozenBallPrefab, "frozen ball");
            CoinManager.instance.item2--;
        }
        else
        {
            Debug.Log("Nu ai suficiente iteme pentru a arunca proiectile!");
        }
    }

    void LaunchProjectile(GameObject prefab, string projectileName)
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab-ul pentru {projectileName} nu este setat!");
            return;
        }

        // Obține poziția mouse-ului în world space
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        Vector2 mouseWorld = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        // Calculează direcția către mouse
        Vector2 direction = (mouseWorld - (Vector2)transform.position).normalized;
        
        if (direction == Vector2.zero)
        {
            Debug.LogWarning("Direcția este zero, folosesc direcția implicită (dreapta)");
            direction = Vector2.right;
        }

        Debug.Log($"Direcția către mouse pentru {projectileName}: " + direction);

        // Calculează poziția de spawn
        Vector2 spawnPosition = (Vector2)transform.position + direction * projectileOffset;

        // Creează proiectilul
        GameObject projectile = Instantiate(prefab, spawnPosition, Quaternion.identity);
        Debug.Log($"{projectileName} a fost creat la poziția: " + spawnPosition);

        // Rotește proiectilul în direcția mouse-ului
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Verifică și adaugă velocity
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogWarning($"{projectileName} nu are Rigidbody2D! Se adaugă automat.");
            rb = projectile.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }

        rb.linearVelocity = direction * projectileSpeed;
        Debug.Log($"Velocity setat pentru {projectileName} la: " + rb.linearVelocity);

        // Asigură-te că nu este kinematic
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Ignoră coliziunea cu player-ul pentru FrozenBall
        if (projectileName == "frozen ball")
        {
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
            Collider2D playerCollider = GetComponent<Collider2D>();
            
            if (projectileCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(projectileCollider, playerCollider, true);
                Debug.Log("Coliziunea între FrozenBall și Player a fost ignorată");
            }
        }
    }
}