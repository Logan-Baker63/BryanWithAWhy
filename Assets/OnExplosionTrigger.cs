using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExplosionTrigger : MonoBehaviour
{
    public enum ExplosionType
    {
        Bomb,
        BulletExplosion,
    }
    [SerializeField] ExplosionType explosionType = ExplosionType.Bomb;

    public void SetExplosionType(ExplosionType _explosionType) { explosionType = _explosionType; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (explosionType == ExplosionType.Bomb)
            {
                FindObjectOfType<PlayerAttack>().DealBombExplosionDamage(collision.gameObject);
            }
            else if (explosionType == ExplosionType.BulletExplosion)
            {
                FindObjectOfType<PlayerAttack>().DealBulletExplosionDamage(collision.gameObject);
            }
        }
        
        
    }
}
