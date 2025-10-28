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
        }
        else
        {
            Debug.LogError("Spear-ul nu are Rigidbody2D!");
        }

        // Verifică dacă are collider
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("Spear-ul nu are Collider2D!");
        }
        else
        {
            Debug.Log("Spear Collider: " + col.GetType() + ", Is Trigger: " + col.isTrigger);
        }
    }

    // Pentru Trigger Colliders (Is Trigger = bifat)
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger detectat cu: " + collision.gameObject.name + ", Tag: " + collision.tag);

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("✅ LOVIT! Distrug inamicul și sulița!");

            // Distruge inamicul
            Destroy(collision.gameObject);

            // Distruge sulița
            Destroy(gameObject);
        }
    }

    // Pentru Non-Trigger Colliders (Is Trigger = nebifat)
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Coliziune detectată cu: " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("✅ LOVIT! Distrug inamicul și sulița!");

            // Distruge inamicul
            Destroy(collision.gameObject);

            // Distruge sulița
            Destroy(gameObject);
        }
    }
}