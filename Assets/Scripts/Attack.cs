using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform spawnPos;
    [SerializeField] protected float shootCooldown = 0.2f;
    [SerializeField] protected float meleeCooldown = 0.2f;

    [SerializeField] protected float bulletDamage = 10;
    [SerializeField] protected float meleeDamage = 15;

    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public float meleeDelay = 0f;
    
    public Coroutine cooldownRoutine;

    protected AudioManager manager;

    protected List<GameObject> enemiesInMeleeRange = new List<GameObject>();

    public List<GameObject> GetEnemiesInRange() { return enemiesInMeleeRange; }
    public void SetEnemiesInRange(List<GameObject> _enemiesInMeleeRange) { enemiesInMeleeRange = _enemiesInMeleeRange; }

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
                bulletInstance.GetComponent<Projectile>().projectileSpeed = 200;
                bulletInstance.transform.localScale *= 2;
            }

            bulletInstance.GetComponent<Projectile>().SetDamage(bulletDamage);
            manager.PlaySound(0);
            cooldownRoutine = StartCoroutine(ShootCooldown());
        }
    }

    protected virtual void Melee()
    {
        if (meleeDelay > 0)
        {
            meleeDelay -= Time.deltaTime;
        }
        if (canAttack && meleeDelay <= 0)
        {
            try
            {
                foreach (GameObject enemy in enemiesInMeleeRange)
                {
                    if (enemy != null)
                    {
                        enemy.GetComponent<Health>().TakeDamage(meleeDamage, this);
                    }
                    else
                    {
                        enemiesInMeleeRange.Remove(enemy);
                    }
                }
            }
            catch
            {

            }
            if (GetComponent<EnemyState>())
            {
                GetComponent<EnemyState>().SetState(0);
            }
            GetComponent<Animator>().SetTrigger("Punch");
            //Play sound
            cooldownRoutine = StartCoroutine(MeleeCooldown());
        }
    }

    public void OnEnterMeleeTrigger(GameObject enemy)
    {
        enemiesInMeleeRange.Add(enemy);
    }

    public void OnExitMeleeTrigger(GameObject enemy)
    {
        enemiesInMeleeRange.Remove(enemy);
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
