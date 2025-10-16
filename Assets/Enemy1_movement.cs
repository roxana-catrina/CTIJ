using UnityEngine;

public class Enemy1_movement : MonoBehaviour
{
    public float speed = 3f;
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        // Găsește playerul
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        rb = GetComponent<Rigidbody2D>();

        // Asigură-te că Enemy are Rigidbody2D setat corect
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        // Calculează direcția spre player
        Vector2 direction = ((Vector2)player.position - rb.position).normalized;

        // Mută enemy-ul spre player
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    // Folosim coliziune fizică reală (nu trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy triggered with: " + other.name);

        if (other.CompareTag("Player") || other.CompareTag("PoweredPlayer"))
        {
            if (CoinManager.instance != null)
            {
                CoinManager.instance.TakeDamage();
            }
        }
    }

}
