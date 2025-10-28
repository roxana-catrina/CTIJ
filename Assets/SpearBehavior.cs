using UnityEngine;

public class SpearBehavior : MonoBehaviour
{
    public float lifetime = 5f; // Timp după care sulița se distruge automat

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Aici poți adăuga logică pentru damage
            Debug.Log("Sulița a lovit inamicul!");

            // Distruge sulița
            Destroy(gameObject);

            // Opțional: distruge și inamicul sau reduce-i viața
            // Destroy(collision.gameObject);
        }
    }
}