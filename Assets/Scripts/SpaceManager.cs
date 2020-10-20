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
            m.SetVector("Vector3_DF1E0349", cam.sphericalCoords);
            m.SetVector("Vector3_B41A4BCF", cam.camRot);
        }
    }

    public Vector3[] updateAllVerticesPos(Vector3[] sphericalCoords, Vector3[] old)
    {
        for (int i = 0; i < sphericalCoords.Length; i++)
        {
            Vector3 sc = sphericalCoords[i] - cam.sphericalCoords;
            old[i] = sc;
        }
        return old;
    }

    public Vector3[] AllSphericalToNormal(Vector3[] sphericalCoords, Vector3[] old)
    {
        for (int i = 0; i < sphericalCoords.Length; i++)
        {

            old[i] = SphericalToNormal(sphericalCoords[i]);
        }
        return old;
    }

    public Vector3 SphericalToNormal(Vector3 sphericalCoords)
    {
        Vector3 sloc = sphericalCoords + cam.sphericalCoords;
        Vector4 v4 = (new Vector4(
            Mathf.Cos(sloc.x),
            Mathf.Sin(sloc.x) * Mathf.Cos(sloc.y),
            Mathf.Sin(sloc.x) * Mathf.Sin(sloc.y) * Mathf.Cos(sloc.z),
            Mathf.Sin(sloc.x) * Mathf.Sin(sloc.y) * Mathf.Sin(sloc.z)
            ));

        return StereographicProjection(v4);
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
