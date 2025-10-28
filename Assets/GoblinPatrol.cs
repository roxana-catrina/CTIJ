using UnityEngine;

public class GoblinPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Transform targetPoint;

    private void Start()
    {
        targetPoint = pointB; // începe să meargă spre punctul B
    }

    private void Update()
    {
        // mișcare constantă spre țintă
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            speed * Time.deltaTime
        );

        // dacă a ajuns aproape de țintă, schimbă direcția
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
            FlipSprite();
        }
    }

    private void FlipSprite()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // întoarce vizual goblinul
        transform.localScale = localScale;
    }
}
