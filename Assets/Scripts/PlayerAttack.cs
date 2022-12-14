using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : Attack
{
    [SerializeField] public float rollCooldown = 0.4f;
    [SerializeField] public float rollSpeed = 0.3f; //Decreases alongside rollCooldown when roll speed is upgraded
    [SerializeField] public float rollInv = 0.40f; //Percentage of total roll time that is invulnerable
    [SerializeField] float rollDist = 3f; //Constant
    [HideInInspector] public bool canRoll = true;
    [HideInInspector] public bool rollInvulnerable = false;
    public float currentStamina = 10;
    public float maxStamina = 10;
    [SerializeField] float rollCost = 5;
    [SerializeField] float staminaRegen = 1.5f; //Stamina gained per second
    [SerializeField] float staminaJailSentence = 3;
    [SerializeField] float staminaJailTime = 0;

    PlayerMovement playerMovement;

    protected override void OnAwake()
    {
        base.OnAwake();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(staminaJailTime <= 0)
        {
            staminaJailTime = 0;
            currentStamina += staminaRegen * Time.deltaTime;
            if(currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
        else
        {
            staminaJailTime -= Time.deltaTime;
        }
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
        if(context.performed && canRoll && staminaJailTime <= 0)
        {
            if(GetComponent<Rigidbody2D>().velocity != new Vector2(0, 0))
            {
                GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity().normalized * rollDist;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = -transform.right * rollDist;
            }
            
            currentStamina -= rollCost;
            if(currentStamina < 0)
            {
                staminaJailTime = staminaJailSentence;
                currentStamina = 0;
            }

            StartCoroutine(RollStop());
            //Play sound
            cooldownRoutine = StartCoroutine(RollCooldown());
        }
    }

    IEnumerator RollStop()
    {
        yield return new WaitForSeconds(rollSpeed);
        canAttack = true;
        GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity();
    }

    IEnumerator RollCooldown()
    {
        rollInvulnerable = true;
        canAttack = false;
        canRoll = false;
        playerMovement.canMove = false;
        yield return new WaitForSeconds(rollInv * rollCooldown);
        rollInvulnerable = false;
        yield return new WaitForSeconds(rollCooldown - (rollInv * rollCooldown));
        canRoll = true;
        playerMovement.canMove = true;
        cooldownRoutine = null;
    }

    public void RollCatch()
    {
        currentStamina += rollCost;
        //Play sound
    }
}
