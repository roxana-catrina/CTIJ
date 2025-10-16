using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // playerul
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        // dacă playerul a fost recreat (ex: după restart), îl căutăm din nou
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
                Debug.Log("🎥 Camera și-a găsit din nou playerul!");
            }
            else
            {
                return; // nu face nimic până nu-l găsește
            }
        }

        // mișcare lină a camerei către poziția playerului
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
