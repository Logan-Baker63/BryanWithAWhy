using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] EnemyScriptable[] enemyTypes;
    [SerializeField] WaveSettings waveSettings;
    [SerializeField] Text waveCountDisplay;

    GameObject colliderLeft;
    GameObject colliderRight;
    GameObject colliderTop;
    GameObject colliderBottom;

    int dontCollideEnemyLayer;

    Vector2 leftBottomCorner;
    Vector2 rightTopCorner;

    [SerializeField] float healthReward = 0.2f; //Percentage of max

    int currentWave = 1;
    [SerializeField] public GameObject waveCounter;

    public Canvas canvas;

    [SerializeField] bool pendingWaveEnd = false;

    [SerializeField] float slowness = 10;
    bool isGameSlow = false;
    public bool IsGameSlow() { return isGameSlow; }
    public float GetSlowness() { return slowness; }
    public void SetSlowness(float _slowness) 
    { 
        slowness = _slowness; 

        Time.timeScale = _slowness;
        Time.fixedDeltaTime = 0.02f * _slowness;
    }

    public bool IsMoreThanOneEnemy() { return GameObject.FindGameObjectsWithTag("Enemy").Length > 1; }

    [SerializeField] GameObject menu;
    [SerializeField] GameObject deathUI;
    [SerializeField] GameObject pipPrefab;

    bool hasSetWaveSettings = false;

    // wave settings
    float enemyTypeSpawnMin;
    float enemyTypeSpawnMax;

    float startingEnemiesMin;
    float startingEnemiesMax;

    float totalEnemiesOverTimeMin;
    float totalEnemiesOverTimeMax;

    float firstEnemySpawnDelayMin;
    float firstEnemySpawnDelayMax;

    float enemySpawnDelayMin;
    float enemySpawnDelayMax;

    float waveSpawnMultiplier;

    float waveSpawnPercentChance;

    float enemiesPerSpawnedWaveMin;
    float enemiesPerSpawnedWaveMax;

    float enemyHPModifierMax;
    float enemyDamageModifierMax;
    float enemyHPModifier = 1;
    float enemyDamageModifier = 1;

    int wavesToIncreaseAbilityCaps;

    private void Awake()
    {
        waveCountDisplay = GameObject.FindGameObjectWithTag("WaveCount").GetComponent<Text>();
        waveCountDisplay.text = "1";

        dontCollideEnemyLayer = LayerMask.NameToLayer("DontCollideEnemy");
        
        canvas = FindObjectOfType<Canvas>();
        menu.SetActive(true);
        deathUI.SetActive(false);

        SetWaveSettings();

        CreateBoundaries();
    }

    public void StartGame()
    {
        SpawnEnemyWave();
    }

    public void LoadDeathUI()
    {
        deathUI.SetActive(true);
    }

    void CreateBoundaries()
    {
        colliderLeft = new GameObject("LeftCollider", typeof(BoxCollider2D));
        colliderRight = new GameObject("RightCollider", typeof(BoxCollider2D));
        colliderTop = new GameObject("TopCollider", typeof(BoxCollider2D));
        colliderBottom = new GameObject("BottomCollider", typeof(BoxCollider2D));

        colliderLeft.transform.SetParent(transform);
        colliderRight.transform.SetParent(transform);
        colliderTop.transform.SetParent(transform);
        colliderBottom.transform.SetParent(transform);

        leftBottomCorner = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightTopCorner = Camera.main.ViewportToWorldPoint(Vector3.one);

        colliderLeft.transform.position = new Vector2(leftBottomCorner.x - 0.5f, Camera.main.transform.position.y);
        colliderRight.transform.position = new Vector2(rightTopCorner.x + 0.5f, Camera.main.transform.position.y);
        colliderTop.transform.position = new Vector2(Camera.main.transform.position.x, rightTopCorner.y + 0.5f);
        colliderBottom.transform.position = new Vector2(Camera.main.transform.position.x, leftBottomCorner.y - 0.5f );

        colliderLeft.transform.localScale = new Vector3(1, Mathf.Abs(rightTopCorner.y - leftBottomCorner.y));
        colliderRight.transform.localScale = new Vector3(1, Mathf.Abs(rightTopCorner.y - leftBottomCorner.y));
        colliderTop.transform.localScale = new Vector3(Mathf.Abs(rightTopCorner.x - leftBottomCorner.x), 1);
        colliderBottom.transform.localScale = new Vector3(Mathf.Abs(rightTopCorner.x - leftBottomCorner.x), 1);

        colliderLeft.layer = dontCollideEnemyLayer;
        colliderRight.layer = dontCollideEnemyLayer;
        colliderTop.layer = dontCollideEnemyLayer;
        colliderBottom.layer = dontCollideEnemyLayer;

    }

    Vector2 FindEnemySpawnPos()
    {
        float rand2;
        float rand3;
        float minX = leftBottomCorner.x - 2;
        float maxX = rightTopCorner.x + 2;
        float minY = leftBottomCorner.y - 2;
        float maxY = rightTopCorner.y + 2;

        rand2 = Random.Range(minX, maxX);
        rand3 = Random.Range(minY, maxY);

        if (rand2 > minX + 2 && rand2 < maxX - 2)
        {
            if (rand3 > minY + 2 && rand3 < maxY - 2)
            {
                return FindEnemySpawnPos();
                
            }
        }

        //Debug.Log("Spawn pos: " + new Vector2(rand2, rand3));
        return new Vector2(rand2, rand3);
    }

    void SpawnEnemiesRandomPos(int _enemyAmount)
    {
        for (int i = 0; i < _enemyAmount; i++)
        {
            float randNum = Random.Range(waveSettings.enemyTypeSpawnMin, waveSettings.enemyTypeSpawnMax); // switch 2.5f to 0 if epic/legendary enemies are added

            GameObject enemy;

            if (randNum < 1)
            {
                enemy = Instantiate(enemyTypes[4].prefab, FindEnemySpawnPos(), Quaternion.identity);
            }
            else if (randNum < 2.5f)
            {
                enemy = Instantiate(enemyTypes[3].prefab, FindEnemySpawnPos(), Quaternion.identity);
            }
            else if (randNum < 5f)
            {
                enemy = Instantiate(enemyTypes[2].prefab, FindEnemySpawnPos(), Quaternion.identity);
            }
            else if (randNum < 9)
            {
                enemy = Instantiate(enemyTypes[1].prefab, FindEnemySpawnPos(), Quaternion.identity);
            }
            else
            {
                enemy = Instantiate(enemyTypes[0].prefab, FindEnemySpawnPos(), Quaternion.identity);
            }

            //enemy.GetComponent<EnemyAttack>().SetShootOffset(Random.Range(0, 0.25f));
            enemy.transform.GetChild(0).GetComponent<EnemyAttack>().SetShootOffset(Random.Range(0, 0.25f));
            enemy.transform.GetChild(0).GetComponent<Health>().currentHealth *= enemyHPModifier;
            enemy.transform.GetChild(0).GetComponent<EnemyAttack>().bulletDamage *= enemyDamageModifier;
            enemy.transform.GetChild(0).GetComponent<EnemyAttack>().meleeDamage *= enemyDamageModifier;
            enemy.transform.GetChild(0).GetComponent<EnemyAttack>().jumpAttackDamage *= enemyDamageModifier;
        }
    }

    int totalEnemiesOverTime = 0;
    void SpawnEnemyWave()
    {
        int startingEnemies = Random.Range((int)startingEnemiesMin, (int)startingEnemiesMax + 1);
        SpawnEnemiesRandomPos(startingEnemies);

        totalEnemiesOverTime = Random.Range((int)totalEnemiesOverTimeMin, (int)totalEnemiesOverTimeMax + 1);

        float firstEnemySpawnDelaySeconds = Random.Range(firstEnemySpawnDelayMin, firstEnemySpawnDelayMax);
        StartCoroutine(SpawnEnemyAtDelay(firstEnemySpawnDelaySeconds));
    }

    IEnumerator SpawnEnemyAtDelay(float _secondDelay)
    {
        if (totalEnemiesOverTime - 1 <= 0)
        {
            pendingWaveEnd = true;
        }
        
        yield return new WaitForSeconds(_secondDelay);

        totalEnemiesOverTime--;
        if (totalEnemiesOverTime > 0)
        {
            float nextEnemySpawnDelaySeconds = Random.Range(enemySpawnDelayMin, enemySpawnDelayMax);

            int randIsWave = Random.Range(1, 101);

            if (randIsWave <= waveSpawnPercentChance)
            {
                // spawn wave
                SpawnEnemiesRandomPos(Random.Range((int)enemiesPerSpawnedWaveMin, (int)enemiesPerSpawnedWaveMax));

                nextEnemySpawnDelaySeconds *= waveSpawnMultiplier;
            }
            else
            {
                // spawn single enemy
                SpawnEnemiesRandomPos(1);
            }
            
            StartCoroutine(SpawnEnemyAtDelay(nextEnemySpawnDelaySeconds));
        }
    }

    public void WaveEnd()
    {
        SetWaveSettings();
        if (pendingWaveEnd)
        {
            if((float)currentWave / (float)wavesToIncreaseAbilityCaps == 1)
            {
                IncreaseAbilityCaps();
            }
            Health playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            playerHealth.currentHealth += healthReward * playerHealth.maxHealth;
            if(playerHealth.currentHealth > playerHealth.maxHealth)
            {
                playerHealth.currentHealth = playerHealth.maxHealth;
            }
            currentWave++;
            waveCountDisplay.text = currentWave.ToString();

            GameObject temp = Instantiate(waveCounter, canvas.transform);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Wave " + (currentWave - 1);
            temp.GetComponent<Animator>().Play("WaveComplete");

            pendingWaveEnd = false;

            SpawnEnemyWave();
        }
    }

    void IncreaseAbilityCaps()
    {
        foreach(AbilityMeter meter in FindObjectsOfType<AbilityMeter>())
        {
            List<Image> pipsList = new List<Image>();
            foreach(Image img in meter.GetComponentsInChildren<Image>())
            {
                pipsList.Add(img);
            }
            Vector3 newPipPos = new Vector3(pipsList[pipsList.Count - 1].transform.position.x, pipsList[pipsList.Count - 1].transform.position.y + 0.28f, pipsList[pipsList.Count - 1].transform.position.z);
            GameObject newPip = Instantiate(pipPrefab, newPipPos, new Quaternion(0, 0, 0, 0), meter.transform);
            meter.abilitySlots.Add(newPip.GetComponent<Image>());
        }
    } 

    public void SetWaveSettings()
    {
        if (!hasSetWaveSettings)
        {
            hasSetWaveSettings = true;

            enemyTypeSpawnMin = waveSettings.enemyTypeSpawnMin;
            enemyTypeSpawnMax = waveSettings.enemyTypeSpawnMax;

            startingEnemiesMin = waveSettings.startingEnemiesMin;
            startingEnemiesMax = waveSettings.startingEnemiesMax;

            totalEnemiesOverTimeMin = waveSettings.totalEnemiesOverTimeMin;
            totalEnemiesOverTimeMax = waveSettings.totalEnemiesOverTimeMax;

            firstEnemySpawnDelayMin = waveSettings.firstEnemySpawnDelayMin;
            firstEnemySpawnDelayMax = waveSettings.firstEnemySpawnDelayMax;

            enemySpawnDelayMin = waveSettings.enemySpawnDelayMin;
            enemySpawnDelayMax = waveSettings.enemySpawnDelayMax;

            waveSpawnMultiplier = waveSettings.waveSpawnMultiplier;

            waveSpawnPercentChance = waveSettings.waveSpawnPercentChance;

            enemiesPerSpawnedWaveMin = waveSettings.enemiesPerSpawnedWaveMin;
            enemiesPerSpawnedWaveMax = waveSettings.enemiesPerSpawnedWaveMax;

            enemyHPModifierMax = waveSettings.enemyHPModifierMax;
            enemyDamageModifierMax = waveSettings.enemyDamageModifierMax;

            wavesToIncreaseAbilityCaps = waveSettings.wavesToIncreaseAbilityCaps;
        }
        else
        {
            if (startingEnemiesMin + waveSettings.startingEnemiesIncreasePerWaveMin <= waveSettings.startingEnemiesCapMin)
            {
                startingEnemiesMin += waveSettings.startingEnemiesIncreasePerWaveMin;
            }

            if (startingEnemiesMax + waveSettings.startingEnemiesIncreasePerWaveMax <= waveSettings.startingEnemiesCapMax)
            {
                startingEnemiesMax += waveSettings.startingEnemiesIncreasePerWaveMax;
            }

            if (totalEnemiesOverTimeMin + waveSettings.totalEnemiesOverTimeMinIncreasePerWave <= waveSettings.totalEnemiesOverTimeCapMin)
            {
                totalEnemiesOverTimeMin += waveSettings.totalEnemiesOverTimeMinIncreasePerWave;
            }

            if (totalEnemiesOverTimeMax + waveSettings.totalEnemiesOverTimeMaxIncreasePerWave <= waveSettings.totalEnemiesOverTimeCapMax)
            {
                totalEnemiesOverTimeMax += waveSettings.totalEnemiesOverTimeMaxIncreasePerWave;
            }

            if (enemySpawnDelayMin + waveSettings.enemySpawnDelayDecreasePerWaveMin <= waveSettings.enemySpawnDelayCapMin)
            {
                enemySpawnDelayMin -= waveSettings.enemySpawnDelayDecreasePerWaveMin;
            }

            if (enemySpawnDelayMax + waveSettings.enemySpawnDelayDecreasePerWaveMax <= waveSettings.enemySpawnDelayCapMax)
            {
                enemySpawnDelayMax -= waveSettings.enemySpawnDelayDecreasePerWaveMax;
            }

            if (enemyHPModifier + waveSettings.enemyHPModifierIncreasePerWave <= waveSettings.enemyHPModifierMax)
            {
                enemyHPModifier += waveSettings.enemyHPModifierIncreasePerWave;
            }

            if (enemyDamageModifier + waveSettings.enemyDamageModifierIncreasePerWave <= waveSettings.enemyDamageModifierMax)
            {
                enemyDamageModifier += waveSettings.enemyDamageModifierIncreasePerWave;
            }
        }
    }
}
