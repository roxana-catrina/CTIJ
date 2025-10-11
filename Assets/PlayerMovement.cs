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
}