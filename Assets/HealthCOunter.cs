using UnityEngine;

public class HealthCounter : MonoBehaviour
{
    public int currentHealth = 3;

    void Start()
    {
        currentHealth = 3;
    }

    public void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        GetComponent<PlayerMovement>().enabled = false;
    }
}
