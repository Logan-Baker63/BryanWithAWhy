using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 5;
    float timeFromSpawn = 0;

    [SerializeField] float projectileSpeed = 300;
    Vector2 dir;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
