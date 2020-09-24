using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    private Vector2[] mainVertices;
    private Vector3[] vertices;
    private Vector2[] uv;

    private int p;
    private int pointDensity;

    private Material material;

    public void Init(int p, int pointDensity, Material material)
    {
        this.p = p;
        this.pointDensity = pointDensity;
        this.material = material;
        mainVertices = new Vector2[p];
    }

    public void Create2DMesh()
    {
        int size = p * pointDensity + 1;
        vertices = new Vector3[size];
        uv = new Vector2[size];
        
        int[] triangles = new int[(p * pointDensity) * 3];


        // subdividing mainVertices
        for (int i = 0; i < p; i++)
        {
            for (int j = 0; j < pointDensity; j++)
            {
                Vector2 v1 = mainVertices[i];
                Vector2 v2 = mainVertices[(i + 1) % p];
                Vector2 k = Vector2.Lerp(v1, v2, j / (float)pointDensity);
                Vector2 newLoc = KleinToPoincare(k);
                uv[i * pointDensity + j + 1] = newLoc;
                vertices[i * pointDensity + j + 1] = new Vector3(newLoc.x, 0, newLoc.y);
            }
        }

        // adding a point for easier tesselation
        Vector2 np = new Vector2();
        for (int i = 0; i < p; i++)
        {
            np += mainVertices[i];
        }
        np.x /= p;
        np.y /= p;
        np = KleinToPoincare(np);
        vertices[0] = new Vector3(np.x, 0, np.y);
        uv[0] = np;

        // forming triangles
        int c = 0;
        for (int i = 0; i < p * pointDensity; i++)
        {
            c++;
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 2] = c;
            triangles[i * 3 + 1] = c % (p * pointDensity) + 1;
        }


        // adding data to mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        gameObject.AddComponent<MeshFilter>();
        GetComponent<MeshFilter>().mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }


    Vector2 KleinToPoincare(Vector2 point)
    {
        float s = Mathf.Sqrt(point.x * point.x + point.y * point.y);
        float u = s / (1 + Mathf.Sqrt(1 - s * s));
        float a = Mathf.Atan2(point.y, point.x);
        return new Vector2(u * Mathf.Cos(a), u * Mathf.Sin(a));
    }

    public Vector2 getVertex(int i)
    {
        return mainVertices[i];
    }

    public void setVertex(int i, Vector2 p)
    {
        mainVertices[i] = p;
    }
}

