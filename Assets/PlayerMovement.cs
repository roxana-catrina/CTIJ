﻿﻿using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

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
    public GameObject frozenBall;

    public AudioClip mudSound;  // sunetul de mers prin noroi
    public AudioClip potionSound;
    public AudioClip mapSound;
    public AudioClip iceSound;
    public AudioClip swordSound; 
    
    private AudioSource audioSource;
    private Sprite originalSprite; // Salvează sprite-ul original
    private Vector3 originalScale; // Salvează scala originală

    void Awake()
    {
        // Salvează sprite-ul și scala originală ÎNAINTE de orice modificare
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalSprite = sr.sprite;
            originalScale = transform.localScale;
            Debug.Log("Original sprite and scale saved in Awake");
        }

        // Găsește startPoint IMEDIAT și mută playerul acolo ÎNAINTE de orice altceva
        if (startPoint == null)
        {
            GameObject startPointObj = GameObject.Find("StartPoint");
            if (startPointObj != null)
            {
                startPoint = startPointObj.transform;
                Debug.Log("startPoint found in Awake: " + startPoint.name);
            }
            else
            {
                Debug.LogError("No GameObject named 'StartPoint' found in the scene!");
            }
        }

        // Mută playerul la startPoint ÎNAINTE ca camera să înceapă să-l urmărească
        if (startPoint != null)
        {
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
            Debug.Log("Player positioned at startPoint in Awake: " + startPoint.position);
            
            // Forțează camera să se poziționeze instant pe player
            CinemachineCamera virtualCamera = FindFirstObjectByType<CinemachineCamera>();
            if (virtualCamera != null)
            {
                virtualCamera.Follow = transform;
                // Forțează o actualizare instantanee a camerei
                virtualCamera.OnTargetObjectWarped(transform, transform.position - virtualCamera.transform.position);
                Debug.Log("Camera forced to player position in Awake");
            }
        }
    }

    void Start()
    {
        Debug.Log("PlayerMovement Start() called - Speed before: " + speed);
        originalSpeed = speed;
        Debug.Log("PlayerMovement Start() - OriginalSpeed set to: " + originalSpeed);
        
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
        
        audioSource = gameObject.AddComponent<AudioSource>();
        
        Debug.Log("PlayerMovement Start() completed - Speed: " + speed + ", OriginalSpeed: " + originalSpeed);
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
        CoinManager.instance.AddPotion();
        
        if (poweredChild != null)
        {
            // Activează poweredChild
            poweredChild.transform.localPosition = Vector3.zero;
            poweredChild.SetActive(true);
            
            // Pornește coroutine pentru a dezactiva după 10 secunde
            StartCoroutine(DeactivatePoweredChildAfterDelay(10f));
        }

        // Ascunde modelul normal
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

private System.Collections.IEnumerator DeactivatePoweredChildAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    
    // După 10 secunde, dezactivează poweredChild și reactivează sprite-ul normal
    if (poweredChild != null)
    {
        poweredChild.SetActive(false);
    }
    
    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    if (sr != null)
    {
        sr.enabled = true;
    }
    
    canAttack = false; // Dezactivează și atacul
    Debug.Log("PoweredChild deactivated after " + delay + " seconds");
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
        Debug.Log("ResetAppearance called!");
        
        // Dezactivează poweredChild (forma powered)
        if (poweredChild != null)
        {
            poweredChild.SetActive(false);
            Debug.Log("PoweredChild deactivated");
        }

        // Reactivează playerul normal
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = true;
            
            // Restaurează sprite-ul original (dacă a fost schimbat de HorseMount)
            if (originalSprite != null)
            {
                sr.sprite = originalSprite;
                Debug.Log("Original sprite restored");
            }
            
            Debug.Log("SpriteRenderer enabled, player should be visible now");
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on player!");
        }

        // Restaurează scala originală (dacă a fost modificată de HorseMount)
        if (originalScale != Vector3.zero)
        {
            transform.localScale = originalScale;
            Debug.Log("Original scale restored: " + originalScale);
        }

        // Reset alte variabile
        canAttack = false; // Resetează capacitatea de atac
        // NU resetăm speed aici - Start() o face automat
        
        // Oprește orice sunet care ar putea rula
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        
        // Regăsește startPoint în scenă dacă nu este setat
        if (startPoint == null)
        {
            GameObject startPointObj = GameObject.Find("StartPoint");
            if (startPointObj != null)
            {
                startPoint = startPointObj.transform;
                Debug.Log("StartPoint regăsit în ResetAppearance: " + startPoint.name);
            }
        }
        
        // Resetează poziția playerului la startPoint
        if (startPoint != null)
        {
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
            Debug.Log("Player repositioned to startPoint: " + startPoint.position);
        }
        else
        {
            Debug.LogWarning("StartPoint not found! Player stays at current position.");
        }
        
        // Reconectează camera Cinemachine la player
        ReconnectCamera();
        
        Debug.Log("Speed: " + speed + ", OriginalSpeed: " + originalSpeed + ", CanAttack: " + canAttack);
    }

    private void ReconnectCamera()
    {
        CinemachineCamera virtualCamera = FindFirstObjectByType<CinemachineCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = transform;
            Debug.Log("Camera reconnected to player: " + gameObject.name);
        }
        else
        {
            Debug.LogWarning("CinemachineCamera not found in scene!");
        }
    }
}