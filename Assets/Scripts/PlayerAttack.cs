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
    [SerializeField] float staminaRegen = 1.5f; //Percentage of max stamina gained per second
    [SerializeField] float staminaJailSentence = 3;
    [SerializeField] float staminaJailTime = 0;
    [SerializeField] ParticleSystem rollParticle;
    public Coroutine rollStopCoroutine = null;
    public Coroutine rollCooldownRoutine = null;

    [SerializeField] int amountOfKillsForAbilityPoint = 4;
    [SerializeField] int amountOfDodgesForAbilityPoint = 2;
    int killsOutOfRequiredDesign = 0;
    int killsOutOfRequiredProgramming = 0;
    int dodgesOutOfRequiredArt = 0;

    [SerializeField] float defaultBombDamage = 25;
    float bombDamage;

    [SerializeField] float defaultBulletExplosionDamage = 10;
    float bulletExplosionDamage;

    PlayerMovement playerMovement;

    [SerializeField] GameObject explosionParticlePrefab;

    
    public void AddKill(bool ranged)
    {
        foreach (AbilityMeter abilityMeter in FindObjectsOfType<AbilityMeter>())
        {
            if (abilityMeter.abilityType == AbilityMeter.AbilityType.Design && ranged)
            {
                if(killsOutOfRequiredDesign >= amountOfKillsForAbilityPoint - 1)
                {
                    abilityMeter.AquireAbilityPoints(1);
                    killsOutOfRequiredDesign = 0;
                }
                else
                {
                    killsOutOfRequiredDesign += 1;
                }
            }
            else if (abilityMeter.abilityType == AbilityMeter.AbilityType.Programming && !ranged)
            {
                if(killsOutOfRequiredProgramming >= amountOfKillsForAbilityPoint - 1)
                {
                    abilityMeter.AquireAbilityPoints(1);
                    killsOutOfRequiredProgramming = 0;
                }
                else
                {
                    killsOutOfRequiredProgramming += 1;
                }
            }
        }
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        playerMovement = GetComponent<PlayerMovement>();
        rollParticle = GameObject.FindGameObjectWithTag("RollParticle").GetComponent<ParticleSystem>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (usePiercing)
        {
            piercingTimer += Time.deltaTime;

            if (piercingTimer >= piercingLength)
            {
                SetPiercing(false);
            }
        }

        if (doMelee)
        {
            Melee();
        }
        if (doShoot)
        {
            Shoot();
        }

        if (staminaJailTime <= 0)
        {
            staminaJailTime = 0;
            currentStamina += (staminaRegen * maxStamina) * Time.deltaTime;
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
            doShoot = true;
        }
        if (context.canceled)
        {
            doShoot = false;
        }
    }

    public void OnMelee(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            doMelee = true;
        }
        if (context.canceled)
        {
            doMelee= false;
        }
    }

    public void Roll(InputAction.CallbackContext context)
    {
        if (!isControlLocked)
        {
            if (context.performed && canRoll && staminaJailTime <= 0)
            {
                if (/*GetComponent<Rigidbody2D>().velocity*/transform.parent.GetComponent<Rigidbody2D>().velocity != new Vector2(0, 0))
                {
                    //GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity().normalized * rollDist;
                    transform.parent.GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity().normalized * rollDist / rollSpeed;
                }
                else
                {
                    //GetComponent<Rigidbody2D>().velocity = -transform.right * rollDist;
                    transform.parent.GetComponent<Rigidbody2D>().velocity = -transform.right * rollDist / rollSpeed;
                }

                currentStamina -= rollCost;
                if (currentStamina < 0)
                {
                    staminaJailTime = staminaJailSentence;
                    currentStamina = 0;
                }

                rollParticle.transform.position = gameObject.transform.position;
                rollParticle.transform.rotation = gameObject.transform.rotation;
                rollParticle.Play();

                rollStopCoroutine = StartCoroutine(RollStop());
                //Play sound
                rollCooldownRoutine = StartCoroutine(RollCooldown());
            }
        }
    }

    IEnumerator RollStop()
    {
        yield return new WaitForSeconds(rollSpeed);
        if (rollStopCoroutine != null)
        {
            canAttack = true;
            //GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity();
            transform.parent.GetComponent<Rigidbody2D>().velocity = GetComponent<PlayerMovement>().GetMoveVelocity();
            rollStopCoroutine = null;
        }
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
        rollCooldownRoutine = null;
    }

    public void RollCatch()
    {
        if (dodgesOutOfRequiredArt >= amountOfDodgesForAbilityPoint - 1)
        {
            dodgesOutOfRequiredArt = 0;
            foreach (AbilityMeter abilityMeter in FindObjectsOfType<AbilityMeter>())
            {
                if (abilityMeter.abilityType == AbilityMeter.AbilityType.Art)
                {
                    abilityMeter.AquireAbilityPoints(1);
                }
            }
        }
        else
        {
            dodgesOutOfRequiredArt += 1;
        }
        
        currentStamina += rollCost;
        //Play sound
    }

    public void DealBombExplosionDamage(GameObject _targetToDamage)
    {
        _targetToDamage.GetComponent<Health>().TakeDamage(bombDamage, this);
    }

    public void DealBulletExplosionDamage(GameObject _targetToDamage)
    {
        _targetToDamage.GetComponent<Health>().TakeDamage(bulletExplosionDamage, this);
    }

    public void ReleaseBomb(int _intensity)
    {
        bombDamage = defaultBombDamage + (_intensity * 5);
        
        GameObject explosion = Instantiate(explosionParticlePrefab, transform.position, transform.rotation);
        explosion.transform.GetChild(0).GetComponent<OnExplosionTrigger>().SetExplosionType(OnExplosionTrigger.ExplosionType.Bomb);
        explosion.transform.localScale = Vector3.one + (Vector3.one * ((float)_intensity / 10));
        explosion.GetComponent<ParticleSystem>().Play();
    }

    int bulletIntensity = 0;
    public void ExplodeBullet(Projectile bullet)
    {
        bulletExplosionDamage = defaultBulletExplosionDamage + (bulletIntensity * 5);

        GameObject explosion = Instantiate(explosionParticlePrefab, bullet.transform.position, transform.rotation);
        explosion.transform.GetChild(0).GetComponent<OnExplosionTrigger>().SetExplosionType(OnExplosionTrigger.ExplosionType.BulletExplosion);
        explosion.transform.localScale = (Vector3.one + (Vector3.one * ((float)bulletIntensity / 10))) / 5;
        explosion.GetComponent<ParticleSystem>().Play();
    }

    public void AquireExplosiveBulletPowerUp(int _intensity)
    {
        bulletIntensity = _intensity;

        StartCoroutine(ExplosiveBulletTime(3 * _intensity));
    }

    IEnumerator ExplosiveBulletTime(float _secondsTillTimeOut)
    {
        SetExplosiveBullets(true);

        yield return new WaitForSeconds(_secondsTillTimeOut);

        SetExplosiveBullets(false);
    }
}
