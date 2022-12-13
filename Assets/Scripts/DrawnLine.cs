using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Color damageFinalColour = Color.red;
    public Color initialColour = Color.black;

    public void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = initialColour;
        lineRenderer.endColor = initialColour;
    }

    public void UpdateColour(float healthPercent)
    {
        Color colour = Color.Lerp(damageFinalColour,initialColour, healthPercent);
        lineRenderer.material.color = colour;
        lineRenderer.startColor = colour;
        lineRenderer.endColor = colour;
    }

    public void GenerateLineCollider(float pointsSpent)
    {
        // 2D line renderer mesh

        EdgeCollider2D collider = GetComponent<EdgeCollider2D>();

        if (!collider)
        {
            collider = gameObject.AddComponent<EdgeCollider2D>();
            gameObject.tag = "Wall";
            Health wallHealth = gameObject.AddComponent<Health>();
            wallHealth.maxHealth = 60 * (int)pointsSpent;
            wallHealth.currentHealth = wallHealth.maxHealth;
        }

        List<Vector2> edges = new List<Vector2>();
        
        for (int point = 0; point < lineRenderer.positionCount; point++)
        {
            Vector3 lineRendererPoint = lineRenderer.GetPosition(point);
            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }

        collider.SetPoints(edges);
        // 3D line renderer mesh

        //MeshCollider collider = GetComponent<MeshCollider>();
        //
        //if (!collider)
        //{
        //    collider = gameObject.AddComponent<MeshCollider>();
        //}
        //
        //Mesh mesh = new Mesh();
        //GetComponent<LineRenderer>().BakeMesh(mesh, true);
        //
        //collider.sharedMesh = mesh;
    }
}
