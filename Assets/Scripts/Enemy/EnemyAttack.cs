using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyState>().GetState() == 1)
        {
            
        }
    }

    void Shoot()
    {

    }
}
