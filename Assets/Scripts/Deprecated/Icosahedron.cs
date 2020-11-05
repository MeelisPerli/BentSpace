using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icosahedron
{
    public float r;
    public Triangle[] triangles;

    public Icosahedron(float r, bool norm, int layer)
    {
        this.r = r;
        GenIcosahedron(norm, layer);
        }

    private void GenIcosahedron(bool norm, int layer)
    {
        // radius, theta [0, PI], phi [0, 2PI)
        // omega between xy plane and shows height
        // phi is on xy plane

        // generating vertices
        Vector3 top = new Vector3(r, 0, 0);
        Vector3 bottom = new Vector3(r, -Mathf.PI, 0);
        Vector3[] topLayer = new Vector3[5];
        Vector3[] bottomLayer = new Vector3[5];
        for (int i = 0; i < 5; i++)
        {
            topLayer[i] = new Vector3(r, Mathf.PI / 2 - Mathf.Atan(0.5f), 2 * i * Mathf.PI / 5);
            bottomLayer[i] = new Vector3(r, Mathf.PI/2 + Mathf.Atan(0.5f), 2 * i * Mathf.PI / 5 + Mathf.PI / 5);
        }

        // making triangles
        triangles = new Triangle[20];

        // bottom triangles
        triangles[0] = new Triangle(bottom, bottomLayer[0], bottomLayer[1], norm, layer);
        triangles[1] = new Triangle(bottom, bottomLayer[1], bottomLayer[2], norm, layer);
        triangles[2] = new Triangle(bottom, bottomLayer[2], bottomLayer[3], norm, layer);
        triangles[3] = new Triangle(bottom, bottomLayer[3], bottomLayer[4], norm, layer);
        triangles[4] = new Triangle(bottom, bottomLayer[4], bottomLayer[0], norm, layer);

        // top triangles
        
        triangles[5] = new Triangle(top, topLayer[1], topLayer[0], norm, layer);
        triangles[6] = new Triangle(top, topLayer[2], topLayer[1], norm, layer);
        triangles[7] = new Triangle(top, topLayer[3], topLayer[2], norm, layer);
        triangles[8] = new Triangle(top, topLayer[4], topLayer[3], norm, layer);
        triangles[9] = new Triangle(top, topLayer[0], topLayer[4], norm, layer);
        
        // between
        for (int i = 0; i < 5; i++)
        {
            triangles[10 + i*2] = new Triangle(topLayer[i], topLayer[(i + 1) % 5], bottomLayer[i], norm, layer);
            triangles[11 + i*2] = new Triangle(bottomLayer[(i + 1) % 5], bottomLayer[i], topLayer[(i+1)%5], norm, layer);

        }
        
    }

    public void InstantiateSurfaces(Material[] materials, bool inverse)
    {
        foreach (Triangle triangle in triangles)
        {
            triangle.Instantiate(materials[Random.Range(0, materials.Length-1)], inverse);
        }    } 


    public class Triangle
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public int layer;
        public bool norm;
        public Vector3 na;
        public Vector3 nb;
        public Vector3 nc;

        private Triangle[] subTriangles;

        public Triangle(Vector3 a, Vector3 b, Vector3 c, bool norm = true, int layer = 0)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.layer = layer;
            this.norm = norm;

            if (layer > 0)
            {
                CreateSubTriangles();
                return;
            }
                

            // dont need this part for big triangles
            
            if (norm)
            {
                na = SphericalToNorm(a);
                nb = SphericalToNorm(b);
                nc = SphericalToNorm(c);
            }
            
        }

        private void CreateSubTriangles()
        {


            Vector3 ab = new Vector3(a.x, FixY(a.y, b.y), FixZ(a.z, b.z));

            Vector3 bc = new Vector3(b.x, FixY(c.y, b.y), FixZ(c.z, b.z));

            Vector3 ca = new Vector3(c.x, FixY(a.y, c.y), FixZ(a.z, c.z));


            subTriangles = new Triangle[4];
            subTriangles[0] = new Triangle(a, ab, ca, norm, layer - 1);
            subTriangles[1] = new Triangle(b, bc, ab, norm, layer - 1);
            subTriangles[2] = new Triangle(c, ca, bc, norm, layer - 1);
            subTriangles[3] = new Triangle(bc, ca, ab, norm, layer - 1);

        }

        private float FixY(float y1, float y2)
        {
            if (Mathf.Abs(y1 - y2) < Mathf.PI)
                return (y1 + y2) / 2;
            else {
                return (y1 + y2 + 2 * Mathf.PI) / 2;
            }
        }

        private float FixZ(float y1, float y2)
        {
            if (Mathf.Abs(y1 - y2) < Mathf.PI)
                return (y1 + y2) / 2;
            else
                return (y1 + y2 + 2 * Mathf.PI) / 2;
        }

        public void Instantiate(Material material, bool inverse)
        {
            if (layer != 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    subTriangles[i].Instantiate(material, inverse);
                }
            }

            GameObject obj = new GameObject();


            Mesh mesh = new Mesh();
            Vector3[] vertices;
            Vector2[] uv;
            if (norm)
            {
                vertices = new Vector3[] { na, nb, nc };
                uv = new Vector2[] { new Vector2(na.x, na.y), new Vector2(nb.x, nb.y), new Vector2(nc.x, nc.y) };
            } else
            {
                vertices = new Vector3[] { a, b, c };
                uv = new Vector2[] { new Vector2(a.x, a.y), new Vector2(b.x, b.y), new Vector2(c.x, c.y) };
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            if (!inverse)
            {
                mesh.triangles = new int[] { 2, 1, 0 };
            }
            else
            {
                mesh.triangles = new int[] { 0, 1, 2 };
            }
            

            obj.AddComponent<MeshFilter>();
            obj.GetComponent<MeshFilter>().mesh = mesh;
            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
        }

        private Vector3 SphericalToNorm(Vector3 v)
        {
            return v.x * (new Vector3(Mathf.Sin(v.y) * Mathf.Cos(v.z), Mathf.Cos(v.y), Mathf.Sin(v.y) * Mathf.Sin(v.z)));
        }
    }
}