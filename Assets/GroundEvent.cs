using UnityEngine;

using Unity.Cinemachine;

public class GroundEvent : MonoBehaviour
{
    public GameObject ground;       // pământul care se crapă
    public GameObject enemy;        // inamicul care apare
    public CinemachineCamera mainCamera;
    public Transform cameraFocusPoint; // locul spre care se va uita camera
    public float cameraMoveTime = 2f;
    public GameObject smoke;
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(EventSequence());
        }
    }

    private System.Collections.IEnumerator EventSequence()
    {
        // 1. Mutăm camera spre zona evenimentului
        mainCamera.Follow = cameraFocusPoint;
        yield return new WaitForSeconds(cameraMoveTime);

        // 2. „Crăpăm" pământul (flacără)
        if (ground != null)
        {
            ground.SetActive(true);
            yield return new WaitForSeconds(1f);
            ground.SetActive(false);
        }

        // 3. Apare fumul
        if (smoke != null)
        {
            smoke.SetActive(true);
            yield return new WaitForSeconds(0.5f); // așteptăm puțin pentru efectul de fum
        }

        // 4. Inamicul iese din fum
        if (enemy != null)
        {
            enemy.SetActive(true);
            yield return new WaitForSeconds(1f); // inamicul apare în timpul fumului
        }

        // 5. Fumul dispare
        if (smoke != null)
        {
            smoke.SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        // 6. Revenim cu camera la player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            mainCamera.Follow = player.transform;
    }
}
