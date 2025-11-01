using UnityEngine;

public class Enemy1_movement : MonoBehaviour
{
    public float speed = 3f;
    public float detectionRange = 1.5f;
    public LayerMask wallLayer;
    public float damageCooldown = 1f; // Cooldown între damage-uri
    
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 currentDirection;
    private float stuckTimer = 0f;
    private Vector2 lastPosition;
    private float lastDamageTime = 0f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        lastPosition = rb.position;
        currentDirection = Vector2.right;
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        Vector2 directionToPlayer = ((Vector2)player.position - rb.position).normalized;
        
        // Verifică dacă este drum liber spre player
        RaycastHit2D hitToPlayer = Physics2D.Raycast(rb.position, directionToPlayer, detectionRange, wallLayer);
        
        if (hitToPlayer.collider == null)
        {
            // Drum liber - mergi direct spre player
            currentDirection = directionToPlayer;
        }
        else
        {
            // Perete în față - caută o direcție liberă
            currentDirection = FindFreeDirection(directionToPlayer);
        }
        
        // Verifică dacă este blocat
        if (Vector2.Distance(rb.position, lastPosition) < 0.01f)
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer > 0.5f)
            {
                // Schimbă direcția aleatoriu când e blocat
                currentDirection = Random.insideUnitCircle.normalized;
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
        
        lastPosition = rb.position;
        
        // Mișcă enemy-ul
        rb.linearVelocity = currentDirection * speed;
    }

    Vector2 FindFreeDirection(Vector2 preferredDirection)
    {
        // Testează multiple direcții
        float[] angles = { 0f, 45f, -45f, 90f, -90f, 135f, -135f };
        
        foreach (float angle in angles)
        {
            Vector2 testDirection = Quaternion.Euler(0, 0, angle) * preferredDirection;
            RaycastHit2D hit = Physics2D.Raycast(rb.position, testDirection, detectionRange, wallLayer);
            
            if (hit.collider == null)
            {
                return testDirection.normalized;
            }
        }
        
        // Dacă nicio direcție nu e liberă, întoarce-te
        return -currentDirection;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Verifică cooldown-ul pentru a evita damage-ul repetat
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                if (CoinManager.instance != null)
                {
                    CoinManager.instance.TakeDamage();
                    lastDamageTime = Time.time;
                    Debug.Log("Enemy hit player, health: " + CoinManager.instance.health);
                }
                else
                {
                    Debug.LogError("CoinManager.instance is null!");
                }
            }
        }
    }
}
