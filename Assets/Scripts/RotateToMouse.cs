using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    public bool canRotate = true;

    float GetAngle(Vector3 a, Vector3 b) { return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg; }

    // Update is called once per frame
    void Update()
    {
        Vector2 objectViewportPos = Camera.main.WorldToViewportPoint(transform.position);

        Vector2 mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float angle = GetAngle(Camera.main.ViewportToWorldPoint(objectViewportPos), Camera.main.ViewportToWorldPoint(mouseViewportPos));

        if (canRotate)
        {
            //GetComponent<Rigidbody2D>().MoveRotation(Quaternion.Euler(new Vector3(0f, 0f, angle)));
            transform.parent.GetComponent<Rigidbody2D>().MoveRotation(Quaternion.Euler(new Vector3(0f, 0f, angle)));
        }
    }

}
