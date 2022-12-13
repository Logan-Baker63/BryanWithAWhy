using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnLine : MonoBehaviour
{
    LineRenderer lineRenderer;

    public void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void GenerateLineCollider()
    {
        // 2D line renderer mesh

        EdgeCollider2D collider = GetComponent<EdgeCollider2D>();

        if (!collider)
        {
            collider = gameObject.AddComponent<EdgeCollider2D>();
            gameObject.tag = "Wall";
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
