using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : Attack
{
    [SerializeField] float rollCooldown = 0.4f;
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
            GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity() * 5;
            StartCoroutine(RollStop());
            //Play sound
            cooldownRoutine = StartCoroutine(RollCooldown());
        }
    }

    IEnumerator RollStop()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity();
    }

    IEnumerator RollCooldown()
    {
        canRoll = false;
        playerMovement.canMove = false;
        yield return new WaitForSeconds(rollCooldown);
        canRoll = true;
        playerMovement.canMove = true;
        cooldownRoutine = null;
    }
}
