using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO.IsolatedStorage;
using UnityEngine;

public class Disk : MonoBehaviour
{

    public int p;
    public int q;
    public Material material;


    // Start is called before the first frame update
    void Start()
    {
        generateTile();
        generateTile().transform.position += new Vector3(1,0,0);
    }


    GameObject generateTile()
    {
        Vector3[] vertices = new Vector3[p];
        Vector2[] uv = new Vector2[p];
        int[] triangles = new int[(p - 2) * 3];

        // vertices and uv

        for (int i = 0; i < p; i++)
        {
            vertices[i] = new Vector3(Mathf.Cos(i * 2 * Mathf.PI / p - Mathf.PI), 0, Mathf.Sin(i * 2 * Mathf.PI / p - Mathf.PI));
            uv[i] = new Vector2(Mathf.Cos(i * 2 * Mathf.PI / p - Mathf.PI), Mathf.Sin(i * 2 * Mathf.PI / p - Mathf.PI));
            Debug.Log(Mathf.Cos(i * 2 * Mathf.PI / p) + " " + Mathf.Sin(i * 2 * Mathf.PI / p));
        }

        // triangles
        int c = 0;
        for (int i = 0; i < p - 2; i++)
        {
            c++;
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 2] = c;
            triangles[i * 3 + 1] = c + 1;

            Debug.Log(triangles[i * 3 + 0] + " " + triangles[i * 3 + 1] + " " + triangles[i * 3 + 2]);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;


        GameObject gameObject = new GameObject("Tile", typeof(MeshFilter), typeof(MeshRenderer));
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        gameObject.GetComponent<MeshFilter>().mesh = mesh;

        gameObject.GetComponent<MeshRenderer>().material = material;

        return gameObject;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
