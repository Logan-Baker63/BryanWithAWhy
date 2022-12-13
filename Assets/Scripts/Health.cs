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

    GameManager gameManager;

    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public virtual void TakeDamage(float _damageToTake, Projectile collidedBullet)
    {
        if (!playerAttack.rollInvulnerable || gameObject.tag == "Enemy" || gameObject.tag == "Wall")
        {
            ParticleSystem blood = null;
            if (bloodParticlePrefab != null)
            {
                blood = Instantiate(bloodParticlePrefab, GetComponent<Collider2D>().ClosestPoint(collidedBullet.transform.position), Quaternion.identity).GetComponent<ParticleSystem>();
            }
            if (currentHealth - _damageToTake <= 0)
            {
                currentHealth = 0;

                if (collidedBullet.owner.GetComponent<PlayerAttack>())
                {
                    collidedBullet.owner.GetComponent<PlayerAttack>().AddKill(true);
                }

                healthBar.UpdateHealthBar();

                Die();
            }
            else
            {
                currentHealth -= _damageToTake;
                healthBar.UpdateHealthBar();
            }

            if (blood)
            {
                blood.Play();
            }
        }
        else
        {
            playerAttack.RollCatch();
        }
    }

    public virtual void TakeDamage(float _damageToTake, Attack attacker)
    {
        if (!playerAttack.rollInvulnerable || gameObject.tag == "Enemy" || gameObject.tag == "Wall")
        {
            ParticleSystem blood = null;
            if (bloodParticlePrefab != null)
            {
                blood = Instantiate(bloodParticlePrefab, GetComponent<Collider2D>().ClosestPoint(attacker.transform.position), Quaternion.identity).GetComponent<ParticleSystem>();
            }

            if (currentHealth - _damageToTake <= 0)
            {
                currentHealth = 0;

                // if this is enemy health
                if (attacker.GetComponent<PlayerAttack>())
                {
                    //attacker.GetComponent<PlayerAttack>().AddKill();

                    List<GameObject> enemies = attacker.GetComponent<PlayerAttack>().GetEnemiesInRange();

                    if (enemies.Contains(gameObject))
                    {
                        enemies.Remove(gameObject);
                    }

                    attacker.GetComponent<PlayerAttack>().SetEnemiesInRange(enemies);
                    attacker.GetComponent<PlayerAttack>().AddKill(false);
                }

                healthBar.UpdateHealthBar();

                Die();
            }
            else
            {
                currentHealth -= _damageToTake;
                if(GetComponent<DrawnLine>())
                {
                    GetComponent<DrawnLine>().UpdateColour(currentHealth / maxHealth);
                }
                healthBar.UpdateHealthBar();
            }

            if(blood != null)
            {
                blood.Play();
            }
        }
        else
        {
            playerAttack.RollCatch();
        }
    }

    

    public virtual void Die()
    {
        if (!gameManager.IsMoreThanOneEnemy() && gameObject.tag != "Wall")
        {
            gameManager.WaveEnd();
        }

        // checks the parent - required by the drawn walls
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
