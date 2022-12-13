using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform spawnPos;
    [SerializeField] protected float shootCooldown = 0.2f;
    [SerializeField] protected float meleeCooldown = 0.2f;

    [HideInInspector] public bool canAttack = true;
    
    public Coroutine cooldownRoutine;

    protected AudioManager manager;

    protected void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        manager = FindObjectOfType<AudioManager>();
    }

    protected virtual void Shoot()
    {
        if (canAttack)
        {
            GameObject bulletInstance = Instantiate(bullet, spawnPos.position, transform.rotation);
            bulletInstance.GetComponent<Projectile>().SetDir(-transform.right);

            bulletInstance.GetComponent<Projectile>().owner = gameObject;
            if (gameObject.tag == "Player")
            {
                bulletInstance.GetComponent<Projectile>().playerBullet = true;
                gameObject.GetComponent<PlayerAttack>().canRoll = true;
                gameObject.GetComponent <PlayerAttack>().rollStopCoroutine = null;
                //gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity();
                transform.parent.GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity();
            }
            else
            {
                bulletInstance.GetComponent<Projectile>().playerBullet = false;
                bulletInstance.layer = 6;
                bulletInstance.GetComponent<Projectile>().projectileSpeed = 180;
                bulletInstance.transform.localScale *= 2;
            }
            manager.PlaySound(0);
            cooldownRoutine = StartCoroutine(ShootCooldown());
        }
    }

    protected virtual void Melee()
    {
        if (canAttack)
        {
            //Do punch
            GetComponent<Animator>().SetTrigger("Punch");
            //Play sound
            cooldownRoutine = StartCoroutine(MeleeCooldown());
        }
    }

    IEnumerator ShootCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(shootCooldown);
        canAttack = true;
        cooldownRoutine = null;
    }

    IEnumerator MeleeCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(meleeCooldown);
        canAttack = true;
        cooldownRoutine = null;
    }
}
