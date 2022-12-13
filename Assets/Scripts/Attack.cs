using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform spawnPos;
    [SerializeField] public float shootCooldown = 0.2f;
    [SerializeField] protected float meleeCooldown = 0.2f;

    public float bulletScaleMultiplier = 1;
    public float bulletDamage = 10;
    public float meleeDamage = 15;

    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public float meleeDelay = 0f;
    [HideInInspector] public bool doMelee = false;
    [HideInInspector] public bool doShoot = false;

    [SerializeField] int maxBulletAmount = 10;
    public int bulletAmount = 1;
    public void SetBulletAmount(int _bulletAmount) 
    { 
        if (_bulletAmount <= maxBulletAmount)
        {
            bulletAmount = _bulletAmount;
        }
        else
        {
            bulletAmount = maxBulletAmount;
        }
    }
    public int GetBulletAmount() { return bulletAmount;}

    [Range(0, 360)]
    public float spreadAngle = 20;

    [SerializeField] public float bulletSpeed = 200;

    protected bool usePiercing = false;
    protected float piercingLength = 0;
    protected float piercingStrength = 0;
    protected float piercingTimer = 0;
    public void SetPiercing(bool ToF) 
    { 
        usePiercing = ToF;
        piercingTimer = 0;
    }
    public void SetPiercingLength(float _lengthSeconds) { piercingLength = _lengthSeconds;}
    public void SetPiercingStrength(float _percentDamageRetainedAfterHit) { piercingStrength = _percentDamageRetainedAfterHit; }

    public Coroutine cooldownRoutine;

    protected AudioManager manager;

    protected List<GameObject> enemiesInMeleeRange = new List<GameObject>();

    public List<GameObject> GetEnemiesInRange() { return enemiesInMeleeRange; }
    public void SetEnemiesInRange(List<GameObject> _enemiesInMeleeRange) { enemiesInMeleeRange = _enemiesInMeleeRange; }

    float speedUpMultiplier = 1;

    public float GetSpeedUpMultiplier() { return speedUpMultiplier; }
    public void SetSpeeUpMultiplier(float _speedUpMultiplier) { speedUpMultiplier = _speedUpMultiplier;}

    protected bool isControlLocked = false;
    public bool IsControlLocked() { return isControlLocked; }
    public void SetControlLocked(bool ToF) { isControlLocked = ToF; }

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
        if (!isControlLocked)
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

                    GameObject bulletInstance = Instantiate(bullet, spawnPos.position, rotation);
                    bulletInstance.GetComponent<Projectile>().SetDir(-bulletInstance.transform.right);

                    bulletInstance.GetComponent<Projectile>().owner = gameObject;
                    if (gameObject.tag == "Player")
                    {
                        bulletInstance.transform.localScale *= bulletScaleMultiplier;
                        bulletInstance.GetComponent<Projectile>().playerBullet = true;
                        gameObject.GetComponent<PlayerAttack>().canRoll = true;
                        gameObject.GetComponent<PlayerAttack>().rollStopCoroutine = null;
                        transform.parent.GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity();
                    }
                    else
                    {
                        bulletInstance.GetComponent<Projectile>().playerBullet = false;
                        bulletInstance.layer = 6;
                        bulletInstance.transform.localScale *= 2;
                    }

                    bulletInstance.GetComponent<Projectile>().projectileSpeed = bulletSpeed;
                    bulletInstance.GetComponent<Projectile>().SetDamage(bulletDamage / bulletAmount);

                    if (usePiercing)
                    {
                        bulletInstance.GetComponent<Projectile>().SetPiercing(true);
                        bulletInstance.GetComponent<Projectile>().SetPiercingStrength(piercingStrength);
                    }
                }

                manager.PlaySound(0);
                cooldownRoutine = StartCoroutine(ShootCooldown());
            }
        }
    }

    IEnumerator ActivatePiercing(float _timeToDeactivate)
    {
        yield return new WaitForSeconds(_timeToDeactivate);

        usePiercing = false;
    }

    protected virtual void Melee()
    {
        if (meleeDelay > 0)
        {
            meleeDelay -= Time.deltaTime;
        }

        if (!isControlLocked)
        {
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
