using UnityEngine;

public class SpearBehavior : MonoBehaviour
{
    public float lifetime = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, lifetime);

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        else
        {
            Debug.LogError("Spear-ul nu are Rigidbody2D!");
        }

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger detectat cu: " + collision.gameObject.name + ", Tag: " + collision.tag);

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("INAMIC LOViT");
            Destroy(collision.gameObject); // Distruge inamicul
            Destroy(gameObject); // Distruge sulița
        }
        else if (collision.CompareTag("Wall") || collision.CompareTag("Obstacle"))
        {
            Debug.Log("PERETE LOVit");
            Destroy(gameObject); // Distruge doar sulița
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Coliziune detectată cu: " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(" INAMIC LOViT");
            Destroy(collision.gameObject); // Distruge inamicul
            Destroy(gameObject); // Distruge sulița
        }
        else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log(" PERETE LOVit!");
            Destroy(gameObject); // Distruge doar sulița
        }
    }
}