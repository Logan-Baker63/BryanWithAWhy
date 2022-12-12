using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType")]
public class EnemyScriptable : ScriptableObject
{
    public GameObject prefab;
    public float ratioWeight;
}
