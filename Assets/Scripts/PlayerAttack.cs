using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform spawnPos;
    [SerializeField] float shootCooldown = 0.2f;
    [SerializeField] public bool canShoot = true;
    public Coroutine cooldownRoutine;
    
    AudioManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<AudioManager>();
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed && canShoot)
        {
            GameObject bulletInstance = Instantiate(bullet, spawnPos.position, transform.rotation);
            bulletInstance.GetComponent<Projectile>().SetDir(-transform.right);
            manager.PlaySound(0);
            cooldownRoutine = StartCoroutine(ShootCooldown());
        }
    }

    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
        cooldownRoutine = null;
    }
}
