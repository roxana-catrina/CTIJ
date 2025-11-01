using UnityEngine;

public class FrozenBallBehavior : MonoBehaviour
{
    public float lifetime = 5f;
    public float freezeDuration = 10f; // Durata înghețării în secunde
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
            Debug.LogError("FrozenBall nu are Rigidbody2D!");
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("FrozenBall nu are Collider2D!");
        }
        else
        {
            Debug.Log("FrozenBall Collider: " + col.GetType() + ", Is Trigger: " + col.isTrigger);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("FrozenBall Trigger detectat cu: " + collision.gameObject.name + ", Tag: " + collision.tag);

        // Ignoră coliziunea cu Player-ul
        if (collision.CompareTag("Player"))
        {
            Debug.Log("FrozenBall a trecut prin Player (ignorat)");
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("INAMIC ÎNGHEȚAT!");
            FreezeEnemy(collision.gameObject);
            Destroy(gameObject); // Distruge frozen ball-ul
        }
        else if (collision.CompareTag("Wall") || collision.CompareTag("Obstacle"))
        {
            Debug.Log("PERETE LOVit de FrozenBall");
            Destroy(gameObject); // Distruge doar frozen ball-ul
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("FrozenBall Coliziune detectată cu: " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag);

        // Ignoră coliziunea cu Player-ul
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("FrozenBall a lovit Player (ignorat)");
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("INAMIC ÎNGHEȚAT!");
            FreezeEnemy(collision.gameObject);
            Destroy(gameObject); // Distruge frozen ball-ul
        }
        else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("PERETE LOVit de FrozenBall!");
            Destroy(gameObject); // Distruge doar frozen ball-ul
        }
    }

    void FreezeEnemy(GameObject enemy)
    {
        // Verifică sau adaugă componenta EnemyFreeze
        EnemyFreeze freezeComponent = enemy.GetComponent<EnemyFreeze>();
        if (freezeComponent == null)
        {
            freezeComponent = enemy.AddComponent<EnemyFreeze>();
        }

        // Înghețarea inamicului
        freezeComponent.Freeze(freezeDuration);
    }
}
