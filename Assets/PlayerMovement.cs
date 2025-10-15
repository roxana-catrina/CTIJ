using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    public GameObject poweredChild;  // copilul powerplayer
    public bool canAttack = false;
    private bool facingRight = false; // la început sabia e pe stânga

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // luam copilul powered automat dacă nu a fost asignat
        if (poweredChild == null)
        {
            poweredChild = transform.Find("PoweredPlayer")?.gameObject;
        }
        if (poweredChild == null)
        {
            Debug.LogError("Nu s-a găsit poweredChild! Verifică numele copilului în ierarhie.");
        }

        if (poweredChild != null)
        {
            poweredChild.transform.localPosition = Vector3.zero;
            poweredChild.SetActive(false);  // la început nu e activ
        }
    }

    void FixedUpdate()
    {
        Vector2 move = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) move.y = 1;
        else if (Keyboard.current.sKey.isPressed) move.y = -1;

        if (Keyboard.current.aKey.isPressed) move.x = -1;
        else if (Keyboard.current.dKey.isPressed) move.x = 1;

        move = move.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + move);

        // Flip player după direcție
        if (move.x > 0 && facingRight)
            Flip(false);
        else if (move.x < 0 && !facingRight)
            Flip(true);
    }

    void Flip(bool faceRight)
    {
        facingRight = faceRight;
        Vector3 scale = transform.localScale;
        scale.x = faceRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void Update()
    {
        if (canAttack && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Potion"))
        {
            canAttack = true;
            if (poweredChild != null)
            {
                poweredChild.transform.localPosition = Vector3.zero;
                poweredChild.SetActive(true);
            }

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;

            Destroy(collision.gameObject);
        }
    }

    private void Attack()
    {
        float attackRange = 1.0f;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
                Destroy(enemy.gameObject);
        }
    }

    public void ClearAppearance()
    {
        if (poweredChild != null)
            poweredChild.SetActive(false);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;
    }

    public void ResetAppearance()
    {
        if (poweredChild != null)
            poweredChild.SetActive(false);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = true;
        canAttack = false;
    }
}
