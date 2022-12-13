using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] EnemyScriptable[] enemyTypes;
    float totalEnemyWeight;

    GameObject colliderLeft;
    GameObject colliderRight;
    GameObject colliderTop;
    GameObject colliderBottom;

    int dontCollideEnemyLayer;

    private void Start()
    {
        dontCollideEnemyLayer = LayerMask.NameToLayer("DontCollideEnemy");

        foreach (EnemyScriptable type in enemyTypes)
        {
            totalEnemyWeight += type.ratioWeight;
        }
        
        CreateBoundaries();

        SpawnEnemiesRandomPos(3);
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

        Vector2 leftBottomCorner = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector2 rightTopCorner = Camera.main.ViewportToWorldPoint(Vector3.one);

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
        int rand1 = Random.Range(0, 4);
        float rand2;
        float rand3;

        Collider2D collider;

        if (rand1 == 0)
        {
            collider = colliderLeft.GetComponent<Collider2D>();
        }
        else if (rand1 == 1)
        {
            collider = colliderRight.GetComponent<Collider2D>();
        }
        else if (rand1 == 2)
        {
            collider = colliderTop.GetComponent<Collider2D>();
        }
        else
        {
            collider = colliderBottom.GetComponent<Collider2D>();
        }

        rand2 = Random.Range(collider.transform.TransformPoint(collider.bounds.min).x, collider.transform.TransformPoint(collider.bounds.max).x);
        rand3 = Random.Range(collider.transform.TransformPoint(collider.bounds.min).y, collider.transform.TransformPoint(collider.bounds.max).y);

        return new Vector2(rand2, rand3);
    }

    void SpawnEnemiesRandomPos(int _enemyAmount)
    {
        for (int i = 0; i < _enemyAmount; i++)
        {
            float randomWeight = 0;
            do
            {
                //No weight on any number?
                if (totalEnemyWeight == 0) return;
                randomWeight = Random.Range(0, totalEnemyWeight);
            }
            while (randomWeight == totalEnemyWeight);

            foreach (EnemyScriptable enemyType in enemyTypes)
            {
                // found enemy type
                if (randomWeight < enemyType.ratioWeight)
                {
                    GameObject enemy = Instantiate(enemyType.prefab, FindEnemySpawnPos(), Quaternion.identity);
                    //enemy.GetComponent<EnemyAttack>().SetShootOffset(Random.Range(0, 0.25f));
                    enemy.transform.GetChild(0).GetComponent<EnemyAttack>().SetShootOffset(Random.Range(0, 0.25f));
                }
                    
                randomWeight -= enemyType.ratioWeight;
            }
        }
    }
}
