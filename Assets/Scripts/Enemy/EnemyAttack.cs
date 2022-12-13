using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack
{
    float shootOffset = 0;
    [SerializeField] float meleeDelayTime = 0.4f;

    public void SetShootOffset(float _shootOffset) { shootOffset = _shootOffset; } 
    protected override void OnAwake()
    {
        base.OnAwake();
        StartCoroutine(WaitForOffset());
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyState>().GetState() == 0)
        {
            meleeDelay = 0;
        }
        if (GetComponent<EnemyState>().GetState() == 1)
        {
            Shoot();
        }
        else if (GetComponent<EnemyState>().GetState() == 2)
        {
            if(meleeDelay <= 0)
            {
                meleeDelay = meleeDelayTime;
            }
            Melee();
        }
    }

    IEnumerator WaitForOffset()
    {
        canAttack = false;
        yield return new WaitForSeconds(shootOffset);
        canAttack = true;
    }
}
