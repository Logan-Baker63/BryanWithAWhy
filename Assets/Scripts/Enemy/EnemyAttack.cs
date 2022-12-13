using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack
{
    float shootOffset = 0;
    [SerializeField] float meleeDelayTime = 0.4f;

    [SerializeField] float superJumpCooldown = 2f;
    [SerializeField] GameObject shockWaveParticlePrefab;

    [SerializeField] float jumpAttackDamage = 20;

    GameObject superJumpColliderObject;

    public void SetShootOffset(float _shootOffset) { shootOffset = _shootOffset; } 
    protected override void OnAwake()
    {
        base.OnAwake();

        if (transform.Find("SuperJumpCollider"))
        {
            superJumpColliderObject = transform.Find("SuperJumpCollider").gameObject;
        }
        
        StartCoroutine(WaitForOffset());
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        base.OnUpdate();
        
        if (GetComponent<EnemyState>().GetState() == 0)
        {
            meleeDelay = 0;
        }
        else if (GetComponent<EnemyState>().GetState() == 1)
        {
            Shoot();
        }
        else if (GetComponent<EnemyState>().GetState() == 2)
        {
            if (meleeDelay <= 0)
            {
                meleeDelay = meleeDelayTime;
            }
            Melee();
        }
        else if (GetComponent<EnemyState>().GetState() == 3)
        {
            SuperJump();
        }
    }

    IEnumerator WaitForOffset()
    {
        canAttack = false;
        yield return new WaitForSeconds(shootOffset);
        canAttack = true;
    }

    void SuperJump()
    {
        if (canAttack)
        {
            transform.parent.GetComponent<Animator>().SetTrigger("Jump");

            StartCoroutine(LandEnd(transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
            StartCoroutine(SuperJumpCooldown());
        }
    }

    IEnumerator SuperJumpCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(superJumpCooldown);
        canAttack = true;
    }

    IEnumerator LandEnd(float _animationLength)
    {
        yield return new WaitForSeconds(_animationLength);
        ParticleSystem shockwave = Instantiate(shockWaveParticlePrefab, transform.position, transform.rotation).GetComponent<ParticleSystem>();
        shockwave.Play();
        superJumpColliderObject.GetComponent<CircleCollider2D>().enabled = true;
        StartCoroutine(ColliderActiveTime(0.1f));
    }

    IEnumerator ColliderActiveTime(float _secondsActive)
    {
        yield return new WaitForSeconds(_secondsActive);

        superJumpColliderObject.GetComponent<OnSuperJumpTrigger>().SetHasHitPlayer(false);
        superJumpColliderObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    public void DealJumpDamage(GameObject _objectToHit)
    {
        _objectToHit.GetComponent<Health>().TakeDamage(jumpAttackDamage, this);
    }
}
