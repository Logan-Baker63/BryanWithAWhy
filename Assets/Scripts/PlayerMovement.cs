using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Movement
{
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveVelocity = context.ReadValue<Vector2>() * movementSpeed * Time.fixedDeltaTime;
        }

        if (context.canceled)
        {
            moveVelocity = Vector2.zero;
        }
    }

    protected override void Move(Vector2 velocity)
    {
        //GetComponent<Rigidbody2D>().velocity = velocity;
        transform.parent.GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
