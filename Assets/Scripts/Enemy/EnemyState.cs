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
}
