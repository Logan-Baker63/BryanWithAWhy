using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform spawnPos;
    [SerializeField] float shootCooldown = 0.2f;
    [SerializeField] float meleeCooldown = 0.2f;
    [SerializeField] float rollCooldown = 0.4f;
    [SerializeField] public bool canAction = true;
    public Coroutine cooldownRoutine;
    
    AudioManager manager;
    PlayerMovement playerMovement;

    private void Awake()
    {
        manager = FindObjectOfType<AudioManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed && canAction)
        {
            GameObject bulletInstance = Instantiate(bullet, spawnPos.position, transform.rotation);
            bulletInstance.GetComponent<Projectile>().SetDir(-transform.right);
            manager.PlaySound(0);
            cooldownRoutine = StartCoroutine(ShootCooldown());
        }
    }

    public void Melee(InputAction.CallbackContext context)
    {
        if(context.performed && canAction)
        {
            //Do sword swing
            //Play sound
            cooldownRoutine = StartCoroutine(MeleeCooldown());
        }
    }

    public void Roll(InputAction.CallbackContext context)
    {
        if(context.performed && canAction)
        {
            //Roll
            //Play sound
            cooldownRoutine = StartCoroutine(RollCooldown());
        }
    }

    IEnumerator ShootCooldown()
    {
        canAction = false;
        yield return new WaitForSeconds(shootCooldown);
        canAction = true;
        cooldownRoutine = null;
    }

    IEnumerator MeleeCooldown()
    {
        canAction = false;
        yield return new WaitForSeconds(meleeCooldown);
        canAction = true;
        cooldownRoutine = null;
    }

    IEnumerator RollCooldown()
    {
        canAction = false;
        playerMovement.canMove = false;
        yield return new WaitForSeconds(rollCooldown);
        canAction = true;
        playerMovement.canMove = true;
        cooldownRoutine = null;
    }
}
