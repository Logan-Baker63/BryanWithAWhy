using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSettings")]
public class WaveSettings : ScriptableObject
{
    [Header("Default Starting Values:")]
    public float enemyTypeSpawnMin = 2.5f;
    public float enemyTypeSpawnMax = 13f;

    public int startingEnemiesMin = 3;
    public int startingEnemiesMax = 6;

    public float totalEnemiesOverTimeMin = 5;
    public float totalEnemiesOverTimeMax = 10;

    public float firstEnemySpawnDelayMin = 4;
    public float firstEnemySpawnDelayMax = 6;

    public float enemySpawnDelayMin = 2;
    public float enemySpawnDelayMax = 6;

    public float waveSpawnMultiplier = 2;

    public float waveSpawnPercentChance = 10;

    public int enemiesPerSpawnedWaveMin = 2;
    public int enemiesPerSpawnedWaveMax = 4;

    [Header("Increases per wave:")]
    public float startingEnemiesIncreasePerWaveMin = 0.2f;
    public float startingEnemiesIncreasePerWaveMax = 0.3f;
    public float startingEnemiesCapMin = 9;
    public float startingEnemiesCapMax = 12;

    public float totalEnemiesOverTimeMinIncreasePerWave = 0.5f;
    public float totalEnemiesOverTimeMaxIncreasePerWave = 0.7f;
    public float totalEnemiesOverTimeCapMin = 20;
    public float totalEnemiesOverTimeCapMax = 30;

    public float enemySpawnDelayDecreasePerWaveMin = 0.2f;
    public float enemySpawnDelayDecreasePerWaveMax = 0.3f;
    public float enemySpawnDelayCapMin = 1;
    public float enemySpawnDelayCapMax = 1.2f;

    public float waveSpawnMultiDecreasePerWave = 0.1f;
}
