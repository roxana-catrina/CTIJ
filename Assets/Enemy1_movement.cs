using System.Diagnostics;
using UnityEngine;
using UnityEngine;
public class Enemy1_movement : MonoBehaviour
{
    public float speed = 3f;
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        // Caută obiectul Player normal
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 direction = ((Vector2)player.position - rb.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PoweredPlayer"))
        {
            if (CoinManager.instance != null)
            {
                CoinManager.instance.TakeDamage();
                UnityEngine.Debug.Log("Viata scade! Health curent: " + CoinManager.instance.health);
            }
            else
            {
                UnityEngine.Debug.LogWarning("CoinManager.instance este null!");
            }
        }
    }

}
