using UnityEngine;

public class SpearBehavior : MonoBehaviour
{
    public float lifetime = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, lifetime);

        // Configurează Rigidbody2D pentru detectare mai bună
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            Debug.Log("✅ Spear Rigidbody2D configurat: " + rb.bodyType);
        }
        else
        {
            Debug.LogError("❌ Spear-ul nu are Rigidbody2D!");
        }

        // Verifică dacă are collider
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("❌ Spear-ul nu are Collider2D!");
        }
        else
        {
            Debug.Log("✅ Spear Collider găsit: " + col.GetType() + ", Is Trigger: " + col.isTrigger + ", Layer: " + LayerMask.LayerToName(gameObject.layer));
        }
    }

    void Update()
    {
        // Debug vizual - trasează o linie roșie în direcția mișcării
        if (rb != null)
        {
            Debug.DrawRay(transform.position, rb.linearVelocity.normalized * 2f, Color.red);
        }
    }

    // Pentru Trigger Colliders (Is Trigger = bifat)
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("🎯 TRIGGER detectat cu: " + collision.gameObject.name + ", Tag: " + collision.tag + ", Layer: " + LayerMask.LayerToName(collision.gameObject.layer));

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("✅ LOVIT INAMIC! Distrug: " + collision.gameObject.name);

            // Distruge inamicul
            Destroy(collision.gameObject);

            // Distruge sulița
            Destroy(gameObject);
        }
    }

    // Pentru Non-Trigger Colliders (Is Trigger = nebifat)
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("💥 COLIZIUNE detectată cu: " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag + ", Layer: " + LayerMask.LayerToName(collision.gameObject.layer));

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("✅ LOVIT INAMIC! Distrug: " + collision.gameObject.name);

            // Distruge inamicul
            Destroy(collision.gameObject);

            // Distruge sulița
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("⚠️ TRIGGER STAY cu: " + collision.gameObject.name);
    }
}