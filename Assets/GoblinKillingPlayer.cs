using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoblinKillPlayer : MonoBehaviour
{
    // referință către prefab-ul de particule
    public GameObject disappearEffect;

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
                    // dacă playerul ARE diamantul → goblinul dispare cu particule
                    StartCoroutine(DestroyWithEffect());
                }
            }
        }
    }

    private IEnumerator DestroyWithEffect()
    {
        // creează efectul la poziția goblinului
        if (disappearEffect != null)
        {
            Instantiate(disappearEffect, transform.position, Quaternion.identity);
        }

        // aștepți puțin ca particulele să se vadă
        yield return new WaitForSeconds(0.5f);

        // distrugi goblinul
        Destroy(gameObject);
    }
}
