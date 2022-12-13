using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] EnemyScriptable[] enemyTypes;
    [SerializeField] WaveSettings waveSettings;

    GameObject colliderLeft;
    GameObject colliderRight;
    GameObject colliderTop;
    GameObject colliderBottom;

    int dontCollideEnemyLayer;

    Vector2 leftBottomCorner;
    Vector2 rightTopCorner;

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

    private void Start()
    {
        dontCollideEnemyLayer = LayerMask.NameToLayer("DontCollideEnemy");
        //waveCounter.SetActive(false);
        canvas = FindObjectOfType<Canvas>();

        CreateBoundaries();

        SpawnEnemyWave();
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
        }
    }

    int totalEnemiesOverTime = 0;
    void SpawnEnemyWave()
    {
        int startingEnemies = Random.Range(waveSettings.startingEnemiesMin, waveSettings.startingEnemiesMax + 1);
        SpawnEnemiesRandomPos(startingEnemies);

        totalEnemiesOverTime = Random.Range(waveSettings.totalEnemiesOverTimeMin, waveSettings.totalEnemiesOverTimeMax + 1);

        float firstEnemySpawnDelaySeconds = Random.Range(waveSettings.firstEnemySpawnDelayMin, waveSettings.firstEnemySpawnDelayMax);
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
            SpawnEnemiesRandomPos(1);

            float nextEnemySpawnDelaySeconds = Random.Range(waveSettings.enemySpawnDelayMin, waveSettings.enemySpawnDelayMax);
            StartCoroutine(SpawnEnemyAtDelay(nextEnemySpawnDelaySeconds));
        }
    }

    public void WaveEnd()
    {
        if (pendingWaveEnd)
        {
            currentWave++;

            GameObject temp = Instantiate(waveCounter, canvas.transform);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Wave " + (currentWave - 1);
            temp.GetComponent<Animator>().Play("WaveComplete");

            pendingWaveEnd = false;

            SpawnEnemyWave();
        }
    }
}
