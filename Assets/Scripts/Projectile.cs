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

    bool usePiercing = false;
    float piercingStrength = 0;
    public void SetPiercing(bool ToF) { usePiercing = ToF; }
    public void SetPiercingStrength(float _percentDamageRetainedAfterHit) { piercingStrength = _percentDamageRetainedAfterHit; }

    List<GameObject> targetsHit = new List<GameObject>();

    float slowness = 1;
    public void SetSlowness(float _slownessDivider) { slowness = _slownessDivider; }

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
        transform.GetComponent<Rigidbody2D>().velocity = ((dir * projectileSpeed) / slowness) * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag != "Bullet")
        {
            if (!targetsHit.Contains(other.gameObject))
            {
                if (other.tag == "Wall")
                {
                    other.GetComponent<Health>().TakeDamage(damage, this);
                }
                else if (other.tag == "Enemy")
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
            }
            
            if (!usePiercing)
            {
                Destroy(gameObject);
            }
            else
            {
                targetsHit.Add(gameObject);
                damage = (piercingStrength * damage) / 100;
            }
        }
    }
}
