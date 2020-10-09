using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    private Vector2[] mainVertices;
    private Vector3[] vertices;
    private Vector2[] uv;

    private int p;
    private float pointDensity;
    private int[] pointDensities;

    private Material material;

    public void Init(int p, float density, Material material)
    {
        this.p = p;
        this.material = material;
        pointDensity = density;
        mainVertices = new Vector2[p];
    }

    public void Create2DMesh()
    {
        pointDensities = new int[p];
        int size = 1;

        for (int i = 0; i < p; i++)
        {
            Vector2 v1 = mainVertices[i];
            Vector2 v2 = mainVertices[(i + 1)%p];
            int points = (int)(1 + Mathf.Floor(Vector2.Distance(v1, v2) / pointDensity));
            if (points < 1) {
                points = 1;
            } else if (points > 50) {
                points = 50;
            }
            pointDensities[i] = points;
            size += points;
        }
        Debug.Log(size);
        vertices = new Vector3[size];
        uv = new Vector2[size];
        
        int[] triangles = new int[(size-1) * 3];

        int counter = 1;
        // subdividing mainVertices
        for (int i = 0; i < p; i++)
        {

            Vector2 v1 = mainVertices[i];
            Vector2 v2 = mainVertices[(i + 1) % p];
            for (int j = 0; j < pointDensities[i]; j++)
            {
                Vector2 k = Vector2.Lerp(v1, v2, j / (float)pointDensities[i]);
                Vector2 newLoc = KleinToPoincare(k);
                uv[counter] = newLoc;
                vertices[counter] = new Vector3(newLoc.x, 0, newLoc.y);
                counter ++;
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
        for (int i = 0; i < size-1; i++)
        {
            c++;
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 2] = c;
            triangles[i * 3 + 1] = c % (size - 1) + 1;
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

        GetComponent<MeshCollider>().sharedMesh = mesh;
        transform.localScale = new Vector3(4, 1, 4);
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

