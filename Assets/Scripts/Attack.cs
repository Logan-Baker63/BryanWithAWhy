using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform spawnPos;
    [SerializeField] protected float shootCooldown = 0.2f;
    [SerializeField] protected float meleeCooldown = 0.2f;
    [SerializeField] public bool canAction = true;
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
        if (canAction)
        {
            GameObject bulletInstance = Instantiate(bullet, spawnPos.position, transform.rotation);
            bulletInstance.GetComponent<Projectile>().SetDir(-transform.right);
            manager.PlaySound(0);
            cooldownRoutine = StartCoroutine(ShootCooldown());
        }
    }

    protected virtual void Melee()
    {
        //Do sword swing
        //Play sound
        cooldownRoutine = StartCoroutine(MeleeCooldown());
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
