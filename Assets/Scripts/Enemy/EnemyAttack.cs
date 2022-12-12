using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack
{
    [SerializeField] float shootOffset = 0;

    public void SetShootOffset(float _shootOffset) { shootOffset = _shootOffset; } 
    protected override void OnAwake()
    {
        base.OnAwake();
        StartCoroutine(WaitForOffset());
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyState>().GetState() == 1)
        {
            Shoot();
        }
        else if (GetComponent<EnemyState>().GetState() == 2)
        {
            Melee();
        }
    }

    IEnumerator WaitForOffset()
    {
        canAction = false;
        yield return new WaitForSeconds(shootOffset);
        canAction = true;
    }
}
