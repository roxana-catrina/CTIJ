using UnityEngine;

using Unity.Cinemachine;

public class GroundEvent : MonoBehaviour
{
    public GameObject ground;       // pământul care se crapă
    public GameObject enemy;        // inamicul care apare
    public CinemachineCamera mainCamera;
    public Transform cameraFocusPoint; // locul spre care se va uita camera
    public float cameraMoveTime = 2f;

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
            Animator anim = ground.GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("Crack");
            else
                ground.SetActive(false); // dacă nu ai animație
        }

        yield return new WaitForSeconds(1f);

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
