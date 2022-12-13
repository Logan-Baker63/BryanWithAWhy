using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSettings")]
public class WaveSettings : ScriptableObject
{
    public float enemyTypeSpawnMin = 2.5f;
    public float enemyTypeSpawnMax = 13f;

    public int startingEnemiesMin = 3;
    public int startingEnemiesMax = 6;

    public int totalEnemiesOverTimeMin = 5;
    public int totalEnemiesOverTimeMax = 10;

    public float firstEnemySpawnDelayMin = 4;
    public float firstEnemySpawnDelayMax = 6;

    public float enemySpawnDelayMin = 2;
    public float enemySpawnDelayMax = 6;
}
