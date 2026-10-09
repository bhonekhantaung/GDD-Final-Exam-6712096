using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCameraX : MonoBehaviour
{
    private float speed = 200f;
    public GameObject player;

    void Update()
    {
        float horizontalInput = 0f;

        // Unity 6 project is using the new Input System.
        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                horizontalInput += 1f;
            }

            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                horizontalInput -= 1f;
            }
        }

        transform.Rotate(
            Vector3.up,
            horizontalInput * speed * Time.deltaTime
        );

        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }
}