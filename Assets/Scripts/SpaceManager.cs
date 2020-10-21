using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{

    public static SpaceManager Instance;
    public SphericalCam cam;
    public Material[] Smaterials;

    void Start()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    void Update()
    {
        foreach (Material m in Smaterials)
        {
            m.SetMatrix("Matrix4_978C2F42", cam.m);
        }
    }



    private Vector3 StereographicProjection(Vector4 v4)
    {
        float d = 1 - v4.w;
        return new Vector3(v4.x / d, v4.y / d, v4.z / d);
    }

    public Vector3[] MoveObjectAroundPoint(Vector3[] vecs)
    {
        for (int i = 0; i < vecs.Length; i++)
        {
            vecs[i] = Quaternion.AngleAxis(50 * Time.deltaTime, Vector3.forward) * vecs[i];
        }
        return vecs;
    }
}
