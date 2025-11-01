using UnityEngine;

public class EnemyFreeze : MonoBehaviour
{
    private bool isFrozen = false;
    private float freezeTimer = 0f;
    private float freezeDuration = 0f;

    // Referințe la componentele care trebuie dezactivate
    private MonoBehaviour[] movementScripts;
    private Rigidbody2D rb;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        if (isFrozen)
        {
            freezeTimer += Time.deltaTime;
            
            if (freezeTimer >= freezeDuration)
            {
                Unfreeze();
            }
        }
    }

    public void Freeze(float duration)
    {
        // Inițializează componentele dacă nu sunt setate (când script-ul e adăugat dinamic)
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
        }

        if (isFrozen)
        {
            // Dacă este deja înghețat, resetează timerul
            freezeTimer = 0f;
            freezeDuration = duration;
            Debug.Log("Enemy deja înghețat, resetare timer la " + duration + " secunde");
            return;
        }

        isFrozen = true;
        freezeTimer = 0f;
        freezeDuration = duration;

        Debug.Log("=== ÎNCEPE ÎNGHEȚAREA ENEMY-ULUI ===");

        // Oprește Rigidbody-ul MAI ÎNTÂI
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Static;
            Debug.Log("Rigidbody2D oprit și setat ca Static");
        }
        else
        {
            Debug.LogError("Nu s-a găsit Rigidbody2D pe enemy!");
        }

        // Oprește toate script-urile de mișcare
        movementScripts = GetComponents<MonoBehaviour>();
        int disabledCount = 0;
        foreach (MonoBehaviour script in movementScripts)
        {
            // Nu dezactiva acest script
            if (script != this && script != null && script.enabled)
            {
                if (script is Enemy1_movement || script is GoblinPatrol)
                {
                    script.enabled = false;
                    disabledCount++;
                    Debug.Log("Script dezactivat: " + script.GetType().Name);
                }
            }
        }
        Debug.Log("Total scripturi de mișcare dezactivate: " + disabledCount);

        // Schimbă culoarea pentru efect vizual (albastru/cyan pentru înghețat)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.5f, 0.8f, 1f, 1f); // Albastru deschis
            Debug.Log("Culoare schimbată în albastru");
        }
        else
        {
            Debug.LogWarning("Nu s-a găsit SpriteRenderer pentru schimbarea culorii");
        }

        Debug.Log("Enemy înghețat pentru " + duration + " secunde");
    }

    void Unfreeze()
    {
        isFrozen = false;
        freezeTimer = 0f;

        // Reactivează script-urile de mișcare
        if (movementScripts != null)
        {
            foreach (MonoBehaviour script in movementScripts)
            {
                if (script != this && (script is Enemy1_movement ))
                {
                    script.enabled = true;
                }
            }
        }

        // Reactivează Rigidbody-ul
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        // Restaurează culoarea originală
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        Debug.Log("Enemy dezghețat");
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }
}
