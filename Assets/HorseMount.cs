using UnityEngine;
using System.Collections;

public class HorseMount : MonoBehaviour
{
    [SerializeField] private Sprite playerOnHorseSprite; // sprite-ul nou
    [SerializeField] private AudioClip mountSound; // opțional: sunet la urcare
    [SerializeField] private float soundVolume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ChangeSpriteAfterDelay(other));
        }
    }

    private IEnumerator ChangeSpriteAfterDelay(Collider2D playerCollider)
    {
        // 🔊 Redă sunetul imediat, dacă există
        if (mountSound != null)
            AudioHelper.PlayClipAtPoint(mountSound, transform.position, soundVolume);

        // 🕐 Așteaptă 0.5 secunde
        yield return new WaitForSeconds(0.5f);

        SpriteRenderer playerRenderer = playerCollider.GetComponent<SpriteRenderer>();
        if (playerRenderer != null && playerOnHorseSprite != null)
        {
            playerRenderer.sprite = playerOnHorseSprite;

            // 🔍 Ajustează mărimea pentru a se potrivi cu calul
            playerRenderer.transform.localScale = new Vector3(2f, 2f, 1f);
        }


        // 🐴 Opțional: calul dispare
        Destroy(gameObject);
    }
}
