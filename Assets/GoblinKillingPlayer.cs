using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoblinKillPlayer : MonoBehaviour
{
    [Header("References")]
    public GameObject disappearEffect;
    public GameObject closedGate;
    public GameObject openGate;

    [Header("Goblin Laugh Settings")]
    [SerializeField] private AudioClip goblinLaugh; // sunetul de râs
    [SerializeField] private float laughDuration = 1f; // durata râsului
    [SerializeField] private float laughVolume = 1f;

    [Header("Gate Sound Settings")]
    [SerializeField] private AudioClip gateOpenSound; // sunetul pentru poartă
    [SerializeField] private float gateSoundVolume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                if (!inventory.hasDiamond)
                {
                    // Playerul NU are diamantul → Game Over
                    PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
                    SceneManager.LoadScene("GameOver");
                }
                else
                {
                    // Playerul ARE diamantul → goblinul râde, apoi dispare, poarta se deschide
                    StartCoroutine(DestroyWithEffectAndLaugh());
                }
            }
        }
    }

    private IEnumerator DestroyWithEffectAndLaugh()
    {
        // 1️⃣ Goblinul râde
        if (goblinLaugh != null)
            AudioHelper.PlayClipAtPoint(goblinLaugh, transform.position, laughVolume);

        // 2️⃣ Așteaptă cât durează râsul
        yield return new WaitForSeconds(laughDuration);

        // 3️⃣ Efect de particule
        if (disappearEffect != null)
            Instantiate(disappearEffect, transform.position, Quaternion.identity);

        // 4️⃣ Pauză scurtă pentru efect vizual
        yield return new WaitForSeconds(0.5f);

        // 5️⃣ Deschide poarta
        if (closedGate != null && openGate != null)
        {
            closedGate.SetActive(false);
            openGate.SetActive(true);

            // 🔊 Redă sunetul porții
            if (gateOpenSound != null)
                AudioHelper.PlayClipAtPoint(gateOpenSound, transform.position, gateSoundVolume);
        }

        // 6️⃣ Distruge goblinul
        Destroy(gameObject);
    }
}
