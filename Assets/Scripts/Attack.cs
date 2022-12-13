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
    [HideInInspector] public bool doMelee = false;
    [HideInInspector] public bool doShoot = false;

    [SerializeField] int maxBulletAmount = 10;
    [SerializeField] int bulletAmount = 1;
    public void SetBulletAmount(int _bulletAmount) 
    { 
        if (_bulletAmount <= maxBulletAmount)
        {
            bulletAmount = _bulletAmount;
        }
    }
    public int GetBulletAmount() { return bulletAmount;}

    [Range(0, 360)]
    [SerializeField] private float spreadAngle = 20;

    [SerializeField] public float bulletSpeed = 200;

    public Coroutine cooldownRoutine;

    protected AudioManager manager;

    protected List<GameObject> enemiesInMeleeRange = new List<GameObject>();

    public List<GameObject> GetEnemiesInRange() { return enemiesInMeleeRange; }
    public void SetEnemiesInRange(List<GameObject> _enemiesInMeleeRange) { enemiesInMeleeRange = _enemiesInMeleeRange; }

    protected void Awake()
    {
        OnAwake();
    }

    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void OnAwake()
    {
        manager = FindObjectOfType<AudioManager>();
    }

    protected virtual void Shoot()
    {
        if (canAttack)
        {
            float angleStep = spreadAngle / bulletAmount;
            float aimingAngle = transform.rotation.eulerAngles.z;
            float centeringOffset = (spreadAngle / 2) - (angleStep / 2); //offsets every projectile so the spread is

            for (int i = 0; i < bulletAmount; i++)
            {
                float currentBulletAngle = angleStep * i;

                Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, aimingAngle + currentBulletAngle - centeringOffset));

                GameObject bulletInstance = Instantiate(bullet, spawnPos.position, rotation/*transform.rotation*/);
                bulletInstance.GetComponent<Projectile>().SetDir(-bulletInstance.transform.right);

                bulletInstance.GetComponent<Projectile>().owner = gameObject;
                if (gameObject.tag == "Player")
                {
                    bulletInstance.GetComponent<Projectile>().playerBullet = true;
                    gameObject.GetComponent<PlayerAttack>().canRoll = true;
                    gameObject.GetComponent<PlayerAttack>().rollStopCoroutine = null;
                    //gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity();
                    transform.parent.GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity();
                }
                else
                {
                    bulletInstance.GetComponent<Projectile>().playerBullet = false;
                    bulletInstance.layer = 6;
                    bulletInstance.transform.localScale *= 2;
                }

                bulletInstance.GetComponent<Projectile>().projectileSpeed = bulletSpeed;
                //Debug.Log((bulletDamage * (bulletAmountDamageIncreaseMulti * (bulletAmount - 1))) / bulletAmount);
                bulletInstance.GetComponent<Projectile>().SetDamage(bulletDamage / bulletAmount);
            }
            
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
