using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;
    public GameObject poweredChild;  // copilul powerplayer
    public bool canAttack = false;

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
        Debug.Log("poweredChild: " + poweredChild);

        if (poweredChild != null)
        {
            poweredChild.transform.localPosition = Vector3.zero; // îl aduce exact peste Player

            poweredChild.SetActive(false);  // la început nu e activ
        }
       /* // dimensiuni pentru clamp
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            playerWidth = sr.bounds.extents.x;
            playerHeight = sr.bounds.extents.y;
        }*/

        // limitele ecranului
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }

    void FixedUpdate()
    {
        Vector2 move = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) move.y = 1;
        else if (Keyboard.current.sKey.isPressed) move.y = -1;

        if (Keyboard.current.aKey.isPressed) move.x = -1;
        else if (Keyboard.current.dKey.isPressed) move.x = 1;

        move = move.normalized * speed;
        rb.linearVelocity = move;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth);
        pos.y = Mathf.Clamp(pos.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);
        transform.position = pos;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Potion"))
        {
            canAttack = true;

            if (poweredChild != null)
            {
                // poziționăm poweredChild peste player
                poweredChild.transform.localPosition = Vector3.zero;
                poweredChild.SetActive(true);
            }

            // ascunde modelul normal (dacă playerul principal are SpriteRenderer)
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.enabled = false;

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
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    void Update()
    {
        if (canAttack && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Attack();
        }
    }

    public void ResetAppearance()
    {
        // Dezactivează poweredChild
        if (poweredChild != null)
            poweredChild.SetActive(false);

        // Reactivează playerul normal
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = true;

        // Reset alte variabile dacă e nevoie
        canAttack = false;
    }

}
