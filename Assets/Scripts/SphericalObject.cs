using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalObject : MonoBehaviour
{

    public float scale = 1;
    public Vector3 rotation;
    public Vector3 pos;

    public bool updat;

    private Vector3[] vecs3;
    private MeshFilter meshfilter;


    void Start()
    {
        meshfilter = GetComponent<MeshFilter>();
        vecs3 = normalizeAll(meshfilter.mesh.vertices);

        updateVecs();
        meshfilter.mesh.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(10, 10, 10));
    }

    private void Update()
    {
        if (updat)
        {
            updateVecs();
        }
    }

    private void updateVecs()
    {
        Vector3[] svecs = new Vector3[vecs3.Length];
        
        for (int i = 0; i < vecs3.Length; i++)
        {
            Vector3 v3 = Quaternion.Euler(rotation*57.3f) * vecs3[i] * scale;
            
            Vector4 v4 = SpaceManager.Instance.InverseStereographicProjection(v3);
            svecs[i] = SpaceManager.Instance.CartesianToSpherical(v4) + pos;
        }

        meshfilter.mesh.vertices = svecs;
    }
    private Vector3[] normalizeAll(Vector3[] vecs)
    {
        float mn = 10000;
        float mx = -10000;
        foreach (var v in vecs)
        {
            if (v.x > mx)
                mx = v.x;
            else if (v.x < mn)
                mn = v.x;

            if (v.y > mx)
                mx = v.y;
            else if (v.y < mn)
                mn = v.y;

            if (v.z > mx)
                mx = v.z;
            else if (v.z < mn)
                mn = v.z;
        }
        for (int i = 0; i < vecs.Length; i++)
        {
            vecs[i].x = ((vecs[i].x - mn) / (mx - mn)) - 0.5f;
            vecs[i].y = ((vecs[i].y - mn) / (mx - mn)) - 0.5f;
            vecs[i].z = ((vecs[i].z - mn) / (mx - mn)) - 0.5f;
        }
        
        return vecs;
    }
}
