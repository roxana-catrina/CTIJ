using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 100f; // viteza în grade/secundă
    private float currentRotation = 0f;

    void Update()
    {
        // creștem unghiul în funcție de timp și viteză
        currentRotation += rotationSpeed * Time.deltaTime;

        // limităm între 0° și 360° pentru o rotație completă
        if (currentRotation > 360f)
            currentRotation -= 360f;

        // aplicăm rotația doar pe axa X
        transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
    }
}
