using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoblinKillPlayer : MonoBehaviour
{
    public GameObject disappearEffect;
    public GameObject closedGate;
    public GameObject openGate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                if (!inventory.hasDiamond)
                {
                    // dacă playerul NU are diamantul → Game Over
                    PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
                    SceneManager.LoadScene("GameOver");
                }
                else
                {
                    // dacă playerul ARE diamantul → goblinul dispare + poarta se deschide
                    StartCoroutine(DestroyWithEffect());
                }
            }
        }
    }

    private IEnumerator DestroyWithEffect()
    {
        // Efect de particule
        if (disappearEffect != null)
            Instantiate(disappearEffect, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.5f); // mică pauză pentru efect

        // Deschide poarta
        if (closedGate != null && openGate != null)
        {
            closedGate.SetActive(false); // ascunde poarta închisă
            openGate.SetActive(true);    // arată poarta deschisă
        }

        // Distruge goblinul
        Destroy(gameObject);
    }
}
