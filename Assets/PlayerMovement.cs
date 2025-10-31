﻿﻿using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private float originalSpeed;
    private Rigidbody2D rb;
    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;
    public GameObject poweredChild;  // copilul powerplayer
    public bool canAttack = false;
    private bool facingRight = false; // la început sabia e pe stânga
    [SerializeField] public Transform startPoint;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public GameObject spear;

    public AudioClip mudSound;  // sunetul de mers prin noroi
    public AudioClip potionSound;
    public AudioClip mapSound;
    public AudioClip iceSound;
    public AudioClip swordSound; 
    private AudioSource audioSource;
    void Start()
    {
        originalSpeed = speed;
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
      

        // limitele ecranului
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        if (startPoint == null)
        {
            GameObject startPointObj = GameObject.Find("StartPoint");
            if (startPointObj != null)
            {
                startPoint = startPointObj.transform;
                Debug.Log("startPoint found in scene: " + startPoint.name);
            }
            else
            {
                Debug.LogError("No GameObject named 'StartPoint' found in the scene!");
            }
        }
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        Vector2 move = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) move.y = 1;
        else if (Keyboard.current.sKey.isPressed) move.y = -1;

        if (Keyboard.current.aKey.isPressed) move.x = -1;
        else if (Keyboard.current.dKey.isPressed) move.x = 1;

        move = move.normalized * speed * Time.fixedDeltaTime;

        // Mută Rigidbody fără să “teleportezi” obiectul
        rb.MovePosition(rb.position + move);
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

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Potion"))
        {
            canAttack = true;
            CoinManager.instance.AddPotion(); // adaugă o monedă când iei poțiunea
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

            if (potionSound != null && audioSource != null)
                audioSource.PlayOneShot(potionSound);

            Destroy(collision.gameObject);
        }


        if (collision.CompareTag("Map"))
        {
            CoinManager.instance.AddMap(); // adaugă o hartă când iei harta

             if (mapSound != null && audioSource != null)
                audioSource.PlayOneShot(mapSound);

            Destroy(collision.gameObject);
        }


        if (collision.CompareTag("SlowZone"))
        {
            speed = originalSpeed / 3f; // sau orice factor vrei (ex: /2f pentru jumătate)
            Debug.Log("Entered SlowZone, speed reduced to: " + speed);
            if (mudSound != null && !audioSource.isPlaying)
             {
                 audioSource.clip = mudSound;
                 audioSource.loop = true;
                 audioSource.Play();
             }
        }

        if (collision.CompareTag("FastZone"))
        {
            speed = originalSpeed * 3f; // sau *2f dacă vrei dublă viteză
            Debug.Log("Entered FastZone, speed increased to: " + speed);
             if (iceSound != null && !audioSource.isPlaying)
             {
                 audioSource.clip = iceSound;
                 audioSource.loop = true;
                 audioSource.Play();
             }    
        }
    }




   private void Attack()
    {
        float attackRange = 1.0f;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

         if (swordSound != null)
             AudioHelper.PlayClipAtPoint(swordSound, transform.position);

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SlowZone"))
        {
            speed = originalSpeed; // revine la viteza normală
            Debug.Log("Exited SlowZone, speed restored to: " + speed);
            audioSource.Stop();
        }

        if (collision.CompareTag("FastZone"))
        {
            speed = originalSpeed;

            Debug.Log("Exited FastZone, speed restored to: " + speed);
            audioSource.Stop();
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
        // Dezactivează poweredChild
        if (poweredChild != null)
            poweredChild.SetActive(false);

        // Reactivează playerul normal
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = true;

        // Reset alte variabile dacă e nevoie
        canAttack = false;
        if (startPoint == null)
        {
            GameObject startPointObj = GameObject.Find("StartPoint");
            if (startPointObj != null)
            {
                startPoint = startPointObj.transform;
                Debug.Log("startPoint reassigned in ResetAppearance: " + startPoint.name);
            }
            else
            {
                Debug.LogError("No GameObject named 'StartPoint' found in the scene during ResetAppearance!");
                return; // Iese din metodă dacă startPoint nu este găsit
            }
        }
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
    }

}