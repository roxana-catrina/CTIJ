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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (other.CompareTag("Player") || other.CompareTag("PoweredPlayer"))
        {
            if (CoinManager.instance != null)
            {
                CoinManager.instance.TakeDamage();
                Debug.Log("Player hit, health: " + CoinManager.instance.health);
            }
            else
            {
                UnityEngine.Debug.LogWarning("CoinManager.instance este null!");
            }
        }
    }

}
