using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OpenDoorTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "FinalScene"; // numele scenei finale
    [SerializeField] private float delayBeforeLoad = 0.5f; // jumătate de secundă

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadNextSceneAfterDelay());
        }
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(nextSceneName);
    }
}
