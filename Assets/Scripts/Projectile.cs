using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float timeToDestroy = 5;
    float timeFromSpawn = 0;

    public float projectileSpeed = 300;
    Vector2 dir;

    [SerializeField] int defaultDamage;
    [SerializeField] public bool playerBullet;

    public GameObject owner;

    float damage;

    public void SetDamage(float _damage) { damage = _damage; }

    private void Awake()
    {
        damage = defaultDamage;
    }

    public void SetDir(Vector2 _dir) { dir = _dir; }
    // Update is called once per frame
    void LateUpdate()
    {
        timeFromSpawn += Time.deltaTime;

        if (timeToDestroy <= timeFromSpawn)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag != "Bullet")
        {
            if (other.tag == "Enemy")
            {
                if (playerBullet)
                {
                    other.GetComponent<Health>().TakeDamage(damage, this);
                }
            }
            else if (other.tag == "Player")
            {
                if (!playerBullet)
                {
                    other.GetComponent<Health>().TakeDamage(damage, this);
                }
            }

            Destroy(gameObject);
        }
    }
}
