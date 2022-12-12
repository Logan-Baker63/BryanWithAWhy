using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float currentHealth = 100;

    public virtual void TakeDamage(int _damageToTake)
    {
        if (currentHealth - _damageToTake <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            currentHealth -= _damageToTake;
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
