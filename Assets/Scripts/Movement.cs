using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool canMove = true;
    [SerializeField] protected float movementSpeed = 5;
    protected Vector2 moveVelocity;
    protected float slowness = 1;

    public void SetSlowness(float _slownesMulti)
    {
        slowness = _slownesMulti;
    }
    public Vector2 GetMoveVelocity() { return moveVelocity; }
    private void FixedUpdate()
    {
        if (canMove)
        {
            Move(moveVelocity / slowness);
        }
    }

    protected virtual void Move(Vector2 velocity)
    {

    }
}
