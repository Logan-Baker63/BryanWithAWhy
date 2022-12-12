using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        CreateBoundaries();
    }

    void CreateBoundaries()
    {
        GameObject colliderLeft = new GameObject("LeftCollider", typeof(BoxCollider2D));
        GameObject colliderRight = new GameObject("RightCollider", typeof(BoxCollider2D));
        GameObject colliderTop = new GameObject("TopCollider", typeof(BoxCollider2D));
        GameObject colliderBottom = new GameObject("BottomCollider", typeof(BoxCollider2D));

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
    }
}
