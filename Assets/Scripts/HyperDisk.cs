using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HyperDisk : MonoBehaviour
{

    Polygon[] polygons;
    private int[] rule;
    private int totalPolygons;
    private int innerPolygons;
    private bool alternating;

    public int p;
    public int q;
    public int density;
    public int layers;
    public Material[] materials;
    public GameObject tilePrefab;


    // Start is called before the first frame update
    void Start()
    {
        alternating =  q % 2 == 0;
        countPolygons(layers);
        DeterminePolygons();
        foreach (Polygon p in polygons)
        {
            if (p != null)
            {
                p.Create2DMesh();
            }
        }
    }

    private void countPolygons(int layer)
    {
        // Determine
        //   totalPolygons:  the number of polygons there are through that many layers
        //   innerPolygons:  the number through one less layer
        totalPolygons = 1;    // count the central polygon
        innerPolygons = 0;
        int a = p * (q - 3);  // polygons in first layer joined by a vertex
        int b = p;        // polygons in first layer joined by an edge
        int next_a, next_b;
        if (q == 3)
        {
            for (int l = 1; l <= layer; ++l)
            {
                innerPolygons = totalPolygons;
                next_a = a + b;
                next_b = (p - 6) * a + (p - 5) * b;
                totalPolygons += a + b;
                a = next_a;
                b = next_b;
            } // for
        }
        else
        { // k >= 4
            for (int l = 1; l <= layer; ++l)
            {
                innerPolygons = totalPolygons;
                next_a = ((p - 2) * (p - 3) - 1) * a
                       + ((p - 3) * (p - 3) - 1) * b;
                next_b = (p - 2) * a + (p - 3) * b;
                totalPolygons += a + b;
                a = next_a;
                b = next_b;
            } // for
        } // if/else
    } // countPolygons

    private void DeterminePolygons()
    {
        polygons = new Polygon[totalPolygons];
        rule = new int[totalPolygons];
        polygons[0] = ConstructCentrePolygon();
        rule[0] = 0;
        int j = 1; // index of the next polygon to create
        for (int i = 0; i < innerPolygons; ++i)
            j = ApplyRule(i, j);
    }

    // Used https://mathcs.clarku.edu/~djoyce/poincare/Polygon.java
    Polygon ConstructCentrePolygon()
    {
        Polygon P = Instantiate(tilePrefab).GetComponent<Polygon>();
        P.Init(p, density, materials[Random.Range(0, materials.Length)]);

        float angleA = Mathf.PI / p;
        float angleB = Mathf.PI / q;
        float angleC = Mathf.PI / 2f;

        float sinA = Mathf.Sin(angleA);
        float sinB = Mathf.Sin(angleB);
        float s = Mathf.Sin(angleC - angleB - angleA) / Mathf.Sqrt(1.0f - (sinB * sinB + sinA * sinA));

        // vertices and uv
        for (int i = 0; i < p; i++)
        {
            float a = (3 + 2 * i) * angleA;
            P.setVertex(i, new Vector2(s * Mathf.Cos(a), s * Mathf.Sin(a)));
        }

        // subdividing vertices
        
        return P;
    }

    private int ApplyRule(int i, int j)
    {
        int r = rule[i];
        bool special = (r == 1);
        if (special) r = 2;
        int start = (r == 4) ? 3 : 2;
        int quantity = (q == 3 && r != 0) ? p - r - 1 : p - r;
        for (int s = start; s < start + quantity; ++s)
        {
            // Create a polygon adjacent to P[i]
            polygons[j] = createNextPolygon(polygons[i], s % p);
            rule[j] = (q == 3 && s == start && r != 0) ? 4 : 3;
            j++;
            int m;
            if (special) m = 2;
            else if (s == 2 && r != 0) m = 1;
            else m = 0;
            for (; m < q - 3; ++m)
            {
                // Create a polygon adjacent to P[j-1]
                polygons[j] = createNextPolygon(polygons[j - 1], 1);
                rule[j] = (p == 3 && m == q - 4) ? 1 : 2;
                j++;
            } // for m
        } // for r
        return j;
    } // applyRule


    // reflect P thru the point or the side indicated by the side s
    //  to produce the resulting polygon Q
    private Polygon createNextPolygon(Polygon P, int s)
    {
        Polygon Q = Instantiate(tilePrefab).GetComponent<Polygon>();

        Q.Init(p, density, materials[Random.Range(0, materials.Length)]);

        // regular
        Line C = new Line(P.getVertex(s), P.getVertex((s + 1) % p));
        for (int i = 0; i < p; ++i)
        { // reflect P[i] thru C to get Q[j]}
            int j = (p + s - i + 1) % p;
            Q.setVertex(j, C.reflect(P.getVertex(i)));
        } // for
    // if/else
    return Q;
    } // computeNextPolygon

    // Update is called once per frame
    void Update()
    {
        
    }
}
