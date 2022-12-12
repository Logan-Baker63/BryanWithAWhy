using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    float GetAngle(Vector3 a, Vector3 b) { return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg; }

    // Update is called once per frame
    void Update()
    {
        Vector2 objectViewportPos = Camera.main.WorldToViewportPoint(transform.position);

        Vector2 mouseViewportPos = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float angle = GetAngle(objectViewportPos, mouseViewportPos);

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

}
