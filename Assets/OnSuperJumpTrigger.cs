using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSuperJumpTrigger : MonoBehaviour
{
    bool hasHitPlayer = false;

    public void SetHasHitPlayer(bool ToF) { hasHitPlayer = ToF; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !hasHitPlayer)
        {
            hasHitPlayer = true;

            transform.parent.GetComponent<EnemyAttack>().DealJumpDamage(collision.gameObject);
        }
    }
}
