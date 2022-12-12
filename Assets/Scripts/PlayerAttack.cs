using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : Attack
{
    [SerializeField] public float rollCooldown = 0.4f;
    [SerializeField] public float rollSpeed = 0.3f; //Decreases alongside rollCooldown when roll speed is upgraded
    [SerializeField] public float rollInv = 0.15f;
    [SerializeField] float rollDist = 3f; //Constant
    [HideInInspector] public bool canRoll = true;

    PlayerMovement playerMovement;

    protected override void OnAwake()
    {
        base.OnAwake();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Shoot();
        }
    }

    public void OnMelee(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Melee();
        }
    }

    public void Roll(InputAction.CallbackContext context)
    {
        if(context.performed && canRoll)
        {
            if(GetComponent<Rigidbody2D>().velocity != new Vector2(0, 0))
            {
                GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity().normalized * rollDist;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = -transform.right * rollDist;
            }

            StartCoroutine(RollStop());
            //Play sound
            cooldownRoutine = StartCoroutine(RollCooldown());
        }
    }

    IEnumerator RollStop()
    {
        yield return new WaitForSeconds(0.2f);
        canAttack = true;
        GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity();
    }

    IEnumerator RollCooldown()
    {
        canAttack = false;
        canRoll = false;
        playerMovement.canMove = false;
        yield return new WaitForSeconds(rollCooldown);
        canRoll = true;
        playerMovement.canMove = true;
        cooldownRoutine = null;
    }
}
