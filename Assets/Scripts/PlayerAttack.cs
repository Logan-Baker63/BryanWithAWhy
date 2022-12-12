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
    [SerializeField] public bool canAction = true;
    public Coroutine cooldownRoutine;
    
    AudioManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<AudioManager>();
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
}
