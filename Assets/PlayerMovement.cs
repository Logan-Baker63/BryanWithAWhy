using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5;

    Vector2 moveVelocity;

    // Update is called once per frame
    void Update()
    {
        Move(moveVelocity);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveVelocity = context.ReadValue<Vector2>() * movementSpeed;
        }
    }

    private void Move(Vector2 velocity)
    {
        GetComponent<Rigidbody>().velocity = velocity;
    }
}
