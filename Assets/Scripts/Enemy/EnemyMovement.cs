using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Movement
{
    Transform player;
    [SerializeField] float minFollowDist = 1;

    float GetAngle(Vector3 a, Vector3 b) { return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg; }

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Move(Vector2 velocity)
    {
        if (player)
        {
            // rotation
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, GetAngle(transform.position, player.position)));

            // movement
            if ((player.position - transform.position).magnitude > minFollowDist || !transform.Find("OffScreenCheck").GetComponent<Renderer>().isVisible)
            {
                GetComponent<EnemyState>().SetState(0);
                moveVelocity = (player.position - transform.position).normalized * movementSpeed * Time.fixedDeltaTime;
                //transform.GetComponent<Rigidbody2D>().velocity = moveVelocity / slowness;
                transform.parent.GetComponent<Rigidbody2D>().velocity = moveVelocity / slowness;
            }
            else
            {
                if (GetComponent<EnemyState>().GetEnemyType() == EnemyState.EnemyType.Ranged)
                {
                    GetComponent<EnemyState>().SetState(1);
                }
                else if (GetComponent<EnemyState>().GetEnemyType() == EnemyState.EnemyType.Melee)
                {
                    GetComponent<EnemyState>().SetState(2);
                }
                else if (GetComponent<EnemyState>().GetEnemyType() == EnemyState.EnemyType.Big)
                {
                    GetComponent<EnemyState>().SetState(3);
                }

                //transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                transform.parent.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }
    }

}
