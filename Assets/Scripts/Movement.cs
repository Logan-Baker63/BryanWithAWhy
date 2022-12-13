using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool canMove = true;
    public float movementSpeed = 5;
    protected Vector2 moveVelocity;


    protected bool isControlLocked = false;
    public bool IsControlLocked() { return isControlLocked; }
    public void SetControlLocked(bool ToF) { isControlLocked = ToF; }


    public Vector2 GetMoveVelocity() { return moveVelocity; }
    private void FixedUpdate()
    {
        if (!isControlLocked)
        {
            if (canMove)
            {
                Move(moveVelocity);
            }
        }
        else
        {
            Move(Vector2.zero);
        }
    }

    protected virtual void Move(Vector2 velocity)
    {

    }
}
