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

        // 2. „Crăpăm” pământul
        if (ground != null)
        {
            ground.SetActive(true); // dacă nu ai animație
            yield return new WaitForSeconds(1f);
            ground.SetActive(false); // dacă nu ai animație
        }

        if (smoke != null)
        {
            smoke.SetActive(true); // dacă nu ai animație
            yield return new WaitForSeconds(1f);
            smoke.SetActive(false); // dacă nu ai animație
        }


        // 3. Activăm inamicul
        if (enemy != null)
            enemy.SetActive(true);

        yield return new WaitForSeconds(2f);

        // 4. Revenim cu camera la player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            mainCamera.Follow = player.transform;
    }
}
