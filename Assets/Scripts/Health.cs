using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float currentHealth = 100;

    [SerializeField] GameObject bloodParticlePrefab;
    HealthBar healthBar;
    PlayerAttack playerAttack;

    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        playerAttack = FindObjectOfType<PlayerAttack>();
    }

    public virtual void TakeDamage(float _damageToTake, Projectile collidedBullet)
    {
        if (!playerAttack.rollInvulnerable)
        {
            ParticleSystem blood = Instantiate(bloodParticlePrefab, GetComponent<Collider2D>().ClosestPoint(collidedBullet.transform.position), Quaternion.identity).GetComponent<ParticleSystem>();

            if (currentHealth - _damageToTake <= 0)
            {
                currentHealth = 0;

                if (collidedBullet.owner.GetComponent<PlayerAttack>())
                {
                    collidedBullet.owner.GetComponent<PlayerAttack>().AddKill();
                }

                healthBar.UpdateHealthBar();

                Die();
            }
            else
            {
                currentHealth -= _damageToTake;
                healthBar.UpdateHealthBar();
            }

            blood.Play();
        }
        else
        {
            playerAttack.RollCatch();
        }
    }

    public virtual void TakeDamage(float _damageToTake, Attack attacker)
    {
        if (!playerAttack.rollInvulnerable)
        {
            ParticleSystem blood = Instantiate(bloodParticlePrefab, GetComponent<Collider2D>().ClosestPoint(attacker.transform.position), Quaternion.identity).GetComponent<ParticleSystem>();

            if (currentHealth - _damageToTake <= 0)
            {
                currentHealth = 0;

                if (attacker.GetComponent<PlayerAttack>())
                {
                    //attacker.GetComponent<PlayerAttack>().AddKill();

                    List<GameObject> enemies = attacker.GetComponent<PlayerAttack>().GetEnemiesInRange();

                    if (enemies.Contains(gameObject))
                    {
                        enemies.Remove(gameObject);
                    }

                    attacker.GetComponent<PlayerAttack>().SetEnemiesInRange(enemies);
                }

                healthBar.UpdateHealthBar();

                Die();
            }
            else
            {
                currentHealth -= _damageToTake;
                healthBar.UpdateHealthBar();
            }

            blood.Play();
        }
        else
        {
            playerAttack.RollCatch();
        }
    }

    public virtual void Die()
    {
        Destroy(transform.parent.gameObject);
    }
}
