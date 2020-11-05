using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpaceManager : MonoBehaviour
{


    public static SpaceManager Instance;

    public SphericalCam cam;
    public Material[] Smaterials;


    public int density;
    private float[] vertexSlots;



    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;

        GenHyperGrid();
    }


    void Update()
    {
        foreach (Material m in Smaterials)
        {
            m.SetMatrix("Matrix4_978C2F42", cam.m);
        }
    }

    private void GenHyperGrid()
    {
        vertexSlots = new float[density];

        for (int i = 0; i < density; i++)
        {
            vertexSlots[i] = 2 * i * Mathf.PI / density;
        }
    }

    public float getGridPos(int a)
    {
        while (a < 0)
        {
            a += density;
        }
        return vertexSlots[a % density];
    }

    public Matrix4x4 getMoveUpDownRotMat(float phi)
    {
        float s = Mathf.Sin(phi);
        float c = Mathf.Cos(phi);
        return new Matrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, c, s),
            new Vector4(0, 0, -s, c));
    }

    public Matrix4x4 getMoveLeftRightRotMat(float phi)
    {
        float s = Mathf.Sin(phi);
        float c = Mathf.Cos(phi);
        return new Matrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, c, 0, s),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, -s, 0, c));
    }

    public Matrix4x4 getMoveForwardBackRotMat(float phi)
    {
        float s = Mathf.Sin(phi);
        float c = Mathf.Cos(phi);
        return new Matrix4x4(
            new Vector4(c, 0, 0, s),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(-s, 0, 0, c));
    }


    // Cartesian to Spherical
    public Vector3[] From3DCartTo3Sphere(Vector3[] cartCoords)
    {
        Vector3[] sCoords = new Vector3[cartCoords.Length];
        for (int i = 0; i < cartCoords.Length; i++)
        {
            sCoords[i] = From3DCartTo3Sphere(cartCoords[i]);
        }
        return sCoords;
    }

    public Vector3 From3DCartTo3Sphere(Vector3 cartCoords)
    {
        return CartesianToSpherical(InverseStereographicProjection(cartCoords));
    }


    // Spherical to Cartesian
    public Vector3[] From3SphereTo3DCart(Vector3[] sphericalCoords)
    {
        Vector3[] cCoords = new Vector3[sphericalCoords.Length];
        for (int i = 0; i < sphericalCoords.Length; i++)
        {
            cCoords[i] = From3DCartTo3Sphere(sphericalCoords[i]);
        }
        return cCoords;
    }

    public Vector3 From3SphereTo3DCart(Vector3 sphericalCoords)
    {
        return StereographicProjection(SphericalToCartesian(sphericalCoords));
    }



    // Conversion between Spherical and Cartesian Systems
    private Vector4 SphericalToCartesian(Vector3 sphericalCoords)
    {

        return new Vector4(
            Mathf.Cos(sphericalCoords.x),
            Mathf.Sin(sphericalCoords.x) * Mathf.Cos(sphericalCoords.y),
            Mathf.Sin(sphericalCoords.x) * Mathf.Sin(sphericalCoords.y) * Mathf.Cos(sphericalCoords.z),
            Mathf.Sin(sphericalCoords.x) * Mathf.Sin(sphericalCoords.y) * Mathf.Sin(sphericalCoords.z)
            );
    }

    private Vector3 CartesianToSpherical(Vector4 CartCoords)
    {
        float s = CartCoords.z * CartCoords.z + CartCoords.w * CartCoords.w;
        float f3 = Mathf.Acos(CartCoords.z / Mathf.Sqrt(s));

        s += CartCoords.y * CartCoords.y;
        float f2 = Mathf.Acos(CartCoords.y / Mathf.Sqrt(s));

        s += CartCoords.x * CartCoords.x;
        float f1 = Mathf.Acos(CartCoords.x / Mathf.Sqrt(s));
        
        
        if (CartCoords.w < 0)
        {
            f3 = 2 * Mathf.PI - f3;
        }

        return new Vector3(f1, f2, f3);
    }

    // Stereograpgic projections
    private Vector4 InverseStereographicProjection(Vector3 v3)
    {

        float d = 1 + v3.x* v3.x + v3.y * v3.y + v3.z * v3.z;

        return new Vector4((2 * v3.x)/d, (2 * v3.y) / d, (2 * v3.z) / d, (d - 2) / d);
    }

    private Vector3 StereographicProjection(Vector4 v4)
    {
        float d = 1 - v4.w;
        return new Vector3(v4.x / d, v4.y / d, v4.z / d);
    }

}
