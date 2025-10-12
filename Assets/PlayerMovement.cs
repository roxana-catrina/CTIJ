using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // viteza de mișcare

    void Update()
    {
        Vector2 move = Vector2.zero;

        // sus-jos
        if (Keyboard.current.wKey.isPressed) move.y = 1;
        else if (Keyboard.current.sKey.isPressed) move.y = -1;

        // stânga-dreapta
        if (Keyboard.current.aKey.isPressed) move.x = -1;
        else if (Keyboard.current.dKey.isPressed) move.x = 1;

        // normalizează pentru diagonală și deplasare uniformă
        move = move.normalized * speed * Time.deltaTime;

        // mută playerul
        transform.Translate(move);
    }

    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;

    void Start()
    {
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
       /* SpriteRenderer sr = GetComponent<SpriteRenderer>();
        playerWidth = sr.bounds.extents.x;
        playerHeight = sr.bounds.extents.y;*/
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        // Limite pe X și Y
        pos.x = Mathf.Clamp(pos.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth);
        pos.y = Mathf.Clamp(pos.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);

        transform.position = pos;
    }
}