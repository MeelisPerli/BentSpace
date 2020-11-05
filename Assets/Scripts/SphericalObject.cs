using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalObject : MonoBehaviour
{

    public float scale = 1;
    public Vector3 pos;

    private MeshFilter meshfilter;


    void Start()
    {
        meshfilter = GetComponent<MeshFilter>();
        Vector3[] vecs = meshfilter.mesh.vertices;

        for (int i = 0; i < vecs.Length; i++)
        {
            vecs[i] *= scale;
            vecs[i] += pos;
        }

        meshfilter.mesh.vertices = SpaceManager.Instance.From3DCartTo3Sphere(vecs);
        meshfilter.mesh.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));

    }

    private Vector3[] normalizeAll(Vector3[] vecs)
    {
        Vector3 mx = new Vector3();
        Vector3 mn = new Vector3();
        foreach (var v in vecs)
        {
            if (v.x > mx.x)
                mx.x = v.x;
            else if (v.x < mn.x)
                mn.x = v.x;

            if (v.y > mx.y)
                mx.y = v.y;
            else if (v.y < mn.y)
                mn.y = v.y;

            if (v.z > mx.z)
                mx.z = v.z;
            else if (v.z < mn.z)
                mn.z = v.z;
        }
        for (int i = 0; i < vecs.Length; i++)
        {
            vecs[i] = (vecs[i] - mn);
            vecs[i].x /= mx.x;
            vecs[i].y /= mx.y;
            vecs[i].z /= mx.z;
        }

        return vecs;
    }
}
