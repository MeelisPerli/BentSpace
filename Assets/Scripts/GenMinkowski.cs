using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GenMinkowski : MonoBehaviour
{

    public float radius;
    public int depth;
    public bool inverse;
    public bool norm;
    public Material[] materials;


    void Start()
    {
        Icosahedron icosa = new Icosahedron(radius, norm, depth);
        icosa.InstantiateSurfaces(materials, inverse);
    }


    void genFirstLayer()
    {
        /*
        Vector3[] vertices = new Vector3[verticesOnFirstLayer + 1];
        Vector2[] uv = new Vector2[verticesOnFirstLayer + 1];
        int[] triangles = new int[verticesOnFirstLayer * 3];
        Vector3[] layerVertices = new Vector3[verticesOnFirstLayer];

        // adding the tip
        vertices[0] = new Vector3();
        uv[0] = new Vector3();

        // x lenght in euclidean distance
        float x0 = (float)(Math.Cosh(edgeLen) - Math.Cosh(0));

        // radial distance between 2 vertices
        float r = (2 * Mathf.PI) / verticesOnFirstLayer;

        // heigth at x0
        float y = -x0 * x0;

        for (int i = 0; i < verticesOnFirstLayer; i++)
        {
            float a = i * r;
            Vector3 v = new Vector3(x0 * Mathf.Cos(a), y, x0 * Mathf.Sin(a));
            vertices[i + 1] = v;
            layerVertices[i] = v;
            uv[i + 1] = new Vector2(x0 * Mathf.Cos(a), x0 * Mathf.Sin(a));
        }

        // forming triangles
        int c = 0;
        for (int i = 0; i < verticesOnFirstLayer; i++)
        {
            c++;
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 2] = c;
            triangles[i * 3 + 1] = c % (verticesOnFirstLayer) + 1;
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
        */
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
