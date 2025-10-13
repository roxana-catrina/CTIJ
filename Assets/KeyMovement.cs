using UnityEngine;

public class KeyMovement : MonoBehaviour
{
    public float rotationSpeed = 50f; // degrees per second

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
