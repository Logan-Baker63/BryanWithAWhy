using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    int state = 0;

    public int GetState() { return state; }
    public void SetState(int _state)
    {
        state = _state;
    }

    public enum EnemyType
    {
        Ranged,
        Melee,
        Big,
    }
    [SerializeField] EnemyType enemyType;
    public EnemyType GetEnemyType() { return enemyType; }
}
