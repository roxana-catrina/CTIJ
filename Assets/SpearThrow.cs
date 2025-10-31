using UnityEngine;
using UnityEngine.InputSystem;

public class SpearThrow : MonoBehaviour
{
    public GameObject spearPrefab;
    public float spearSpeed = 10f;
    public float spearOffset = 0.5f;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
        {
            ThrowSpear();
        }
    }

    void ThrowSpear()
    {
        if (CoinManager.instance.item1 != 0)
        {
            CoinManager.instance.item1--;

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

            Debug.Log("Direcția către mouse: " + direction);

            // Calculează poziția de spawn
            Vector2 spawnPosition = (Vector2)transform.position + direction * spearOffset;

            // Creează sulița
            GameObject spear = Instantiate(spearPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Sulița a fost creată la poziția: " + spawnPosition);

            // Rotește sulița în direcția mouse-ului
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            spear.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Verifică și adaugă velocity
            Rigidbody2D rb = spear.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                Debug.LogWarning("Spear-ul nu are Rigidbody2D! Se adaugă automat.");
                rb = spear.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0;
            }

            rb.linearVelocity = direction * spearSpeed;
            Debug.Log("Velocity setat la: " + rb.linearVelocity);

            // Asigură-te că nu este kinematic
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        else
        {
            Debug.Log("Nu ai suficiente iteme pentru a arunca sulița!");
        }
    }
}