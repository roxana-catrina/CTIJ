using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;

    void Start()
    {
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        
        // Get and setup Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void FixedUpdate()
    {
        Vector2 move = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) move.y = 1;
        else if (Keyboard.current.sKey.isPressed) move.y = -1;

        if (Keyboard.current.aKey.isPressed) move.x = -1;
        else if (Keyboard.current.dKey.isPressed) move.x = 1;

        move = move.normalized * speed;
        rb.linearVelocity = move;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth);
        pos.y = Mathf.Clamp(pos.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);
        transform.position = pos;
    }
}