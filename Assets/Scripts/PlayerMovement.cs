using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5;

    Vector2 moveVelocity;
    public bool canMove = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(moveVelocity);
    }

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

    private void Move(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
